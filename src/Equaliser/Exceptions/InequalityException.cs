namespace Equaliser.Exceptions;

public class InequalityException<T> : Exception
{
    private const string _inEqualityMessage = "Inequality test failed (A == B returned true). ";
    private const string _hashCodeMessage = "HashCode test failed (A.GetHashCode() == A.GetHashCode() returned true). ";

    public InequalityException(
        bool isEqualityInvalid, bool isHashCodeInvalid, string propertyName) : base(
        ConstructMessage(isEqualityInvalid, isHashCodeInvalid, propertyName)) { }
    
    private static string ConstructMessage(bool isEqualityInvalid, bool isHashCodeInvalid, string? propertyName)
    {
        var message = "";
        message += isEqualityInvalid ? _inEqualityMessage : "";
        message += isHashCodeInvalid ? _hashCodeMessage : "";
        message += $"Object: {typeof(T).FullName}. Property: {propertyName}.";

        return message;
    }
}