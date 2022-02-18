namespace equaliser.Exceptions
{
    public class InequalityException<T> : Exception
    {
        public InequalityException(string? propertyName) : base(ConstructMessage(propertyName)) { }
        public InequalityException(string? propertyName, Exception inner) : base(ConstructMessage(propertyName), inner) { }

        private static string ConstructMessage(string? propertyName)
        {
            var baseMessage = $"Inequality test failed (A == B returned true). Object: {typeof(T).FullName}.";
            if (propertyName is not null)
            {
                return $"{baseMessage} Property: {propertyName}.";
            }

            return baseMessage;
        }
    }
}