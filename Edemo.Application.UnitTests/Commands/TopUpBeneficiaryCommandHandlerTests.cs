using Ardalis.GuardClauses;
using Edemo.Application.Common.Exceptions;
using Edemo.Application.Common.Interfaces;
using Edemo.Application.Common.Interfaces.ExternalServices;
using Edemo.Application.TopUps.Commands.TopUpBeneficiary;
using Edemo.Domain.Common;
using Edemo.Domain.TopUp;
using Edemo.Domain.TopUp.Specs;
using Edemo.Domain.User;
using MapsterMapper;
using MediatR;

namespace Edemo.Application.UnitTests.Commands;

public class TopUpBeneficiaryCommandHandlerTests
{
    private readonly ICurrentUser _currentUser;
    private readonly TopUpService _topUpService;
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<TopUpBeneficiary> _beneficiaryRepo;
    private readonly ITopUpOptions _topUpOptions;
    private readonly IUserBalanceService _userBalanceService;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;
    private readonly TopUpBeneficiaryCommandHandler _handler;

    public TopUpBeneficiaryCommandHandlerTests()
    {
        _currentUser = Substitute.For<ICurrentUser>();
        _topUpService = Substitute.For<TopUpService>();
        _userRepo = Substitute.For<IRepository<User>>();
        _beneficiaryRepo = Substitute.For<IRepository<TopUpBeneficiary>>();
        _topUpOptions = Substitute.For<ITopUpOptions>();
        _topUpOptions.AvailableTopUpAmounts.Returns(new List<decimal>() {50,100 });
        _userBalanceService = Substitute.For<IUserBalanceService>();
        _publisher = Substitute.For<IPublisher>();
        _mapper = Substitute.For<IMapper>();
        _handler = new TopUpBeneficiaryCommandHandler(_currentUser, _topUpService, _userRepo, _beneficiaryRepo,
            _topUpOptions, _userBalanceService, _publisher, _mapper);
        
    }



}