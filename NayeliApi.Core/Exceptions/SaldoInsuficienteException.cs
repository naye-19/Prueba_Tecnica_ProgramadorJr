namespace NayeliApi.Core.Exceptions;

public class SaldoInsuficienteException : Exception
{
    public SaldoInsuficienteException()
        : base("Saldo insuficiente para realizar la transacción.")
    {
    }

    public SaldoInsuficienteException(string message)
        : base(message)
    {
    }

    public SaldoInsuficienteException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
