﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.Api.Data.Repository;
using NSE.Catalogo.Api.Models;
using NSE.WebApi.Core.Controllers;

namespace NSE.Catalogo.Api.Controllers
{
    [Route("api/v1/catalogo")]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [AllowAnonymous]
        [HttpGet("produtos")]
        public async Task<PagedResult<Produto>> TodosProdutos([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string q = null)
        {
            return await _produtoRepository.ObterTodosAsync(ps, page, q);
        }

        [HttpGet("produtos/{id:guid}")]
        public async Task<IActionResult> ProdutoDetalhes(Guid id)
        {
            var produto = await _produtoRepository.ObterPorId(id);
            return CustomResponse(produto);
        }

        [HttpGet("produtos/lista/{ids}")]
        public async Task<IActionResult> ObterProdutosPorId(string ids)
        {
            var produtos = await _produtoRepository.ObterProdutosPorId(ids);
            return CustomResponse(produtos);
        }
    }
}
