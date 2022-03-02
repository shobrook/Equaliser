namespace Equaliser.Exceptions;

public class EqualityException<T> : Exception
{
    private const string _equalityMessage = "Equality test failed (A == A returned false). ";
    private const string _hashCodeMessage = "HashCode test failed (A.GetHashCode() == A.GetHashCode() returned false). ";

    public EqualityException(bool isEqualityInvalid, bool isHashCodeInvalid) : base(
        ConstructMessage(isEqualityInvalid, isHashCodeInvalid)) { }

    private static string ConstructMessage(bool isEqualityInvalid, bool isHashCodeInvalid)
    {
        var message = "";
        message += isEqualityInvalid ? _equalityMessage : "";
        message += isHashCodeInvalid ? _hashCodeMessage : "";
        message += $"Object: {typeof(T).FullName}.";

        return message;
    }
}