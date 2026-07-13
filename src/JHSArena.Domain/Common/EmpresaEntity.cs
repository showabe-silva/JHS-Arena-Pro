using JHSArena.Domain.Exceptions;

namespace JHSArena.Domain.Common;

/// <summary>
/// Classe base para entidades que pertencem a uma empresa.
/// </summary>
public abstract class EmpresaEntity : BaseEntity
{
    protected EmpresaEntity()
    {
    }

    protected EmpresaEntity(Guid empresaId)
    {
        if (empresaId == Guid.Empty)
        {
            throw new DomainException("A empresa é obrigatória.");
        }

        EmpresaId = empresaId;
    }

    public Guid EmpresaId { get; protected set; }
}
