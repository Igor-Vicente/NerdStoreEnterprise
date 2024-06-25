using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.Api.Models;
using NSE.Core.Data;

namespace NSE.Catalogo.Api.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _catalogoContext;

        public ProdutoRepository(CatalogoContext catalogoContext)
        {
            _catalogoContext = catalogoContext;
        }

        public IUnitOfWork UnitOfWork => _catalogoContext;

        public void Adicionar(Produto produto)
        {
            _catalogoContext.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _catalogoContext.Produtos.Update(produto);
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _catalogoContext.Produtos.FindAsync(id);
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _catalogoContext.Produtos.AsNoTracking().ToListAsync();
        }

        public void Dispose()
        {
            _catalogoContext?.Dispose();
        }
    }
}
