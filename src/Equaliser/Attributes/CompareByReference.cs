namespace Equaliser.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CompareByReference : Attribute
{
    public CompareByReference() { }
}