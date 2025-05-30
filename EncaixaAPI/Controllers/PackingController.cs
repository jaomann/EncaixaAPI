using EncaixaAPI.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EncaixaAPI.ViewModels.OutputOrders;

namespace EncaixaAPI.Controllers
{
    [ApiController]
    [Route("api/packing")]
    [Authorize]
    public class PackingController : ControllerBase
    {
        private readonly IPackingService _packingService;

        public PackingController(IPackingService packingService)
        {
            _packingService = packingService;
        }

        [HttpPost("optimize")]
        public async Task<IActionResult> OptimizePacking([FromBody] PedidosInput input)
        {
            try
            {
                if (input?.pedidos == null || !input.pedidos.Any())
                {
                    return BadRequest("Nenhum pedido recebido");
                }

                var result = await _packingService.PackProducts(input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar pedidos: {ex.Message}");
            }
        }
    }
}
