namespace TSCubemapGenerator.Window
{
    public class CubemapConverterSelector
    {
        private readonly Cubemap2HorizontalPngConverter _horizontalPngConverter = new Cubemap2HorizontalPngConverter();
        private readonly Cubemap2VerticalPngConverter _verticalPngConverter = new Cubemap2VerticalPngConverter();

        public ICubemapConverter Select(OutputImageType fileType)
        {
            switch (fileType)
            {
                case OutputImageType.HorizontalPng:
                    return _horizontalPngConverter;
                default:
                    return _verticalPngConverter;
            }
        }
    }
}