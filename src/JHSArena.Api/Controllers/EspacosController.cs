using JHSArena.Application.Features.Espacos.Commands.AlterarStatusEspaco;
using JHSArena.Application.Features.Espacos.Commands.CriarEspaco;
using JHSArena.Application.Features.Espacos.DTOs;
using JHSArena.Application.Features.Espacos.Queries;
using JHSArena.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JHSArena.Api.Controllers;

[ApiController]
[Route("api/espacos")]
public sealed class EspacosController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EspacoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EspacoDto>>> Listar(
        [FromQuery] Guid empresaId,
        [FromQuery] Guid? unidadeId,
        [FromServices] ListarEspacosHandler handler,
        CancellationToken cancellationToken)
    {
        var espacos = await handler.HandleAsync(
            empresaId,
            unidadeId,
            cancellationToken);

        return Ok(espacos);
    }

    [HttpGet("{espacoId:guid}")]
    [ProducesResponseType(typeof(EspacoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EspacoDto>> ObterPorId(
        Guid espacoId,
        [FromQuery] Guid empresaId,
        [FromServices] ObterEspacoPorIdHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var espaco = await handler.HandleAsync(
                empresaId,
                espacoId,
                cancellationToken);

            return Ok(espaco);
        }
        catch (DomainException exception)
        {
            return NotFound(new { erro = exception.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(EspacoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EspacoDto>> Criar(
        [FromBody] CriarEspacoCommand command,
        [FromServices] CriarEspacoHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var espaco = await handler.HandleAsync(
                command,
                cancellationToken);

            return CreatedAtAction(
                nameof(ObterPorId),
                new
                {
                    espacoId = espaco.Id,
                    empresaId = espaco.EmpresaId
                },
                espaco);
        }
        catch (DomainException exception)
        {
            return BadRequest(new { erro = exception.Message });
        }
    }

    [HttpPatch("{espacoId:guid}/manutencao")]
    [ProducesResponseType(typeof(EspacoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EspacoDto>> ColocarEmManutencao(
        Guid espacoId,
        [FromQuery] Guid empresaId,
        [FromServices] AlterarStatusEspacoHandler handler,
        CancellationToken cancellationToken)
    {
        return await ExecutarAlteracaoStatus(
            () => handler.ColocarEmManutencaoAsync(
                empresaId,
                espacoId,
                cancellationToken));
    }

    [HttpPatch("{espacoId:guid}/liberar")]
    [ProducesResponseType(typeof(EspacoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EspacoDto>> LiberarParaUso(
        Guid espacoId,
        [FromQuery] Guid empresaId,
        [FromServices] AlterarStatusEspacoHandler handler,
        CancellationToken cancellationToken)
    {
        return await ExecutarAlteracaoStatus(
            () => handler.LiberarParaUsoAsync(
                empresaId,
                espacoId,
                cancellationToken));
    }

    [HttpPatch("{espacoId:guid}/desativar")]
    [ProducesResponseType(typeof(EspacoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EspacoDto>> Desativar(
        Guid espacoId,
        [FromQuery] Guid empresaId,
        [FromServices] AlterarStatusEspacoHandler handler,
        CancellationToken cancellationToken)
    {
        return await ExecutarAlteracaoStatus(
            () => handler.DesativarAsync(
                empresaId,
                espacoId,
                cancellationToken));
    }

    private async Task<ActionResult<EspacoDto>> ExecutarAlteracaoStatus(
        Func<Task<EspacoDto>> operacao)
    {
        try
        {
            return Ok(await operacao());
        }
        catch (DomainException exception)
        {
            return NotFound(new { erro = exception.Message });
        }
    }
}
