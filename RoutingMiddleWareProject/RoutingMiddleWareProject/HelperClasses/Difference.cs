namespace RoutingMiddleWareProject.HelperClasses
{
    public class Difference
    {
        public static int Calculate(int num1, int num2)
        {
            return num1 - num2;
        }

        public static int? CalculateObject(object? num1, object? num2)
        {
            if (num1 == null || num2 == null)
                return null;

            if (int.TryParse(num1.ToString(), out var number1) && int.TryParse(num2.ToString(), out int number2))
                return number1 - number2;
            else
                return null;
        }
    }
}
