namespace EcommerceMicroserviceCase.Order.Api.Helpers;

public static class OrderHelper
{
    private static Random _random = new Random();

    public static string GenerateOrderNumber()
    {
        string datePart = DateTime.Now.ToString("yyyyMMddHHmm");
        string dateWithDash = datePart + "-";
        string randomPart = GenerateRandomDigits(7);
        
        return dateWithDash + randomPart;
    }
    
    public static string GenerateRandomDigits(int length)
    {
        char[] digits = new char[length];
        for (int i = 0; i < length; i++)
        {
            digits[i] = (char)('0' + _random.Next(10));
        }
        
        return new string(digits);
    }
}