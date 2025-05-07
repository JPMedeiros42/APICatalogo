using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.DTO.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();

            if (categorias is null)
            {
                return NotFound("Não existem categorias cadastradas");
            }

            var categoriasDto = categorias.ToCategoriaDTOList();

            return Ok(categoriasDto);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {

            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaID == id);

            if (categoria == null)
            {
                _logger.LogWarning($"##### GET categorias/id = {id} não encontrada ####");
                return NotFound($"##### GET categorias/id = {id} não encontrada ####");
            }

            var categoriaDto = categoria.ToCategoriaDTO();

            return Ok(categoriaDto);
            
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Categoria inválida");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

            var novaCategoriaDto = categoriaCriada.ToCategoriaDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = novaCategoriaDto.CategoriaID }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaID)
            {
                _logger.LogWarning($"Categoria inválida");
                return BadRequest("Categoria inválida");
            }

            var categoria = categoriaDto.ToCategoria();

            var categoriaAtualizada = _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            var categoriaAtualizadaDto = categoriaAtualizada.ToCategoriaDTO();

            return Ok(categoriaAtualizadaDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id) {
            
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaID == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada");
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

            var categoriaExcluidaDTO = categoriaExcluida.ToCategoriaDTO();

            return Ok(categoriaExcluidaDTO);
        }
    }
}
