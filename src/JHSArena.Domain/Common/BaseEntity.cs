namespace JHSArena.Domain.Common;

/// <summary>
/// Classe base para entidades persistidas pelo sistema.
/// </summary>
public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTimeOffset.UtcNow;
        Ativo = true;
    }

    public Guid Id { get; protected set; }

    public DateTimeOffset CriadoEm { get; protected set; }

    public DateTimeOffset? AtualizadoEm { get; protected set; }

    public Guid? CriadoPor { get; protected set; }

    public Guid? AtualizadoPor { get; protected set; }

    public bool Ativo { get; protected set; }

    public void DefinirCriador(Guid usuarioId)
    {
        CriadoPor = usuarioId;
    }

    public void RegistrarAtualizacao(Guid usuarioId)
    {
        AtualizadoPor = usuarioId;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void Ativar()
    {
        Ativo = true;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }

    public void Inativar()
    {
        Ativo = false;
        AtualizadoEm = DateTimeOffset.UtcNow;
    }
}
