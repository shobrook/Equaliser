namespace Equaliser.Tests;

public interface IObjectEqualityTests<T>
{
    void AssertEquality() {}
    void AssertInequalityForAllProperties() {}
    void AssertInequalityByProperty() {}
    void AssertAll() {}
}