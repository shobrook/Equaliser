namespace Equaliser.UnitTests.TestInputs;

public class ObjectUnderTest : IEquatable<ObjectUnderTest>
{
    public int Property1 { get; set; }

    public string Property2 { get; set; }

    public ChildObject Property3 { get; set; }
    
    public bool Equals(ObjectUnderTest? other)
    {
        return Property1.Equals(other.Property1)
               && Property2.Equals(other.Property2, StringComparison.Ordinal)
               && Property3.Equals(other.Property3);
    }
}