using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.ParseModels;
using Elsa.CustomActivities.Handlers.Syntax;
using He.PipelineAssessment.Tests.Common;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers.Syntax
{
    
    public class TypeConverterTests
    {
        [Theory, AutoMoqData]
        public void CanSerializeValue_GivenObjectHasObjectTypeProperty(ElsaProperty elsaProperty, HeActivityInputDescriptor descriptor)
        {
            //Arrange
            descriptor.DefaultValue = typeof(CheckboxModel).FullName;
            ActivityPropertyRaw sut = new ActivityPropertyRaw(elsaProperty, descriptor);
            string valueJson = JsonConvert.SerializeObject(sut);
            TypeConverter converter = new TypeConverter();
            //Act

            ActivityPropertyRaw? converted = JsonConvert.DeserializeObject<ActivityPropertyRaw>(valueJson, converter);
            ActivityPropertyRaw? convertedWithoutDescriporDefaultValue = JsonConvert.DeserializeObject<ActivityPropertyRaw>(valueJson);

            //Assert
            if (converted != null)
            {
                Assert.Equal(sut.Value.ToString(), converted.Value.ToString());
                Assert.Equal(sut.Descriptor.ToString(), converted.Descriptor.ToString());
            }
            else
            {
                Assert.False(true, "Converted Value should not be null");
            }
        }
    }


    public class TestTypeConversionObject
    {
        public string SampleName { get; set; } = null!;
        public Type TypeToConvert { get; set; } = null!;
        public Type? NullableTypeToConvert { get; set; }
    }

}
