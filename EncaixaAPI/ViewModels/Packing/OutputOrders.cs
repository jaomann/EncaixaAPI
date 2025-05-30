using static EncaixaAPI.ViewModels.Packing.OutputOrders;

namespace EncaixaAPI.ViewModels.Packing
{
    public class OutputOrders
    {
        public List<PedidoOutput> pedidos { get; set; } = new List<PedidoOutput>();
        public class PedidoOutput
        {
            public int pedido_id { get; set; }
            public List<ProdutoEmCaixa> caixas { get; set; } = new List<ProdutoEmCaixa>();
        }
        public class ProdutoEmCaixa
        {
            public string caixa_id { get; set; }
            public List<string> produtos { get; set; }
            public string observacao { get; set; }
        }
        public class PedidosInput
        {
            public List<PedidoInput> pedidos { get; set; }
        }
        public class PedidoInput
        {
            public int pedido_id { get; set; }
            public List<ProdutoPedido> produtos { get; set; }
        }
        public class ProdutoPedido
        {
            public string produto_id { get; set; }
            public DimensoesProduto dimensoes { get; set; }
        }
        public class DimensoesProduto
        {
            public decimal altura { get; set; }
            public decimal largura { get; set; }
            public decimal comprimento { get; set; }
        }
    }
}
