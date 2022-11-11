using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Force.DeepCloner;
using Equaliser.Attributes;
using Equaliser.Exceptions;

namespace Equaliser.Tests;

public class EqualityTests<TObj> : IEqualityTests where TObj : IEquatable<TObj>
{
    private Fixture _fixture;
    private bool _isEqualsImplemented;
    private bool _isGetHashCodeImplemented;

    public EqualityTests()
    {
        _fixture = new Fixture();
        _isEqualsImplemented = HasMethod("Equals");
        _isGetHashCodeImplemented = HasMethod("GetHashCode");
    }

    public void AssertAll()
    {
        var exceptions = new List<Exception>();
        try
        {
            AssertEquality(); 
        }
        catch (EqualityException<TObj> ee)
        {
            exceptions.Add(ee);
        }
        
        try
        {
            AssertInequality();
        }
        catch (AggregateException ae)
        {
            exceptions.AddRange(ae.InnerExceptions);
        }

        if (exceptions.Any())
            throw new AggregateException(exceptions);
    }

    public void AssertEquality()
    {
        var mockObject = _fixture.Create<TObj>();
        var clonedMockObject = CloneMockObject(mockObject);

        var isEqualityInvalid = _isEqualsImplemented
            ? !mockObject.Equals(clonedMockObject) || !clonedMockObject.Equals(mockObject)
            : false;
        var isHashCodeInvalid = _isGetHashCodeImplemented
            ? mockObject.GetHashCode() != clonedMockObject.GetHashCode()
            : false;
        
        if (isEqualityInvalid || isHashCodeInvalid)
            throw new EqualityException<TObj>(isEqualityInvalid, isHashCodeInvalid);
    }

    public void AssertInequality()
    {
        var exceptions = new List<InequalityException<TObj>>();
        var mockObject = _fixture.Create<TObj>();
        var objectProperties = GetObjectProperties(mockObject);
        foreach (var property in objectProperties)
        {
            var changedMockObject = CloneThenChangePropertyValue(property, mockObject);

            var isEqualityInvalid = _isEqualsImplemented
                ? mockObject.Equals(changedMockObject) || changedMockObject.Equals(mockObject)
                : false;
            var isHashCodeInvalid = _isGetHashCodeImplemented
                ? mockObject.GetHashCode() == changedMockObject.GetHashCode()
                : false;
            
            if (isEqualityInvalid || isHashCodeInvalid)
                exceptions.Add(new InequalityException<TObj>(isEqualityInvalid, isHashCodeInvalid, property.Name));
        }

        if (exceptions.Any())
            throw new AggregateException(exceptions);
    }

    private TObj CloneMockObject(TObj mockObject)
    {
        var clonedMockObject = mockObject.ShallowClone(); // DeepClone()?
        var deepProperties = GetObjectProperties(mockObject)
            .Where(p => !Attribute.IsDefined(p, typeof(CompareByReference)));

        foreach (var property in deepProperties)
        {
            var propertyValue = property.GetValue(clonedMockObject, null);
            property.SetValue(clonedMockObject, propertyValue.DeepClone());
        }

        return clonedMockObject;
    }

    private IEnumerable<PropertyInfo> GetObjectProperties(TObj objectInstance) =>
        objectInstance.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(Ignore)));

    private TObj CloneThenChangePropertyValue(PropertyInfo property, TObj mockObject)
    {
        var initialPropertyValue = property.GetValue(mockObject, null);
        // var newPropertyValue = GenerateNewPropertyValue(initialPropertyValue);
        var newPropertyValue = GenerateNewPropertyValue(property.PropertyType);

        var changedMockObject = CloneMockObject(mockObject);
        property.SetValue(changedMockObject, newPropertyValue);

        return changedMockObject;
    }

    // private TProp GenerateNewPropertyValue<TProp>(TProp initialPropertyValue) => _fixture.Create<TProp>();

    private object GenerateNewPropertyValue(Type propertyType)
    {
        var context = new SpecimenContext(_fixture);
        return context.Resolve(propertyType);
    }
    
    private static bool HasMethod(string methodName)
    {
        try
        {
            var method = typeof(TObj).GetMethod(methodName);
            return method != null && method.DeclaringType == typeof(TObj);
        }
        catch (AmbiguousMatchException)
        {
            return true;
        }
    } 
}