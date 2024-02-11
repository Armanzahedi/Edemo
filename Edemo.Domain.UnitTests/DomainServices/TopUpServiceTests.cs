using Edemo.Domain.Common;
using Edemo.Domain.ExternalServices;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Specs;
using Edemo.Domain.TopUp.ValueObjects;
using NotFoundException = Edemo.Domain.Common.Exceptions.NotFoundException;

namespace Edemo.Domain.UnitTests.DomainServices;

public class TopUpServiceTests
{
    private readonly IRepository<User.User> _userRepo;
    private readonly IRepository<TopUpBeneficiary> _beneficiaryRepo;
    private readonly IRepository<TopUpTransaction> _transactionRepo;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserBalanceService _userBalanceService;
    private readonly ITopUpOptions _topUpOptions;
    private readonly TopUpService _topUpService;

    public TopUpServiceTests()
    {
        _userRepo = Substitute.For<IRepository<User.User>>();
        _beneficiaryRepo = Substitute.For<IRepository<TopUpBeneficiary>>();
        _transactionRepo = Substitute.For<IRepository<TopUpTransaction>>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _userBalanceService = Substitute.For<IUserBalanceService>();
        _topUpOptions = Substitute.For<ITopUpOptions>();
        _topUpService = new TopUpService(_userRepo, _beneficiaryRepo, _transactionRepo, _dateTimeProvider,
            _userBalanceService, _topUpOptions);
    }

    [Fact]
    public async Task CreateBeneficiary_WhenUnderMaxLimit_ShouldAddBeneficiary()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var nickName = "Test Beneficiary";
        var phoneNumber = "971501234567";
        _topUpOptions.MaxTopUpBeneficiaries.Returns(5);
        _beneficiaryRepo.CountAsync(Arg.Any<BeneficiaryByUserIdSpec>()).Returns(1);

