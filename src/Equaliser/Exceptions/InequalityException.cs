namespace Equaliser.Exceptions;

public class InequalityException<T> : Exception
{
    public InequalityException(string propertyName) : base(ConstructMessage(propertyName)) { }
    
    private static string ConstructMessage(string? propertyName)
    {
        var baseMessage = "Inequality test failed (A == B returned true)";
        return $"{baseMessage}. Object: {typeof(T).FullName}. Property: {propertyName}.";
    }
}