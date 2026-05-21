namespace ECommerce.Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(int requested, int available)
        : base($"No hay stock suficiente. Solicitado: {requested}. Disponible: {available}.") { }
}
