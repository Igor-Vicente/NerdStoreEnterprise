using NSE.Core.Data;

namespace NSE.Catalogo.Api.Models
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto> ObterPorId(Guid id);
        void Adicionar(Produto produto);
        void Atualizar(Produto produto);
    }
}
