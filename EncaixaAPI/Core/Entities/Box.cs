using System.ComponentModel.DataAnnotations.Schema;

namespace EncaixaAPI.Core.Entities
{
    public class Box
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }

        public decimal Volume => Width * Height * Depth;

        public int AvailableQuantity { get; set; }

        public decimal MaxWeight { get; set; } = 10m;
        public string Type { get; set; } = "Padrão";
        public bool IsActive { get; set; } = true;
    }
}
