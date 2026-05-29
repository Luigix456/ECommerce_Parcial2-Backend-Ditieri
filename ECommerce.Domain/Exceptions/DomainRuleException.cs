namespace ECommerce.Domain.Exceptions;

public class DomainRuleException : DomainException
{
    public DomainRuleException(string message)
        : base(message) { }
}
