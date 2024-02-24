using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ContaFeature.Queries
{
    public class SelecionarContaByIdQuery : IRequest<SelecionarContaByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarContaByIdQueryResponse : Entity
    {
        public long Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }

    public class SelecionarContaByIdQueryHandler : IRequestHandler<SelecionarContaByIdQuery, SelecionarContaByIdQueryResponse>
    {
        private readonly IRepository<Conta> _repository;

        public SelecionarContaByIdQueryHandler
        (
            IRepository<Conta> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarContaByIdQueryResponse> Handle
        (
            SelecionarContaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarContaByIdQuery>());

            Conta conta = await GetFirstAsync(request, cancellationToken);

            Validator(conta, cancellationToken);

            SelecionarContaByIdQueryResponse response = new SelecionarContaByIdQueryResponse();

            response.Cpf = conta.Cpf;
            response.Nome = conta.Nome;
            response.Email = conta.Email;
            response.Telefone = conta.Telefone;
            response.DataCadastro = conta.DataCadastro;
            response.DataAtualizacao = conta.DataAtualizacao;
            response.Id = conta.Id;

            return response;
        }

        private async void Validator
        (
            Conta conta,
            CancellationToken cancellationToken
        )
        {
            if (conta is null) throw new ArgumentNullException("Conta não encontrada");
        }

        private async Task<Conta> GetFirstAsync
        (
            SelecionarContaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
