using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Pedidos.API.Application.Queries;
using NSE.WebApi.Core.Controllers;

namespace NSE.Pedidos.API.Controllers
{
    [Authorize]
    [Route("api/v1/voucher")]
    public class VoucherController : MainController
    {
        private readonly IVoucherQueries _voucherQueries;

        public VoucherController(IVoucherQueries voucherQueries)
        {
            _voucherQueries = voucherQueries;
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> ObterPorCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return NotFound();

            var voucher = await _voucherQueries.ObterVoucherPorCodigo(codigo);

            return voucher == null ? NotFound() : CustomResponse(voucher);
        }
    }
}
