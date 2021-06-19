namespace TSCubemapGenerator
{
    public class SkipRounder : IRounder
    {
        public int Round(int value)
        {
            return value;
        }
    }
}