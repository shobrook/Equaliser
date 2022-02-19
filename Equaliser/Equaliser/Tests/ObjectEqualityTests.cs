using System.Reflection;
using AutoFixture;
using Force.DeepCloner;
using Equaliser.Exceptions;

namespace Equaliser.Tests
{
    public class ObjectEqualityTests<TObj> : IObjectEqualityTests
    {
        private Fixture _fixture = new Fixture();
 
        public void AssertAll()
        {
            var exceptions = new List<Exception>();
            try
            {
                AssertEquality();
                AssertInequalityByProperty();
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
                throw new AggregateException(exceptions);
        }

        public void AssertEquality()
        {
            var mockObject = _fixture.Create<TObj>();
            var clonedMockObject = mockObject.DeepClone();

            if (!mockObject.Equals(clonedMockObject))
                throw new EqualityException<TObj>();
        }

        public void AssertInequalityForAllProperties()
        {
            var mockObject = _fixture.Create<TObj>();
            var otherMockObject = _fixture.Create<TObj>();

            if (mockObject.Equals(otherMockObject))
                throw new InequalityException<TObj>(null);
        }

        public void AssertInequalityByProperty()
        {
            var exceptions = new List<InequalityException<TObj>>();
            var mockObject = _fixture.Create<TObj>();
            foreach (var property in mockObject.GetType().GetProperties())
            {
                var changedMockObject = CloneAndChangePropertyValue(property, mockObject);
                if (mockObject.Equals(changedMockObject))
                    exceptions.Add(new InequalityException<TObj>(property.Name));
            }

            if (exceptions.Any())
                throw new AggregateException(exceptions);
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
            return _fixture.Create<TProp>();
        }
    }
}