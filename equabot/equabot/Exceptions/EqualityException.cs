namespace equabot.Exceptions
{
    public class EqualityException<T> : Exception
    {
        public EqualityException() : base(ConstructMessage()) { }
        public EqualityException(Exception inner) : base(ConstructMessage(), inner) { }

        private static string ConstructMessage()
        {
            return $"Equality test failed (A == A returned false). Object: {typeof(T).FullName}.";
        }
    }
}