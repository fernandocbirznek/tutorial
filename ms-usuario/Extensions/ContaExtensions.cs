using ms_usuario.Domains;
using ms_usuario.Features.ContaFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class ContaExtensions
    {
        public static Conta ToDomain(this InserirContaCommand request)
        {
            return new()
            {
                Cpf = request.Cpf,
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone,
                DataCadastro = DateTime.Now
            };
        }
    }
}
