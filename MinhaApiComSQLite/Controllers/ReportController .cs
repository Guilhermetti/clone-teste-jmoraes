using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.Data.Repositories.Interfaces;
using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Controllers
{
    [ApiController]
    [Route("api/summary")]
    public class ReportController(
        IReportRepository repository,
        ILogger<ReportController> logger) : ControllerBase
    {
        /// <summary>
        /// Obtém estatísticas gerais sobre os produtos e categorias cadastrados.
        /// </summary>
        /// <returns>Resumo contendo total de produtos, média de preços, valor total, total de categorias e produtos por categoria.</returns>
        /// <response code="200">Resumo retornado com sucesso.</response>
        /// <response code="404">Nenhum dado encontrado para gerar o resumo.</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> GetSummary()
        {
            int totalProducts = await repository.GetTotalProducts();
            decimal averagePrice = await repository.GetAveragePrice();
            decimal totalValue = await repository.GetTotalValue();
            int totalCategories = await repository.GetTotalCategories();
            List<SummaryDTO> productsPerCategory = await repository.GetProductsPerCategory();

            return Ok(new
            {
                totalProducts,
                averagePrice,
                totalValue,
                totalCategories,
                productsPerCategory
            });
        }
    }
}
