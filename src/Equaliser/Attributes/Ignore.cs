namespace Equaliser.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class Ignore : Attribute
{
    public Ignore() { }
}