namespace Equaliser.Exceptions;

public class EqualityException<T> : Exception
{
    public EqualityException() : base(ConstructMessage()) { }

    private static string ConstructMessage()
    {
        var baseMessage = "Equality test failed (A == A returned false)";
        return $"{baseMessage}. Object: {typeof(T).FullName}.";
    }
}