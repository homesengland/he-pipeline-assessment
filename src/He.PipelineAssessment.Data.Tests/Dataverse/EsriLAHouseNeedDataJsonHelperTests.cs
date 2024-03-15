using He.PipelineAssessment.Data.Dataverse;
using He.PipelineAssessment.Data.LaHouseNeed;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Xrm.Sdk;
using System.Text.Json;
using Xunit;

namespace He.PipelineAssessment.Data.Tests.Dataverse
{
    public class DataverseResultConverterTests
    {
        [Fact]
        public void Empty_Result_Convert()
        {
            //Arrange
            List<Entity> records = new List<Entity>();
            var converter = new DataverseResultConverter();

            //Act
            var result = converter.Convert(records);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(records.Count, result.RowCount);
            Assert.Equal(records.Count, result.Rows.Length);
            Assert.Null(result.FirstRow);
        }

        [Fact]
        public void SomeData_Result_Convert()
        {
            //Arrange
            string stringFieldName = "Field1";
            string optionSetFieldName = "Field2";
            string optionSetCollectionFieldName = "Field3";
            string entityReferenceFieldName = "Field4";

            List<Entity> records = new List<Entity>();
            var converter = new DataverseResultConverter();
            var record1 = new Entity();
            records.Add(record1);
            record1[stringFieldName] = "Some text";
            record1[optionSetFieldName] = new Microsoft.Xrm.Sdk.OptionSetValue(1);
            record1.FormattedValues.Add(optionSetFieldName, "Value1");

            record1[optionSetCollectionFieldName] = new OptionSetValueCollection(
                new List<Microsoft.Xrm.Sdk.OptionSetValue>()
                {
                new Microsoft.Xrm.Sdk.OptionSetValue(1),
                new Microsoft.Xrm.Sdk.OptionSetValue(2),
                new Microsoft.Xrm.Sdk.OptionSetValue(3)
                }
            );
            record1.FormattedValues.Add(optionSetCollectionFieldName, "Value1, Value2, Value 3");

            record1[entityReferenceFieldName] = new EntityReference() {
                LogicalName = "account",
                Id = Guid.NewGuid(),
                Name = "Account 1"
            };

            //Act
            var result = converter.Convert(records);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(records.Count, result.RowCount);
            Assert.Equal(records.Count, result.Rows.Length);
            Assert.NotNull(result.FirstRow);
            
            //String field
            Assert.Equal(record1[stringFieldName], result.FirstRow![stringFieldName]);

            //Optionset Value
            Assert.Equal(
                ((Microsoft.Xrm.Sdk.OptionSetValue)(record1[optionSetFieldName])).Value, 
                ((Data.Dataverse.OptionSetValue)(result.FirstRow[optionSetFieldName])).Value);
            Assert.Equal(
                record1.FormattedValues[optionSetFieldName],
                ((Data.Dataverse.OptionSetValue)(result.FirstRow[optionSetFieldName])).Name);

            //Optionset Collection Value
            Assert.Equal(
                ((Microsoft.Xrm.Sdk.OptionSetValueCollection)(record1[optionSetCollectionFieldName])).Count,
                ((Data.Dataverse.OptionSetValues)(result.FirstRow[optionSetCollectionFieldName])).Values.Length);
            Assert.Equal(
                record1.FormattedValues[optionSetCollectionFieldName],
                ((Data.Dataverse.OptionSetValues)(result.FirstRow[optionSetCollectionFieldName])).FormatedNames);

            //Entity Reference
            Assert.Equal(
                ((EntityReference)(record1[entityReferenceFieldName])).Name,
                ((EntityReference)(result.FirstRow[entityReferenceFieldName])).Name);
            Assert.Equal(
                ((EntityReference)(record1[entityReferenceFieldName])).Id,
                ((EntityReference)(result.FirstRow[entityReferenceFieldName])).Id);
            Assert.Equal(
                ((EntityReference)(record1[entityReferenceFieldName])).LogicalName,
                ((EntityReference)(result.FirstRow[entityReferenceFieldName])).LogicalName);
        }
    }
}
