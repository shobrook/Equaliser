namespace Equaliser.UnitTests.TestInputs;

public class ObjectUnderTestWithIgnoredPropertyWithoutAttribute : IEquatable<ObjectUnderTestWithIgnoredPropertyWithoutAttribute>
{
    public int Property1 { get; set; }

    public string Property2 { get; set; }

    public ChildObject Property3 { get; set; }
    
    public bool Equals(ObjectUnderTestWithIgnoredPropertyWithoutAttribute other)
    {
        return Property1.Equals(other.Property1) && Property3.Equals(other.Property3);
    }
}