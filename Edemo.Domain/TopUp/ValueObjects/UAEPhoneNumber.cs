using Edemo.Domain.Common;
using PhoneNumbers;

namespace Edemo.Domain.TopUp.ValueObjects;

public class UAEPhoneNumber : ValueObject
{
    public string Number { get; }

    private UAEPhoneNumber()
    {
        
    }
    private UAEPhoneNumber(string value)
    {
        Number = value;
    }
    public static UAEPhoneNumber Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Phone number cannot be empty.");

        if (!IsValidUaePhoneNumber(number))
            throw new ArgumentException("Phone number is not in a valid UAE format.");

        return new UAEPhoneNumber(number);
    }

    private static bool IsValidUaePhoneNumber(string number)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var phoneNumber = phoneNumberUtil.Parse(number, "AE");
            return phoneNumberUtil.IsValidNumber(phoneNumber) && phoneNumberUtil.GetRegionCodeForNumber(phoneNumber) == "AE";
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Number;
    }

    public override string ToString() => Number;
    public static implicit operator string(UAEPhoneNumber phoneNumber)
    {
        return phoneNumber.ToString();
    }
}