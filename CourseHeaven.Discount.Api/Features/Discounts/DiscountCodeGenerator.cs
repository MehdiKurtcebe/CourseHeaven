namespace CourseHeaven.Discount.Api.Features.Discounts;

public class DiscountCodeGenerator
{
    private static readonly Random Rand = new Random();

    public static string GenerateDiscountCode(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Rand.Next(s.Length)]).ToArray());
    }
}