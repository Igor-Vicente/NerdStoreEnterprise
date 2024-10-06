﻿using FluentValidation;
using FluentValidation.Results;

namespace NSE.Carrinho.Api.Model
{
    public class CarrinhoCliente
    {
        internal const int MAX_QUANTIDADE_ITEM = 5;
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Itens { get; set; } = new();
        public ValidationResult ValidationResult { get; set; }
        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }

        public CarrinhoCliente()
        { }

        internal CarrinhoItem ObterPorProdutoId(Guid produtoId)
        {
            return Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        }

        internal void AdicionarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);
            var itemExistenteNoCarrinho = ObterPorProdutoId(item.ProdutoId);
            if (itemExistenteNoCarrinho != null)
            {
                itemExistenteNoCarrinho.AdicionarUnidades(item.Quantidade);
            }
            else
            {
                Itens.Add(item);
            }

            CalcularValorTotalCarrinho();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);
            var itemExistenteNoCarrinho = ObterPorProdutoId(item.ProdutoId);
            Itens.Remove(itemExistenteNoCarrinho);
            Itens.Add(item);
            CalcularValorTotalCarrinho();
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            var itemExistenteCarrinho = ObterPorProdutoId(item.ProdutoId);
            Itens.Remove(itemExistenteCarrinho);
            CalcularValorTotalCarrinho();
        }

        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        internal void CalcularValorTotalCarrinho()
        {
            ValorTotal = Itens.Sum(i => i.CalcularValor());
        }

        internal bool EhValido()
        {
            var erros = Itens.SelectMany(i => new CarrinhoItem.ItemCarrinhoValidation().Validate(i).Errors).ToList();
            erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }

        public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
        {
            public CarrinhoClienteValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precisa ser maior que 0");
            }
        }
    }
}