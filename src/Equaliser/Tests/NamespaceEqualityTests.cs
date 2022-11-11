namespace Equaliser.Tests;

public class NamespaceEqualityTests : INamespaceEqualityTests
{
    public string Namespace { get; set; }

    public NamespaceEqualityTests(string nspace)
    {
        Namespace = nspace;
    }

    public void AssertAll()
    {
        AssertEqualityTestForAllObjects(tests => tests.AssertAll());
    }
    
    public void AssertEquality()
    {
        AssertEqualityTestForAllObjects(tests => tests.AssertEquality());
    }
    
    public void AssertInequality()
    {
        AssertEqualityTestForAllObjects(tests => tests.AssertInequality());
    }

    private void AssertEqualityTestForAllObjects(Action<IEqualityTests> invokedTestMethod)
    {
        var exceptions = new List<Exception>();
        var equalityTypes = GetTypesImplementingInterface(typeof(IEquatable<>));
        foreach (var type in equalityTypes)
        {
            var objectEqualityTestsType = typeof(EqualityTests<>).MakeGenericType(type);
            var objectEqualityTests = (IEqualityTests) Activator.CreateInstance(objectEqualityTestsType);

            try
            {
                invokedTestMethod(objectEqualityTests);
            }
            catch (AggregateException ae)
            {
                exceptions.AddRange(ae.InnerExceptions);
            }
            catch (Exception e)
            {
                exceptions.Add(e);
            }
        }

        if (exceptions.Any())
            throw new AggregateException(exceptions);
    }

    private IEnumerable<Type> GetTypesImplementingInterface(Type inter)
    {
        return AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => DoesTypeInheritFromInterface(t, inter) && t.Namespace == Namespace);
    }

    private static bool DoesTypeInheritFromInterface(Type type, Type inter)
    {
        if (inter.IsAssignableFrom(type))
            return true;
        
        return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == inter);
        // type.IsClass && !type.IsAbstract
    }
}