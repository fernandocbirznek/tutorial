using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_usuario.Extensions;
using ms_usuario.Features.ContaFeature.Commands;
using ms_usuario.Features.ContaFeature.Queries;

namespace ms_usuario.Features.ContaFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirContaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarContaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{contaId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long contaId)
        {
            return await this.SendAsync(_mediator, new RemoverContaCommand() { Id = contaId });
        }

        [HttpGet("selecionar-conta/{contaId}")]
        public async Task<ActionResult> GetForum(long contaId)
        {
            return await this.SendAsync(_mediator, new SelecionarContaByIdQuery() { Id = contaId });
        }

        [HttpGet("selecionar-conta-many")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarContaFiltersQuery());
        }
    }
}
