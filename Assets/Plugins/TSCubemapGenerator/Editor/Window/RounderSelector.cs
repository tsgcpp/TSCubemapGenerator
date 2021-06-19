namespace TSCubemapGenerator.Window
{
    public class RounderSelector
    {
        private readonly NumberMultipleRounder _numberMultipleRounder = new NumberMultipleRounder(number: 4);
        private readonly NumberPowerRounder _numberPowerRounder = new NumberPowerRounder(number: 2);
        private readonly SkipRounder _skipRounder = new SkipRounder();

        public IRounder Select(SizeType sizeType)
        {
            switch (sizeType)
            {
                case SizeType.MultipleOf4:
                    return _numberMultipleRounder;
                case SizeType.PowerOf2:
                    return _numberPowerRounder;
                default:
                    return _skipRounder;
            }
        }
    }
}