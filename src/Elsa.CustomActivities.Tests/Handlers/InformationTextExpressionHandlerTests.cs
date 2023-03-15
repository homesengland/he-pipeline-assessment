using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.Syntax;
using Elsa.Expressions;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers
{
    public class InformationTextExpressionHandlerTests
    {

        [Theory, AutoMoqData]
        public async void EvaluateAsync_ReturnsInformationTextData(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator)
        {
            //Arrange
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);
            List<ElsaProperty> informationTextProperties = new List<ElsaProperty>();


            //Act



            //Assert


        }
    }
}
