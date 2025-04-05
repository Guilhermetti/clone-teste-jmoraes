using Flunt.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.Controllers.Command.Product;
using MinhaApiComSQLite.Controllers.Validators.Product;
using MinhaApiComSQLite.Data.Repositories.Interfaces;
using MinhaApiComSQLite.Helpers;
using MinhaApiComSQLite.Models;
using MinhaApiComSQLite.Models.DTO;

namespace MinhaApiComSQLite.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController(
        IProductRepository repository,
        ICategoryRepository repositoryCategory,
        ILogger<ProductController> logger) : ControllerBase
    {
        /// <summary>
        /// Retorna todos os produtos cadastrados.
        /// </summary>
        /// <response code="200">Retorna a lista de produtos</response>
        /// <response code="404">Nenhum produto encontrado</response>
        /// <response code="500">Erro interno</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            try
            {
                var products = await repository.GetAll();
                if (!products.Any())
                {
                    logger.LogWarning("Nenhum produto cadastrado encontrado.");
                    return NotFound(new Notification
                    {
                        Key = "Produto",
                        Message = "Nenhum produto cadastrado."
                    });
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao recuperar os produtos.");
                return StatusCode(StatusCodes.Status500InternalServerError, new Notification
                {
                    Key = "ErroInterno",
                    Message = "Erro ao recuperar os produtos."
                });
            }
        }

        /// <summary>
        /// Retorna os produtos paginados.
        /// </summary>
        /// <param name="pageNumber">Número da página</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <response code="200">Retorna os produtos paginados</response>
        /// <response code="404">Nenhum produto encontrado</response>
        /// <response code="500">Erro interno</response>
        [Authorize]
        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResult<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ProductDTO>>> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await repository.GetPaged(pageNumber, pageSize);
                if (!result.Items.Any())
                {
                    logger.LogWarning("Nenhum produto encontrado na página {PageNumber}.", pageNumber);
                    return NotFound(new Notification
                    {
                        Key = "Produto",
                        Message = "Nenhum produto cadastrado."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao obter produtos paginados.");
                return StatusCode(StatusCodes.Status500InternalServerError, new Notification
                {
                    Key = "ErroInterno",
                    Message = "Erro ao obter produtos paginados."
                });
            }
        }

        /// <summary>
        /// Retorna um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <response code="200">Produto encontrado</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="500">Erro interno</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            try
            {
                var product = await repository.GetIdDTO(id);
                if (product == null)
                {
                    logger.LogWarning("Produto com ID {ProductId} não encontrado.", id);
                    return NotFound(new Notification
                    {
                        Key = "Produto",
                        Message = "Produto não encontrado."
                    });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao recuperar o produto com ID {ProductId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new Notification
                {
                    Key = "ErroInterno",
                    Message = "Erro ao recuperar o produto."
                });
            }
        }

        /// <summary>
        /// Cadastra um novo produto.
        /// </summary>
        /// <param name="command">Dados do produto</param>
        /// <response code="200">Produto cadastrado com sucesso</response>
        /// <response code="400">Erro de validação ou categoria inexistente</response>
        /// <response code="500">Erro interno</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Notification>> PostProduct(InsertProduct command)
        {
            try
            {
                command.Clear();
                command.Validate();
                if (!command.IsValid)
                {
                    logger.LogWarning("Validação falhou ao inserir produto: {@Notifications}", command.Notifications);
                    return BadRequest(command.Notifications);
                }

                var category = await repositoryCategory.GetId(command.CategoryId);
                if (category == null)
                {
                    logger.LogWarning("Categoria {CategoryId} não encontrada.", command.CategoryId);
                    return NotFound(new Notification
                    {
                        Key = "Categoria",
                        Message = "Categoria não encontrada."
                    });
                }

                var productToInsert = new Product
                {
                    Name = command.Name,
                    Description = command.Description,
                    CategoryId = command.CategoryId,
                    Price = command.Price
                };

                await repository.Insert(productToInsert);
                logger.LogInformation("Produto inserido com sucesso. ID: {ProductId}", productToInsert.Id);

                return Ok(productToInsert);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao inserir produto.");
                return StatusCode(StatusCodes.Status500InternalServerError, new Notification
                {
                    Key = "ErroInterno",
                    Message = "Erro ao inserir produto."
                });
            }
        }

        /// <summary>
        /// Atualiza um produto existente.
        /// </summary>
        /// <param name="command">Dados do produto</param>
        /// <response code="200">Produto atualizado com sucesso</response>
        /// <response code="400">Erro de validação</response>
        /// <response code="404">Produto ou categoria não encontrado</response>
        /// <response code="500">Erro interno</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutProduct(UpdateProduct command)
        {
            try
            {
                command.Clear();
                command.Validate();
                if (!command.IsValid)
                {
                    logger.LogWarning("Validação falhou ao atualizar produto: {@Notifications}", command.Notifications);
                    return BadRequest(command.Notifications);
                }

                var category = await repositoryCategory.GetId(command.CategoryId);
                if (category == null)
                {
                    logger.LogWarning("Categoria {CategoryId} não encontrada.", command.CategoryId);
                    return NotFound(new Notification
                    {
                        Key = "Categoria",
                        Message = "Categoria não encontrada."
                    });
                }

                var productToUpdate = await repository.GetId(command.Id);
                if (productToUpdate == null)
                {
                    logger.LogWarning("Produto {ProductId} não encontrado.", command.Id);
                    return NotFound(new Notification
                    {
                        Key = "Produto",
                        Message = "Produto não encontrado."
                    });
                }

                productToUpdate.Name = command.Name;
                productToUpdate.Description = command.Description;
                productToUpdate.Price = command.Price;
                productToUpdate.CategoryId = command.CategoryId;

                await repository.Update(productToUpdate);

                logger.LogInformation("Produto {ProductId} atualizado com sucesso.", productToUpdate.Id);
                return Ok(productToUpdate);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar produto {ProductId}.", command.Id);
                return StatusCode(StatusCodes.Status500InternalServerError, new Notification
                {
                    Key = "ErroInterno",
                    Message = "Erro ao atualizar produto."
                });
            }
        }

        /// <summary>
        /// Remove um produto pelo ID.
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <response code="200">Produto removido com sucesso</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="500">Erro interno</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Notification), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await repository.GetId(id);
                if (product == null)
                {
                    logger.LogWarning("Produto {ProductId} não encontrado para exclusão.", id);
                    return NotFound(new Notification
                    {
                        Key = "Produto",
                        Message = "Produto não encontrado."
                    });
                }

                await repository.Delete(product);
                logger.LogInformation("Produto {ProductId} excluído com sucesso.", id);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao excluir produto {ProductId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new Notification
                {
                    Key = "ErroInterno",
                    Message = "Erro ao excluir produto."
                });
            }
        }
    }
}
