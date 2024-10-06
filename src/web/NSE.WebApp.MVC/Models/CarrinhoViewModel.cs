namespace NSE.WebApp.MVC.Models
{
    public class CarrinhoViewModel
    {
        public decimal ValorTotal { get; set; }
        public List<ItemProdutoViewModel> Itens { get; set; } = new List<ItemProdutoViewModel>();
    }

    public class ItemProdutoViewModel
    {
        public Guid ProdutoId { get; set; }
        public string? NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public string? Imagem { get; set; }
    }
}