        // Act
        var result = await _topUpService.CreateBeneficiary(userId, nickName, phoneNumber);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.Nickname.Should().Be(Nickname.Create(nickName));
        result.PhoneNumber.Should().Be(UAEPhoneNumber.Create(phoneNumber));
        await _beneficiaryRepo.Received(1).AddAsync(Arg.Any<TopUpBeneficiary>());
    }


    [Fact]
    public async Task CreateBeneficiary_WhenAtLimit_ThrowsException()
    {
        // Arrange
        Guid userId = Guid.NewGuid();
        _beneficiaryRepo.CountAsync(Arg.Any<BeneficiaryByUserIdSpec>()).Returns(Task.FromResult(5)); // At limit
        _topUpOptions.MaxTopUpBeneficiaries.Returns(5);

        // Act
        Func<Task> act = async () => await _topUpService.CreateBeneficiary(userId, "Extra", "971501234567");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task DeleteBeneficiary_WhenExists_ShouldRemoveBeneficiary()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var beneficiary = TopUpBeneficiary.Create(userId, "test Beneficiary", "971501234567");
        _beneficiaryRepo.FirstOrDefaultAsync(Arg.Any<BeneficiaryByUserIdBeneficiaryIdSpec>()).Returns(beneficiary);

        // Act
        await _topUpService.DeleteBeneficiary(userId, beneficiaryId);

        // Assert
        await _beneficiaryRepo.Received(1).DeleteAsync(beneficiary);
    }


    [Fact]
    public async Task DeleteBeneficiary_WhenBeneficiaryNotFound_ShouldThrowNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        _beneficiaryRepo.FirstOrDefaultAsync(Arg.Any<BeneficiaryByUserIdBeneficiaryIdSpec>())
            .Returns((TopUpBeneficiary?)null);

        // Act
        Func<Task> act = async () => await _topUpService.DeleteBeneficiary(userId, beneficiaryId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Beneficiary was not found.");
    }

    [Fact]
    public async Task PerformTopUp_WhenValidRequest_ShouldCreateTransaction()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var topUpAmount = 50;
        var user = new User.User()
        {
            Id = userId
        };
        var beneficiary = TopUpBeneficiary.Create(userId, "test Beneficiary", "971501234567");
        var userBalance = new UserBalance(userId, 1000);
        _topUpOptions.AvailableTopUpAmounts.Returns(new List<decimal> { 50, 100, 200 });
        _topUpOptions.UnverifiedUserTopUpLimitPerMonthPerBeneficiary.Returns(100);
        _topUpOptions.UserTotalTopUpMonthlyLimit.Returns(100);
        _topUpOptions.TopUpTransactionFee.Returns(1);
        _userRepo.GetByIdAsync(userId).Returns(user);
        _beneficiaryRepo.FirstOrDefaultAsync(Arg.Any<BeneficiaryByUserIdBeneficiaryIdSpec>()).Returns(beneficiary);
        _userBalanceService.GetBalanceAsync(userId).Returns(userBalance);

        var monthlyTransactions = new List<decimal> { 50 };

        _transactionRepo.ListAsync(Arg.Any<TransactionsAmountByBeneficiaryId>())
            .Returns(monthlyTransactions);

        _transactionRepo.ListAsync(Arg.Any<TransactionsAmountByUserId>())
            .Returns(monthlyTransactions);
        
        // Act
        var result = await _topUpService.PerformTopUp(userId, beneficiary.Id, topUpAmount);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(userId);
        result.BeneficiaryId.Should().Be(beneficiary.Id);
        result.Amount.Should().Be(topUpAmount);
        await _transactionRepo.Received(1).AddAsync(Arg.Any<TopUpTransaction>());
        await _userBalanceService.Received(1).DebitBalanceAsync(userId, Arg.Any<DebitRequest>());
    }

    [Fact]
    public async Task PerformTopUp_WhenAmountNotValid_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var invalidAmount = 999;
        _topUpOptions.AvailableTopUpAmounts.Returns(new List<decimal> { 50, 100, 200 });

        // Act
        Func<Task> act = async () => await _topUpService.PerformTopUp(userId, beneficiaryId, invalidAmount);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("TopUp amount is not in the range of valid TopUp amounts.");
    }

    [Fact]
    public async Task PerformTopUp_WhenUserNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiaryId = Guid.NewGuid();
        var topUpAmount = 100;
        _topUpOptions.AvailableTopUpAmounts.Returns(new List<decimal> { 50, 100, 200 });
        _userRepo.GetByIdAsync(userId).Returns((User.User?)null);

        // Act
        Func<Task> act = async () => await _topUpService.PerformTopUp(userId, beneficiaryId, topUpAmount);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("User was not found.");
    }


    [Fact]
    public async Task PerformTopUp_WhenBeneficiaryDoesNotBelongToUser_ThrowsNotFoundException()
    {
        // Arrange
        _topUpOptions.AvailableTopUpAmounts.Returns(new List<decimal> { 50, 100, 200 });

        _userRepo.GetByIdAsync(Arg.Any<Guid>())!.Returns(Task.FromResult(new User.User()));
        _beneficiaryRepo.GetByIdAsync(Arg.Any<Guid>())!
            .Returns(Task.FromResult(TopUpBeneficiary.Create(Guid.NewGuid(), "test", "971501234567")));

        // Act
        Func<Task> act = async () => await _topUpService.PerformTopUp(Guid.NewGuid(), Guid.NewGuid(), 100);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Beneficiary was not found.");
    }

    [Fact]
    public async Task PerformTopUp_WhenBeneficiaryMonthlyLimitExceeded_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiary = TopUpBeneficiary.Create(userId, "test", "971501234567");
        var topUpAmount = 100;
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        var monthlyTransactions = new List<decimal> { 50, 50 };

        _transactionRepo.ListAsync(Arg.Any<TransactionsAmountByBeneficiaryId>())
            .Returns(monthlyTransactions);

        _userRepo.GetByIdAsync(userId).Returns(new User.User { Id = userId });
        _beneficiaryRepo.FirstOrDefaultAsync(Arg.Any<BeneficiaryByUserIdBeneficiaryIdSpec>())
            .Returns(beneficiary);

        _topUpOptions.UnverifiedUserTopUpLimitPerMonthPerBeneficiary.Returns(50);
        _topUpOptions.AvailableTopUpAmounts.Returns(new List<decimal> { 50, 100, 200 });

        // Act
        Func<Task> act = async () => await _topUpService.PerformTopUp(userId, beneficiary.Id, topUpAmount);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("User Monthly allowed top-up per beneficiary exceeded.");
    }

    [Fact]
    public async Task PerformTopUp_WhenUserMonthlyLimitExceeded_ShouldThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var beneficiary = TopUpBeneficiary.Create(userId, "test", "971501234567");
        var topUpAmount = 100;
        _dateTimeProvider.UtcNow.Returns(DateTime.UtcNow);
        var monthlyTransactions = new List<decimal> { 50, 50 };

        _transactionRepo.ListAsync(Arg.Any<TransactionsAmountByBeneficiaryId>())
            .Returns(monthlyTransactions);

        _transactionRepo.ListAsync(Arg.Any<TransactionsAmountByUserId>())
            .Returns(monthlyTransactions);
        
        _userRepo.GetByIdAsync(userId).Returns(new User.User { Id = userId });
        _beneficiaryRepo.FirstOrDefaultAsync(Arg.Any<BeneficiaryByUserIdBeneficiaryIdSpec>())
            .Returns(beneficiary);

        _topUpOptions.UnverifiedUserTopUpLimitPerMonthPerBeneficiary.Returns(200);
        _topUpOptions.UserTotalTopUpMonthlyLimit.Returns(100);
        _topUpOptions.AvailableTopUpAmounts.Returns(new List<decimal> { 50, 100, 200 });

        // Act
        Func<Task> act = async () => await _topUpService.PerformTopUp(userId, beneficiary.Id, topUpAmount);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("User Monthly allowed top-up exceeded.");
    }
}