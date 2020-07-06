namespace Structure.Extensions
{
    public static class IntegerExtensions
    {
        public static bool EqualsAndNotDefault(this int value, int valueToCompare)
        {
            return value == valueToCompare && value != default(int);
        }

        public static bool In(this int value, params int[] values)
        {
            foreach (var item in values)
            {
                if (item == value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
