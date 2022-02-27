using System.Reflection;
using AutoFixture;
using Force.DeepCloner;
using Equaliser.Attributes;
using Equaliser.Exceptions;

namespace Equaliser.Tests;

public class EqualityTests<TObj> : IEqualityTests
{
    private Fixture _fixture;

    public EqualityTests()
    {
        _fixture = new Fixture();
    }

    public void AssertAll()
    {
        var exceptions = new List<Exception>();
        try
        {
            AssertEquality();
            AssertInequalityByProperty();
        }
        catch (EqualityException<TObj> ee)
        {
            exceptions.Add(ee);
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
        
        if (!mockObject.Equals(clonedMockObject))
            throw new EqualityException<TObj>();
    }

    public void AssertInequalityByProperty()
    {
        var exceptions = new List<InequalityException<TObj>>();
        var mockObject = _fixture.Create<TObj>();
        var objectProperties = GetObjectProperties(mockObject);
        foreach (var property in objectProperties)
        {
            var changedMockObject = CloneThenChangePropertyValue(property, mockObject);
            if (mockObject.Equals(changedMockObject))
                exceptions.Add(new InequalityException<TObj>(property.Name));
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

    private IEnumerable<PropertyInfo> GetObjectProperties(TObj objectInstance)
    {
        return objectInstance.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(Ignore)));
    }

    private TObj CloneThenChangePropertyValue(PropertyInfo property, TObj mockObject)
    {
        var initialPropertyValue = property.GetValue(mockObject, null);
        var newPropertyValue = GenerateNewPropertyValue(initialPropertyValue);

        var changedMockObject = CloneMockObject(mockObject);
        property.SetValue(changedMockObject, newPropertyValue);

        return changedMockObject;
    }

    private TProp GenerateNewPropertyValue<TProp>(TProp initialPropertyValue)
    {
        return _fixture.Create<TProp>();
    }
}