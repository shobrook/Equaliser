using Equaliser.Attributes;

namespace Equaliser.UnitTests.TestInputs;

public class ObjectUnderTestWithIgnoredPropertyWithAttribute : IEquatable<ObjectUnderTestWithIgnoredPropertyWithAttribute>
{
    public int Property1 { get; set; }

    [Ignore]
    public string Property2 { get; set; }

    public ChildObject Property3 { get; set; }
    
    public bool Equals(ObjectUnderTestWithIgnoredPropertyWithAttribute other)
    {
        return Property1.Equals(other.Property1) && Property3.Equals(other.Property3);
    }
}