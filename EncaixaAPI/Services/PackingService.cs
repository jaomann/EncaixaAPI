using EncaixaAPI.Core.Contracts;
using EncaixaAPI.Core.Entities;
using EncaixaAPI.ViewModels.Packing;
using static EncaixaAPI.ViewModels.Packing.OutputOrders;

namespace EncaixaAPI.Services
{
    public class PackingService : IPackingService
    {
        private IEnumerable<Box> _availableBoxes;
        private readonly IBoxService _boxService;
        public PackingService(IBoxService boxService)
        {
            _boxService = boxService;
        }

        public async Task<OutputOrders> PackProducts(PedidosInput input)
        {
            var output = new OutputOrders();
            _availableBoxes = await _boxService.GetAllBoxesAsync();
            foreach (var pedido in input.pedidos)
            {
                var pedidoOutput = new PedidoOutput { pedido_id = pedido.pedido_id };

                // Converter produtos para formato de empacotamento
                var items = pedido.produtos.Select(p => new Item(
                    p.produto_id,
                    p.dimensoes.largura,
                    p.dimensoes.altura,
                    p.dimensoes.comprimento
                )).ToList();

                // Ordenar produtos por volume (maior primeiro)
                items = items.OrderByDescending(i => i.Volume).ToList();

                // Empacotar usando First-Fit Decreasing (FFD)
                var packedItems = new List<Item>();

                foreach (var box in _availableBoxes.OrderBy(b => b.Volume))
                {
                    var binPacker = new BinPacker();
                    var result = binPacker.Pack(items.Except(packedItems).ToList(), box);

                    if (result.Any())
                    {
                        pedidoOutput.caixas.Add(new ProdutoEmCaixa
                        {
                            caixa_id = box.Name,
                            produtos = result.Select(i => i.Id).ToList()
                        });
                        packedItems.AddRange(result);
                    }

                    if (packedItems.Count == items.Count) break;
                }

                // Lidar com produtos que não couberam
                var unpackedItems = items.Except(packedItems).ToList();
                if (unpackedItems.Any())
                {
                    pedidoOutput.caixas.Add(new ProdutoEmCaixa
                    {
                        caixa_id = null,
                        produtos = unpackedItems.Select(i => i.Id).ToList(),
                        observacao = "Produto não cabe em nenhuma caixa disponível."
                    });
                }

                output.pedidos.Add(pedidoOutput);
            }

            return output;
        }
    }
}
