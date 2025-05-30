using EncaixaAPI.Core.Entities;
using EncaixaAPI.Services;

namespace EncaixaAPI.ViewModels
{
    public class BinPacker
    {
        public List<Item> Pack(List<Item> items, Box box)
        {

            var packedItems = new List<Item>();
            var remainingSpace = new Container(box.Width, box.Height, box.Depth);

            foreach (var item in items.OrderByDescending(i => i.Volume))
            {
                if (item.Width <= remainingSpace.Width &&
                    item.Height <= remainingSpace.Height &&
                    item.Depth <= remainingSpace.Depth)
                {
                    packedItems.Add(item);
                    remainingSpace.Width -= item.Width;
                }
            }

            return packedItems;
        }
    }
}
