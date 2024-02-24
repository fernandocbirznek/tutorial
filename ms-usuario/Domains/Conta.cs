namespace ms_usuario.Domains
{
    public class Conta : Entity
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}
