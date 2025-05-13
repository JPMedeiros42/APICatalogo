using APICatalogo.Context;
using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork uof, IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet("categorias/{id}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosPorCategoriaAsync(id);

            if (produtos is null || !produtos.Any())
                return NotFound("Nenhum produto encontrado");

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosAsync(produtosParameters);
            var metadata = new
            {
                produtos.TotalItemCount,
                produtos.PageSize,
                produtos.PageNumber,
                produtos.PageCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination",
                System.Text.Json.JsonSerializer.Serialize(metadata));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(IPagedList<Produto> produtos)
        {
            var metadata = new
            {
                produtos.TotalItemCount,
                produtos.PageSize,
                produtos.PageNumber,
                produtos.PageCount,
                produtos.HasNextPage,
                produtos.HasPreviousPage
            };

            Response.Headers.Append("X-Pagination",
                System.Text.Json.JsonSerializer.Serialize(metadata));

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("filter/preco/pagination")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosFilterPrecoAsync([FromQuery] ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFiltroParams);
            return ObterProdutos(produtos);
        }

        // /produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync()
        {
            var produtos = await _uof.ProdutoRepository.GetAllAsync();

            if (produtos is null || !produtos.Any())
                return NotFound("Nenhum produto encontrado");

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);

        }

        [HttpGet("{id}", Name ="ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> GetByIdAsync(int id)
        {
            var produto = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado");

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoDTO>> PostAsync(ProdutoDTO produtoDto)
        {
            if(produtoDto is null)
                return BadRequest("Produto inválido");

            var produto = _mapper.Map<Produto>(produtoDto);

            var novoProduto = _uof.ProdutoRepository.Create(produto);
            await _uof.CommitAsync();

            var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = novoProduto.ProdutoId }, novoProduto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> PutAsync(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest("Produto diferente do informado");

            var produtoExistente = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            if(produtoExistente is null)
                return NotFound("Produto não encontrado");

            var produto = _mapper.Map<Produto>(produtoDto);

            var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
            await _uof.CommitAsync();

            var produtoDtoAtualizado = _mapper.Map<ProdutoDTO>(produtoAtualizado);

            return Ok(produtoAtualizado);

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> DeleteAsync(int id)
        {
            var produto = await _uof.ProdutoRepository.GetAsync(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound("Produto não encontrado");

            var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
            await _uof.CommitAsync();

            var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produtoDeletado);

            return Ok(produtoDeletadoDto);

        }
    }
}
