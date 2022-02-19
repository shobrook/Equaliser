namespace Equaliser.Tests
{
    public static class AggregateObjectEqualityTests
    {
        public static void AssertForAllIEquatables()
        {
            var equalityTypes = GetTypesImplementingInterface(typeof(IEquatable<>));
            foreach (var type in equalityTypes)
            {
                var objectEqualityTestsType = typeof(ObjectEqualityTests<>).MakeGenericType(type);
                var objectEqualityTests = (IObjectEqualityTests) Activator.CreateInstance(objectEqualityTestsType);
                
                objectEqualityTests.AssertAll();
            }
        }

        private static IEnumerable<Type> GetTypesImplementingInterface(Type inter)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => DoesTypeInheritFromInterface(t, inter));
        }

        private static bool DoesTypeInheritFromInterface(Type type, Type inter)
        {
            if (inter.IsAssignableFrom(type))
                return true;
            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == inter))
                return true;

            // type.IsClass && !type.IsAbstract
            
            return false;
        }
    }
}