using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.DTO.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork uof, ILogger<CategoriasController> logger)
        {
            _uof = uof;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAllAsync()
        {
            var categorias = await _uof.CategoriaRepository.GetAllAsync();

            if (categorias is null)
            {
                return NotFound("Não existem categorias cadastradas");
            }

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("Paginacao")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetByPaginationAsync([FromQuery] CategoriasParameters categoriasParams)
        {
            var categorias = await _uof.CategoriaRepository.GetCategoriasPaginationAsync(categoriasParams);

            var metadata = new
            {
                categorias.TotalItemCount,
                categorias.PageSize,
                categorias.PageNumber,
                categorias.PageCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("Filter/Pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetFilterPaginationAsync([FromQuery] CategoriasFiltroNome categoriasParams)
        {
            var categoriasFiltradas = await _uof.CategoriaRepository.GetFilterPaginationAsync(categoriasParams);

            return ObterCategorias(categoriasFiltradas);
        }

        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalItemCount,
                categorias.PageSize,
                categorias.PageNumber,
                categorias.PageCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(metadata));

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
        {

            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaID == id);

            if (categoria == null)
            {
                _logger.LogWarning($"##### GET categorias/id = {id} não encontrada ####");
                return NotFound($"##### GET categorias/id = {id} não encontrada ####");
            }

            var categoriaDto = categoria.ToCategoriaDTO();

            return Ok(categoriaDto);
            
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDTO>> PostAsync(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Categoria inválida");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            await _uof.CommitAsync();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDto.CategoriaID }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> PutAsync(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaID)
            {
                _logger.LogWarning($"Categoria inválida");
                return BadRequest("Categoria inválida");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            await _uof.CommitAsync();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> DeleteAsync(int id) {
            
            var categoria = await _uof.CategoriaRepository.GetAsync(c => c.CategoriaID == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada");
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            await _uof.CommitAsync();

            var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);
        }
    }
}
