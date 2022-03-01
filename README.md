# Equaliser

Equaliser automates unit testing for `IEquatable` objects –– i.e. any object that implements the `Equals` method. Instead of manually writing equality tests, Equaliser will do it for you in two lines of code:

```csharp
var equalityTests = EqualityTests<ObjectUnderTest>();
equalityTests.AssertAll();
```

<!--Equaliser also tests the GetHashCode method.-->

## Installation

TODO: Publish on nuget.org

## Usage

We'll introduce the features of Equaliser by using the following class as an example:

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

This class has _incorrectly_ implemented its equality method by returning `Prop2 != other.Prop2` instead of `Prop2 == other.Prop2`. We'll show how Equaliser automatically catches this mistake.

> Notice the attributes on `Prop3` and `Prop4`. Equaliser allows you to specify which properties are *ignored* in the equality method or compared by *reference* (instead of by *value*), using the `Ignore` and `CompareByReference` attributes, respectively. This information gets factored into the tests.

To begin testing with Equaliser, we need to instantiate an `EqualityTests` object:

```csharp
using Equaliser.Tests;

var equalityTests = new EqualityTests<ObjectUnderTest>();
```

This object exposes all the methods we need to test the _equality_ case and the _inequality_ cases, which we'll cover in the sections below.

### Testing Equality

Testing equality involves instantiating two identical versions of the object under test (OUT) and asserting that they're equal. Doing this manually requires creating mock objects and writing assert statements, but with Equaliser it only requires a single method call.

**Without Equaliser**

```csharp
var A = new ObjectUnderTest { Prop1 = 0, Prop2 = "abc", Prop3 = childA };
var B = new ObjectUnderTest { Prop1 = 0, Prop2 = "abc", Prop3 = childA };

Assert.IsTrue(A.Equals(B));
Assert.IsTrue(B.Equals(A)); // Ensure bi-directional equality
```

**With Equaliser**

```csharp
equalityTests.AssertEquality();
```

In our example, `AssertEquality` will throw an `Equaliser.EqualityException` because the equality method is incorrectly implemented:

```csharp
EqualityException("Equality test failed (A == A returned false).")
```

If it were implemented correctly, the method would return nothing. When an `EqualityException` is thrown, Equaliser can help you narrow down which property is causing the issue by running _inequality_ tests, covered in the next section.

### Testing Inequality

Testing inequality involves instantiating two identical versions of the OUT, changing a single property value in one object, and then asserting the two objects aren't equal. Equaliser will repeat this process for every property to ensure robustness.

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

// Ensure bi-directional inequality
Assert.IsFalse(B.Equals(A));
Assert.IsFalse(C.Equals(A));
Assert.IsFalse(D.Equals(A));
```

**With Equaliser**

```csharp
equalityTests.AssertInequality();
```

In our example, `AssertInequality` will throw an `Equaliser.InequalityException` for every altered property that doesn't produce inequality (in our case, only `Prop2`):

```csharp
AggregateException(
  InequalityException("Inequality test failed (A == B returned true). Property: Prop2.")
)
```

### Testing Both

To run both `AssertEquality` and `AssertInequality`, and aggregate the exceptions, simply run:

```csharp
equalityTests.AssertAll();
```

### Namespace-Level Testing

The `EqualityTests` object is built for testing a single object. Equaliser provides the `NamespaceEqualityTests` object for testing all the objects that inherit from `IEquatable` within a given namespace.

```csharp
using Equaliser.Tests;

var namespaceEqualityTests = new NamespaceEqualityTests("my.namespace");
namespaceEqualityTests.AssertAll();
```

```csharp
AggregateException(
  EqualityException("Equality test failed (A == A returned false). Object: ObjectUnderTest."),
  InequalityException("Inequality test failed (A == B returned true). Object: ObjectUnderTest. Property: Prop2."),
  ...
)
```
