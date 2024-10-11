namespace NSE.Bff.Compras.Models
{
    public class ItemCarrinhoDTO
    {
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public string Imagem { get; set; }
        public int Quantidade { get; set; }
    }
}