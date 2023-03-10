using Elsa.CustomActivities.Activities;
using Elsa.CustomActivities.PropertyDecorator;

namespace Elsa.CustomActivities.Tests
{
    public class TestClassWithOneStringProperty
    {
        public string StringType { get; set; } = null!;
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
    public class TestClassWithOneListProperty
    {
        public List<object> ListType { get; set; } = null!;
    }

    public class TestClassWithOneOtherProperty
    {
        public object ObjectType { get; set; } = null!;
    }

    public class TestClassWithOnePropertyAndExpectedOutputType
    {
        [HeActivityInput(ExpectedOutputType = "test")]
        public object ObjectType { get; set; } = null!;
    }

    public class TestClassWithOneCustomElsaAttributeProperty
    {
        [CustomElsaAttributeObject]
        public MyNestedClass CustomElsaAttributeProperty { get; set; } = null!;

        public class MyNestedClass
        {
            [HeActivityInput]
            public string HeActivityInput { get; set; } = null!;
        }
    }

    public class TestClassWithOneCustomElsaAttributePropertyAndCategory
    {
        [CustomElsaAttributeObject]
        public MyNestedClass CustomElsaAttributeProperty { get; set; } = null!;

        public class MyNestedClass
        {
            [HeActivityInput(Category = "TestCategory")]
            public string HeActivityInput { get; set; } = null!;
        }
    }

    public class TestClassWithOneHeActivityInputAttributeProperty
    {
        [HeActivityInput]
        public string HeActivityInput { get; set; } = null!;
    }

    public class TestClassWithMultipleHeActivityInputAttributeProperties
    {
        [HeActivityInput(Order = 2)]
        public string HeActivityInput { get; set; } = null!;
        [HeActivityInput(Order = 5)]
        public string Last { get; set; } = null!;
        [HeActivityInput(Order = 1)]
        public string First { get; set; } = null!;
    }

    public class TestClassWithOneUIHintProperty
    {
        //[HeActivityInput(OptionsProvider = typeof(IOptionsProvider))]
        [HeActivityInput(UIHint = "TestUIHint")]
        public string HeActivityInput { get; set; } = null!;
    }
}
