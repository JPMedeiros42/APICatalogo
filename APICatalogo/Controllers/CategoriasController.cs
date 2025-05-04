using APICatalogo.Context;
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
        private readonly IRepository<Categoria> _repository;
        private readonly ILogger _logger;

        public CategoriasController(IRepository<Categoria> repository,ILogger<CategoriasController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetAll();

            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {

            var categoria = _repository.Get(c => c.CategoriaID == id);

            if (categoria == null)
            {
                _logger.LogWarning($"##### GET categorias/id = {id} não encontrada ####");
                return NotFound($"##### GET categorias/id = {id} não encontrada ####");
            }

            return Ok(categoria);
            
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos...");
                return BadRequest("Categoria inválida");
            }

            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoriaCriada.CategoriaID }, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaID)
            {
                _logger.LogWarning($"Categoria inválida");
                return BadRequest("Categoria inválida");
            }

            _repository.Update(categoria);
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Categoria> Delete(int id) {
            
            var categoria = _repository.Get(c => c.CategoriaID == id);

            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada");
                return NotFound($"Categoria com id = {id} não encontrada");
            }

            var categoriaExcluida = _repository.Delete(categoria);

            return Ok(categoriaExcluida);
        }
    }
}
