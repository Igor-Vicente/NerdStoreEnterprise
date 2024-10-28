using Dapper;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.Api.Models;
using NSE.Core.Data;

namespace NSE.Catalogo.Api.Data.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedResult<Produto>> ObterTodosAsync(int pageSize, int pageIndex, string query = null);
        Task<Produto> ObterPorId(Guid id);
        Task<List<Produto>> ObterProdutosPorId(string ids);

        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
    }

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

        public async Task<PagedResult<Produto>> ObterTodosAsync(int pageSize, int pageIndex, string query = null)
        {
            //var produtos = await _catalogoContext.Produtos.AsNoTracking()
            //    .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
            //    .Where(c => c.Nome.Contains(query)).OrderBy(c => c.Nome).ToListAsync();

            var sql = @$"SELECT * FROM Produtos 
                      WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%') 
                      ORDER BY [Nome] 
                      OFFSET {pageSize * (pageIndex - 1)} ROWS 
                      FETCH NEXT {pageSize} ROWS ONLY 
                      SELECT COUNT(Id) FROM Produtos 
                      WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')";

            var multi = await _catalogoContext.Database.GetDbConnection().QueryMultipleAsync(sql, new { Nome = query });

            var produtos = multi.Read<Produto>();
            var total = multi.Read<int>().FirstOrDefault();

            return new PagedResult<Produto>()
            {
                List = produtos,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };
        }

        public async Task<List<Produto>> ObterProdutosPorId(string ids)
        {
            var idsGuid = ids.Split(',').Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Produto>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _catalogoContext.Produtos.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Ativo).ToListAsync();
        }

        public void Dispose()
        {
            _catalogoContext?.Dispose();
        }
    }
}
