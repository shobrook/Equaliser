namespace Equaliser.UnitTests.TestInputs;

public class ObjectUnderTestWithReferencePropertyWithoutAttribute : IEquatable<ObjectUnderTestWithReferencePropertyWithoutAttribute>
{
    public int Property1 { get; set; }

    public string Property2 { get; set; }

    public ChildObject Property3 { get; set; }
    
    public bool Equals(ObjectUnderTestWithReferencePropertyWithoutAttribute other)
    {
        return Property1.Equals(other.Property1)
               && Property2.Equals(other.Property2)
               && ReferenceEquals(Property3, other.Property3);
    }
}