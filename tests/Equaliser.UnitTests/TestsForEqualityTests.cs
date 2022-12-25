using Equaliser.Tests;
using Equaliser.UnitTests.TestInputs;
using NUnit.Framework;

namespace Equaliser.UnitTests;

public class TestsForEqualityTests
{
    [Test]
    public void SimpleComparisonsShouldPass()
    {
        var equalityTests = new EqualityTests<ObjectUnderTest>();
        equalityTests.AssertAll();
    }
    
    [Test]
    public void IgnoredPropertyShouldPassWithAttribute()
    {
        var equalityTests = new EqualityTests<ObjectUnderTestWithIgnoredPropertyWithAttribute>();
        equalityTests.AssertAll();
    }
    
    [Test]
    public void IgnoredPropertyShouldFailWithoutAttribute()
    {
        var equalityTests = new EqualityTests<ObjectUnderTestWithIgnoredPropertyWithoutAttribute>();
        Assert.Throws<AggregateException>(() => equalityTests.AssertAll());
    }
    
    [Test]
    public void ReferenceEqualsShouldPassWithAttribute()
    {
        var equalityTests = new EqualityTests<ObjectUnderTestWithReferencePropertyWithAttribute>();
        equalityTests.AssertAll();
    }
    
    [Test]
    public void ReferenceEqualsShouldFailWithoutAttribute()
    {
        var equalityTests = new EqualityTests<ObjectUnderTestWithReferencePropertyWithoutAttribute>();
        Assert.Throws<AggregateException>(() => equalityTests.AssertAll());
    }
}