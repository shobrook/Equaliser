# Equaliser

Equaliser automates unit testing for `IEquatable` objects –– i.e. any object that implements an `Equals` method. Instead of manually writing equality tests, Equaliser does it for you in two lines of code:

```csharp
var equalityTests = EqualityTests<ObjectUnderTest>();
equalityTests.AssertEqualityAndInequality();
```

## Installation

TODO: Publish to nuget.org

## Usage

We'll introduce the features of Equaliser by testing the following class:

```csharp
using Equaliser.Attributes;

public class ObjectUnderTest : IEquatable<ObjectUnderTest>
{
  public int Prop1 { get; set; }
  public string Prop2 { get; set; }

  [CompareByReference]
  public MyChildObject Prop3 { get; set; }

  [Ignore]
  public DateTime Prop4 { get; set; }

  public bool Equals(MyObject other)
  {
    return Prop1 == other.Prop1
           && Prop2 != other.Prop2
           && ReferenceEquals(Prop3, other.Prop3);
  }
}
```

This class has _incorrectly_ implemented its equality method by returning `Prop2 != other.Prop2` instead of `==`.

>Notice the attributes on `Prop3` and `Prop4`. Equaliser allows you to specify which properties are *ignored* in the equality method or compared by *reference* (instead of by *value*), using the `Ignore` and `CompareByReference` attributes, respectively.

### Testing Equality

```csharp
var equalityTests = EqualityTests<ObjectUnderTest>();
```

**Without Equaliser**

```csharp
var A = new ObjectUnderTest { Prop1 = 0, Prop2 = "abc", Prop3 = childA };
var B = new ObjectUnderTest { Prop1 = 0, Prop2 = "abc", Prop3 = childA };

Assert.IsTrue(A.Equals(B));
Assert.IsTrue(B.Equals(A));
```

**With Equaliser**

```csharp
equalityTests.AssertEquality();
```

### Testing Inequality

**Without Equaliser**

```csharp
var childA = new MyChildObject(0);
var childB = new MyChildObject(1);

var A = new ObjectUnderTest { Prop1 = 0, Prop2 = "abc", Prop3 = childA };
var B = new ObjectUnderTest { Prop1 = 1, Prop2 = "abc", Prop3 = childA };
var C = new ObjectUnderTest { Prop1 = 0, Prop2 = "def", Prop3 = childA };
var D = new ObjectUnderTest { Prop1 = 0, Prop2 = "abc", Prop3 = childB };

Assert.IsFalse(A.Equals(B)); // Prop1 is different
Assert.IsFalse(A.Equals(C)); // Prop2 is different
Assert.IsFalse(A.Equals(D)); // Prop3 is different

// Ensure bi-directional equality
Assert.IsFalse(B.Equals(A));
Assert.IsFalse(C.Equals(A));
Assert.IsFalse(D.Equals(A));
```

**With Equaliser**

```csharp
outEqualityTests.AssertInequalityByProperty();
```

Note that Equaliser may not be useful for objects with custom logic in their Equals methods, besides comparisons between properties.
