﻿using FluentValidation.Results;
using MediatR;
using NSE.Clientes.Api.Application.Events;
using NSE.Clientes.Api.Data.Repository;
using NSE.Clientes.Api.Models;
using NSE.Core.Messages;

namespace NSE.Clientes.Api.Application.Commands
{
    public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;

            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

            var clienteDb = await _clienteRepository.ObterPorCpf(message.Cpf);

            if (clienteDb != null)
            {
                AdicionarErro("Este CPF já está em uso");
                return ValidationResult;
            }

            _clienteRepository.Adicionar(cliente);
            cliente.AdicionarEvento(new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf));
            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}