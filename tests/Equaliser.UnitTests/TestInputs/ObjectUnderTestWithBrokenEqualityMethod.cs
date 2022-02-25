using System;

namespace Equaliser.UnitTests.TestInputs;

public class ObjectUnderTestWithBrokenEqualityMethod : IEquatable<ObjectUnderTestWithBrokenEqualityMethod>
{
    public int Property1 { get; set; }
    public string Property2 { get; set; }
    public ChildObject Property3 { get; set; }
    
    public bool Equals(ObjectUnderTestWithBrokenEqualityMethod other)
    {
        return Property1.Equals(other.Property1)
               && Property2.Equals(other.Property2)
               && Property3.Equals(other.Property3);
    }
}