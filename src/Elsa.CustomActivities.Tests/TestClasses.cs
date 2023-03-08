using Elsa.CustomActivities.Activities;
using Elsa.CustomActivities.PropertyDecorator;

namespace Elsa.CustomActivities.Tests
{
    public class TestClassWithOneStringProperty
    {
        public string StringType { get; set; }
    }

    public class TestClassWithOneBooleanProperty
    {
        public bool BooleanType { get; set; }
    }

    public class TestClassWithOneNullableBooleanProperty
    {
        public bool? NullableBooleanType { get; set; }
    }

    public class TestClassWithOneIntProperty
    {
        public int IntType { get; set; }
    }

    public class TestClassWithOneOtherProperty
    {
        public object ObjectType { get; set; }
    }

    public class TestClassWithOnePropertyAndExpectedOutputType
    {
        [HeActivityInput(ExpectedOutputType = "test")]
        public object ObjectType { get; set; }
    }

    public class TestClassWithOneCustomElsaAttributeProperty
    {
        [CustomElsaAttributeObject]
        public MyNestedClass CustomElsaAttributeProperty { get; set; }

        public class MyNestedClass
        {
            [HeActivityInput]
            public string HeActivityInput { get; set; }
        }
    }

    public class TestClassWithOneCustomElsaAttributePropertyAndCategory
    {
        [CustomElsaAttributeObject]
        public MyNestedClass CustomElsaAttributeProperty { get; set; }

        public class MyNestedClass
        {
            [HeActivityInput(Category = "TestCategory")]
            public string HeActivityInput { get; set; }
        }
    }

    public class TestClassWithOneHeActivityInputAttributeProperty
    {
        //[HeActivityInput(OptionsProvider = typeof(IOptionsProvider))]
        [HeActivityInput]
        public string HeActivityInput { get; set; }
    }

    public class TestClassWithMultipleHeActivityInputAttributeProperties
    {
        [HeActivityInput(Order = 2)]
        public string HeActivityInput { get; set; }
        [HeActivityInput(Order = 5)]
        public string Last { get; set; }
        [HeActivityInput(Order = 1)]
        public string First { get; set; }
    }
}
