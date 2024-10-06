using FluentValidation;

namespace NSE.Carrinho.Api.Model
{
    public class CarrinhoItem
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public string Imagem { get; set; }
        public Guid CarrinhoId { get; set; }

        /* EF Relation */
        public CarrinhoCliente CarrinhoCliente { get; set; }

        public CarrinhoItem()
        {
            Id = Guid.NewGuid();
        }

        internal void AssociarCarrinho(Guid carrinhoId)
        {
            CarrinhoId = carrinhoId;
        }

        internal decimal CalcularValor()
        {
            return ValorUnitario * Quantidade;
        }

        internal void AdicionarUnidades(int quantidade)
        {
            Quantidade += quantidade;
        }
        internal void AtualizarUnidades(int quantidade)
        {
            Quantidade = quantidade;
        }

        internal bool EhValido()
        {
            return new ItemCarrinhoValidation().Validate(this).IsValid;
        }

        public class ItemCarrinhoValidation : AbstractValidator<CarrinhoItem>
        {
            public ItemCarrinhoValidation()
            {
                RuleFor(c => c.ProdutoId).NotEqual(Guid.Empty).WithMessage("Id do produto inválido");
                RuleFor(c => c.NomeProduto).NotEmpty().WithMessage("O nome do produto não foi informado");
                RuleFor(c => c.Quantidade).GreaterThan(0).WithMessage(item => $"A quantidade mínima para o {item.NomeProduto} é 1");
                RuleFor(c => c.Quantidade).LessThanOrEqualTo(CarrinhoCliente.MAX_QUANTIDADE_ITEM)
                    .WithMessage(item => $"A quantidade máxima para o {item.NomeProduto} {CarrinhoCliente.MAX_QUANTIDADE_ITEM}");
                RuleFor(c => c.ValorUnitario).GreaterThan(0).WithMessage(item => $"O valor do {item.NomeProduto} precisa ser maior do que 0");
            }
        }
    }
}
