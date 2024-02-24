using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ContaFeature.Commands
{
    public class RemoverContaCommand : IRequest<RemoverContaCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverContaCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverContaCommandHandler : IRequestHandler<RemoverContaCommand, RemoverContaCommandResponse>
    {
        private readonly IRepository<Conta> _repository;

        public RemoverContaCommandHandler
        (
            IRepository<Conta> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverContaCommandResponse> Handle
        (
            RemoverContaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverContaCommand>());

            await Validator(request, cancellationToken);

            Conta conta = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(conta);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverContaCommandResponse response = new RemoverContaCommandResponse();
            response.Id = conta.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverContaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Conta não encontrada");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverContaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
