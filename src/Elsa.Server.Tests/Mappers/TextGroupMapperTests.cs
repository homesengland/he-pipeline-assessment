using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomModels;
using Elsa.Server.Features.Workflow.LoadConfirmationScreen;
using He.PipelineAssessment.Tests.Common;
using Elsa.Server.Mappers;
using Xunit;

namespace Elsa.Server.Tests.Mappers
{
    public class TextGroupMapperTests
    {
        [Theory]
        [AutoMoqData]

        public void InformationListFromGroupedTextModel_MapsCorrectly(
            GroupedTextModel groupedTextModel,
            TextGroupMapper sut)
        {
            //Arrange

            //Act
            var result = sut.InformationListFromGroupedTextModel(groupedTextModel);

            //Assert
            Assert.IsType<List<Information>>(result);
            Assert.Equal(groupedTextModel.TextGroups.Count, result.Count);
            for (var i = 0; i < result.Count; i++)
            {
                var expected = groupedTextModel.TextGroups[i];
                var actual = result[i];
                Assert.Equal(expected.Bullets, actual.IsBullets);
                Assert.Equal(expected.Collapsed, actual.IsCollapsed);
                Assert.Equal(expected.Guidance, actual.IsGuidance);
                Assert.Equal(expected.Title, actual.Title);
                for (var j = 0; j < expected.TextRecords.Count; j++)
                {
                    var expectedTextRecord = expected.TextRecords[j];
                    var actualInformationText = actual.InformationTextList[j];

                    Assert.Equal(expectedTextRecord.IsBold, actualInformationText.IsBold);
                    Assert.Equal(expectedTextRecord.IsHyperlink, actualInformationText.IsHyperlink);
                    Assert.Equal(expectedTextRecord.IsParagraph, actualInformationText.IsParagraph);
                    Assert.Equal(expectedTextRecord.Url, actualInformationText.Url);
                    Assert.Equal(expectedTextRecord.Text, actualInformationText.Text);
                }
            }
        }

        [Theory]
        [AutoMoqData]
        public void InformationTextGroupListFromTextGroups_MapsCorrectly(
            List<TextGroup> textGroups,
            TextGroupMapper sut)
        {
            //Arrange

            //Act
            var result = sut.InformationTextGroupListFromTextGroups(textGroups);

            //Assert
            Assert.IsType<List<InformationTextGroup>>(result);
            Assert.Equal(textGroups.Count, result.Count);
            for (var i = 0; i < result.Count; i++)
            {
                var expected = textGroups[i];
                var actual = result[i];
                Assert.Equal(expected.Bullets, actual.IsBullets);
                Assert.Equal(expected.Collapsed, actual.IsCollapsed);
                Assert.Equal(expected.Guidance, actual.IsGuidance);
                Assert.Equal(expected.Title, actual.Title);
                for (var j = 0; j < expected.TextRecords.Count; j++)
                {
                    var expectedTextRecord = expected.TextRecords[j];
                    var actualInformationText = actual.InformationTextList[j];

                    Assert.Equal(expectedTextRecord.IsBold, actualInformationText.IsBold);
                    Assert.Equal(expectedTextRecord.IsHyperlink, actualInformationText.IsHyperlink);
                    Assert.Equal(expectedTextRecord.IsParagraph, actualInformationText.IsParagraph);
                    Assert.Equal(expectedTextRecord.Url, actualInformationText.Url);
                    Assert.Equal(expectedTextRecord.Text, actualInformationText.Text);
                }
            }
        }
    }
}
