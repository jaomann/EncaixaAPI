using EncaixaAPI.ViewModels.Packing;
using static EncaixaAPI.ViewModels.Packing.OutputOrders;

namespace EncaixaAPI.Core.Contracts
{
    public interface IPackingService
    {
        public Task<OutputOrders> PackProducts(PedidosInput input);
    }
}
