namespace CubemapGenerator
{
    public class NumberMultipleRounder : IRounder
    {
        public NumberMultipleRounder(int number)
        {
            if (number <= 0)
            {
                throw new CubemapGeneratorException("Invalid a number. You should use 1 or greater");
            }

            Number = number;
        }

        public int Number { get; }

        public int Round(int value)
        {
            return value / Number * Number;
        }
    }
}