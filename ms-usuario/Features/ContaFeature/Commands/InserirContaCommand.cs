using MediatR;
using ms_usuario.Domains;
using ms_usuario.Extensions;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ContaFeature.Commands
{
    public class InserirContaCommand : IRequest<InserirContaCommandResponse>
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }

    public class InserirContaCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirContaHandler : IRequestHandler<InserirContaCommand, InserirContaCommandResponse>
    {
        private readonly IRepository<Conta> _repositoryConta;

        public InserirContaHandler
        (
            IRepository<Conta> repositoryConta
        )
        {
            _repositoryConta = repositoryConta;
        }

        public async Task<InserirContaCommandResponse> Handle
        (
            InserirContaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirContaCommand>());

            await Validator(request, cancellationToken);

            Conta conta = request.ToDomain();

            await _repositoryConta.AddAsync(conta, cancellationToken);
            await _repositoryConta.SaveChangesAsync(cancellationToken);

            InserirContaCommandResponse response = new InserirContaCommandResponse();
            response.DataCadastro = conta.DataCadastro;
            response.Id = conta.Id;

            return response;
        }

        private async Task Validator
        (
            InserirContaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<InserirContaCommand>(item => item.Nome));
            if (String.IsNullOrEmpty(request.Telefone)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Telefone));
            if (String.IsNullOrEmpty(request.Email)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Email));
            if (String.IsNullOrEmpty(request.Cpf)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Cpf));
            if (await ExistsCpfAsync(request, cancellationToken)) throw new ArgumentNullException("Cpf já cadastrado");
        }

        private async Task<bool> ExistsCpfAsync
        (
            InserirContaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryConta.ExistsAsync
                (
                    item => item.Cpf.Equals(request.Cpf),
                    cancellationToken
                );
        }
    }
}
