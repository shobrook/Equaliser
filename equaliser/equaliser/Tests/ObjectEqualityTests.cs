using System.Reflection;
using AutoFixture;
using Force.DeepCloner;
using equaliser.Exceptions;

namespace equaliser.Tests
{
    public class ObjectEqualityTests<TObj> : IObjectEqualityTests<TObj>
    {
        private Fixture _fixture = new Fixture();
 
        public void AssertAll()
        {
            var exceptions = new List<Exception>();
            try
            {
                AssertEquality();
                AssertInequalityForAllProperties();
            }
            catch (EqualityException<TObj> ee)
            {
                exceptions.Add(ee);
            }
            catch (AggregateException ae)
            {
                exceptions.AddRange(ae.InnerExceptions);
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }

        public void AssertEquality()
        {
            var mockObject = _fixture.Create<TObj>();
            var clonedMockObject = mockObject.DeepClone();

            if (!mockObject.Equals(clonedMockObject))
            {
                throw new EqualityException<TObj>();
            }
            
            // Check if T inherits from IAutoEquatable
            // If it does, then retrieve the list of ignored properties (i.e. properties with the [EquatableIgnore] attribute)
            // Exclude these properties from the fixture by chaining .Without(p => p.PropertyName);
        }

        public void AssertInequalityForAllProperties()
        {
            var mockObject = _fixture.Create<TObj>();
            var clonedMockObject = _fixture.Create<TObj>();
            // QUESTION: How to ensure these objects won't have identical values?

            if (mockObject.Equals(clonedMockObject))
            {
                throw new InequalityException<TObj>(null);
            }
        }

        public void AssertInequalityByProperty()
        {
            var exceptions = new List<InequalityException<TObj>>();
            var mockObject = _fixture.Create<TObj>();
            foreach (var property in mockObject.GetType().GetProperties())
            {
                var changedMockObject = CloneAndChangePropertyValue(property, mockObject);
                if (mockObject.Equals(changedMockObject))
                {
                    exceptions.Add(new InequalityException<TObj>(property.Name));
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
        
        private TObj CloneAndChangePropertyValue(PropertyInfo property, TObj mockObject)
        {
            var initialPropertyValue = property.GetValue(mockObject, null);
            var newPropertyValue = GenerateNewPropertyValue(initialPropertyValue);
            
            var changedMockObject = mockObject.DeepClone();
            property.SetValue(changedMockObject, newPropertyValue);

            return changedMockObject;
        }

        private TProp GenerateNewPropertyValue<TProp>(TProp initialPropertyValue)
        {
            // QUESTION: How can you ensure this isn't identical to the original property value?
            return _fixture.Create<TProp>();
        }
    }
}