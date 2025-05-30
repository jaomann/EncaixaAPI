using EncaixaAPI.ViewModels;
using static EncaixaAPI.ViewModels.OutputOrders;

namespace EncaixaAPI.Core.Contracts
{
    public interface IPackingService
    {
        public Task<OutputOrders> PackProducts(PedidosInput input);
    }
}
