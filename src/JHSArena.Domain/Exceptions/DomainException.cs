namespace JHSArena.Domain.Exceptions;

/// <summary>
/// Representa uma violação de regra de negócio do domínio.
/// </summary>
public sealed class DomainException : Exception
{
    public DomainException(string message)
        : base(message)
    {
    }
}
