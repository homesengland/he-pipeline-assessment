using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using He.PipelineAssessment.UI.Features.Intervention.InterventionList;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionList
{
    public class InterventionListRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsError_GivenRepoThrowsError(
            [Frozen] Mock<IStoredProcedureRepository> repo,
            InterventionListRequest interventionListRequest,
            Exception exception,
            InterventionListRequestHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetInterventionList()).Throws(exception);

            //Act
            var result = await sut.Handle(interventionListRequest, CancellationToken.None);

            //Assert
            Assert.Empty(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsInterventionListData_GivenNoErrorsEncountered(
            [Frozen] Mock<IStoredProcedureRepository> repo,
             InterventionListRequest interventionListRequest,
            List<AssessmentInterventionViewModel> interventions,
            InterventionListRequestHandler sut
        )
        {
            //Arrange
            repo.Setup(x => x.GetInterventionList())
                .ReturnsAsync(interventions);

            //Act
            var result = await sut.Handle(interventionListRequest, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(interventions.Count(), result.Count());
        }
    }
}
