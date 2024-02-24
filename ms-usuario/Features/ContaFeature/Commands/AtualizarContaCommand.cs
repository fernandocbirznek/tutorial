using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ContaFeature.Commands
{
    public class AtualizarContaCommand : IRequest<AtualizarContaCommandResponse>
    {
        public long Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }

    public class AtualizarContaCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarContaHandler : IRequestHandler<AtualizarContaCommand, AtualizarContaCommandResponse>
    {
        private readonly IRepository<Conta> _repositoryConta;

        public AtualizarContaHandler
        (
            IRepository<Conta> repositoryConta
        )
        {
            _repositoryConta = repositoryConta;
        }

        public async Task<AtualizarContaCommandResponse> Handle
        (
            AtualizarContaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>());

            await Validator(request, cancellationToken);

            Conta conta = await GetFirstAsync(request, cancellationToken);
            conta.Nome = request.Nome;
            conta.Cpf = request.Cpf;
            conta.Email = request.Email;
            conta.Telefone = request.Telefone;

            await _repositoryConta.UpdateAsync(conta);
            await _repositoryConta.SaveChangesAsync(cancellationToken);

            AtualizarContaCommandResponse response = new AtualizarContaCommandResponse();
            response.DataAtualizacao = conta.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarContaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Nome)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Nome));
            if (String.IsNullOrEmpty(request.Telefone)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Telefone));
            if (String.IsNullOrEmpty(request.Email)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Email));
            if (String.IsNullOrEmpty(request.Cpf)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarContaCommand>(item => item.Cpf));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Conta não encontrada");
            if (await ExistsCpfAsync(request, cancellationToken)) throw new ArgumentNullException("Cpf já cadastrado");
        }

        private async Task<Conta> GetFirstAsync
        (
            AtualizarContaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryConta.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarContaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryConta.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsCpfAsync
        (
            AtualizarContaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryConta.ExistsAsync
                (
                    item => item.Cpf.Equals(request.Cpf) &&
                    !item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
