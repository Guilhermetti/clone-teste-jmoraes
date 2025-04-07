using Flunt.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaApiComSQLite.Controllers.Validators.Category;
using MinhaApiComSQLite.Data.Repositories.Interfaces;
using MinhaApiComSQLite.Models;

namespace MinhaApiComSQLite.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController(
        ICategoryRepository repository,
        ILogger<CategoryController> logger) : ControllerBase
    {
        /// <summary>
        /// Obtém todas as categorias.
        /// </summary>
        /// <returns>Lista de categorias.</returns>
        /// <response code="200">Categorias retornadas com sucesso.</response>
        /// <response code="404">Nenhuma categoria encontrada.</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Category>), 200)]
        [ProducesResponseType(typeof(Notification), 404)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                var categories = await repository.GetAll();
                if (!categories.Any())
                {
                    logger.LogWarning("Nenhuma categoria cadastrada.");
                    return NotFound(new Notification
                    {
                        Key = "Categoria",
                        Message = "Nenhuma categoria cadastrada."
                    });
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao recuperar categorias.");
                throw;
            }
        }

        /// <summary>
        /// Obtém uma categoria específica pelo ID.
        /// </summary>
        /// <param name="id">ID da categoria.</param>
        /// <returns>Categoria encontrada.</returns>
        /// <response code="200">Categoria encontrada com sucesso.</response>
        /// <response code="404">Categoria não encontrada.</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(typeof(Notification), 404)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var category = await repository.GetIdDTO(id);
                if (category == null)
                {
                    logger.LogWarning("Categoria com ID {CategoryId} não encontrada.", id);
                    return NotFound(new Notification
                    {
                        Key = "Categoria",
                        Message = "Categoria não encontrada."
                    });
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao recuperar categoria com ID {CategoryId}.", id);
                throw;
            }
        }

        /// <summary>
        /// Cadastra uma nova categoria.
        /// </summary>
        /// <param name="command">Dados da categoria.</param>
        /// <returns>Categoria cadastrada.</returns>
        /// <response code="200">Categoria criada com sucesso.</response>
        /// <response code="400">Erro de validação ou categoria já existente.</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), 400)]
        public async Task<ActionResult<Category>> PostCategory(InsertCategory command)
        {
            try
            {
                command.Clear();
                command.Validate();
                if (!command.IsValid)
                {
                    logger.LogWarning("Validação falhou ao tentar inserir categoria. Notificações: {@Notifications}", command.Notifications);
                    return BadRequest(command.Notifications);
                }

                command.Name = StringHelper.Capitalize(command.Name);

                var category = await repository.GetName(command.Name);
                if (category != null)
                {
                    logger.LogWarning("Tentativa de inserir categoria já existente com nome: {CategoryName}", command.Name);
                    return BadRequest(new Notification
                    {
                        Key = "Categoria",
                        Message = "Já existe uma categoria com esse nome."
                    });
                }

                var categoryToInsert = new Category { Name = command.Name };
                await repository.Insert(categoryToInsert);

                logger.LogInformation("Categoria '{CategoryName}' inserida com sucesso. ID: {CategoryId}", command.Name, categoryToInsert.Id);

                return Ok(categoryToInsert);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao inserir nova categoria.");
                throw;
            }
        }

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        /// <param name="command">Dados atualizados da categoria.</param>
        /// <returns>Categoria atualizada.</returns>
        /// <response code="200">Categoria atualizada com sucesso.</response>
        /// <response code="400">Erro de validação ou nome já em uso.</response>
        /// <response code="404">Categoria não encontrada.</response>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), 400)]
        [ProducesResponseType(typeof(Notification), 404)]
        public async Task<ActionResult<Category>> PutCategory(UpdateCategory command)
        {
            try
            {
                command.Clear();
                command.Validate();
                if (!command.IsValid)
                {
                    logger.LogWarning("Validação falhou ao atualizar categoria. Notificações: {@Notifications}", command.Notifications);
                    return BadRequest(command.Notifications);
                }

                command.Name = StringHelper.Capitalize(command.Name);

                var existingCategory = await repository.GetName(command.Name);
                if (existingCategory != null && existingCategory.Id != command.Id)
                {
                    logger.LogWarning("Tentativa de renomear categoria para um nome já utilizado: {CategoryName}", command.Name);
                    return BadRequest(new Notification
                    {
                        Key = "Categoria",
                        Message = "Já existe uma categoria com esse nome."
                    });
                }

                var categoryToUpdate = await repository.GetId(command.Id);
                if (categoryToUpdate == null)
                {
                    logger.LogWarning("Categoria com ID {CategoryId} não encontrada para atualização.", command.Id);
                    return NotFound(new Notification
                    {
                        Key = "Categoria",
                        Message = "Categoria não encontrada."
                    });
                }

                categoryToUpdate.Name = command.Name;
                await repository.Update(categoryToUpdate);

                logger.LogInformation("Categoria com ID {CategoryId} atualizada com sucesso.", command.Id);

                return Ok(categoryToUpdate);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar categoria com ID {CategoryId}.", command.Id);
                throw;
            }
        }

        /// <summary>
        /// Remove uma categoria pelo ID.
        /// </summary>
        /// <param name="id">ID da categoria a ser excluída.</param>
        /// <returns>Status da operação.</returns>
        /// <response code="200">Categoria excluída com sucesso.</response>
        /// <response code="404">Categoria não encontrada.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(Notification), 404)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await repository.GetId(id);
                if (category == null)
                {
                    logger.LogWarning("Categoria com ID {CategoryId} não encontrada para exclusão.", id);
                    return NotFound(new Notification
                    {
                        Key = "Categoria",
                        Message = "Categoria não encontrada."
                    });
                }

                await repository.Delete(category);

                logger.LogInformation("Categoria com ID {CategoryId} excluída com sucesso.", id);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao excluir categoria com ID {CategoryId}.", id);
                throw;
            }
        }
    }
}
