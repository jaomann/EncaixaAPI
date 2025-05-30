namespace EncaixaAPI.ViewModels
{
    public class Container
    {
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }

        public Container(decimal width, decimal height, decimal depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }
    }
}
