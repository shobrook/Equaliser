namespace Equaliser.UnitTests.TestInputs;

public class ChildObject
{
    public int Property1 { get; set; }

    public bool Equals(ChildObject other)
    {
        return Property1.Equals(other.Property1);
    }
}