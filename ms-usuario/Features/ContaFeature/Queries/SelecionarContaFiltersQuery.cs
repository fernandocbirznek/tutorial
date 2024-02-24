using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.ContaFeature.Queries
{
    public class SelecionarContaFiltersQuery : IRequest<IEnumerable<SelecionarContaFiltersQueryResponse>>
    {
    }

    public class SelecionarContaFiltersQueryResponse : Entity
    {
        public long Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }

    public class SelecionarContaFiltersQueryResponseHandler : IRequestHandler<SelecionarContaFiltersQuery, IEnumerable<SelecionarContaFiltersQueryResponse>>
    {
        private readonly IRepository<Conta> _repository;

        public SelecionarContaFiltersQueryResponseHandler
        (
            IRepository<Conta> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarContaFiltersQueryResponse>> Handle
        (
            SelecionarContaFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarContaFiltersQuery>());

            IEnumerable<Conta> contaMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarContaFiltersQueryResponse> responseMany = new List<SelecionarContaFiltersQueryResponse>();

            foreach (Conta conta in contaMany)
            {
                SelecionarContaFiltersQueryResponse response = new SelecionarContaFiltersQueryResponse();
                response.Cpf = conta.Cpf;
                response.Nome = conta.Nome;
                response.Email = conta.Email;
                response.Telefone = conta.Telefone;
                response.DataCadastro = conta.DataCadastro;
                response.DataAtualizacao = conta.DataAtualizacao;
                response.Id = conta.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
