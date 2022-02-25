using System;
using Equaliser.Attributes;

namespace Equaliser.UnitTests.TestInputs;

public class ObjectUnderTestWithIgnoredProperties : IEquatable<ObjectUnderTestWithIgnoredProperties>
{
    public int Property1 { get; set; }
    [Ignore]
    public string Property2 { get; set; }
    [Ignore]
    public ChildObject Property3 { get; set; }
    
    public bool Equals(ObjectUnderTestWithIgnoredProperties other)
    {
        return Property1.Equals(other.Property1);
    }
}