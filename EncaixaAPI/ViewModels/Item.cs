namespace EncaixaAPI.ViewModels
{
    public class Item
    {
        public string Id { get; }
        public decimal Width { get; }
        public decimal Height { get; }
        public decimal Depth { get; }
        public decimal Volume => Width * Height * Depth;

        public Item(string id, decimal width, decimal height, decimal depth)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
        }
    }
}
