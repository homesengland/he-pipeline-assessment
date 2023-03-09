using Castle.Core.Logging;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.Syntax;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Jint.Native.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Security.Principal;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers.Syntax
{
    public class NestedSyntaxExpressionHandlerTests
    {
        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsIntData_GivenIntType(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<NestedSyntaxExpressionHandler> logger)
        {
            //Arrange
            string value = "123";
            string javascriptQuery = "activities.GetTotalInt(foo, bar)";
            Type type = typeof(int);
            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value);
            ElsaProperty javascriptProperty = SampleProperty(SyntaxNames.JavaScript, type, javascriptQuery);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

                int result = Int32.Parse(value);
                evaluator.Setup(x => x.TryEvaluateAsync<int>(sampleProperty.Expressions![sampleProperty.Syntax!],
                    sampleProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(result)));

                evaluator.Setup(x => x.TryEvaluateAsync<int>(javascriptProperty.Expressions![javascriptProperty.Syntax!],
                    javascriptProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(result)));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger);

            //Act

            var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);
            var parsedJavascriptOutput = await handler.EvaluateModel(javascriptProperty!, evaluator.Object, context, type);

            //Assert
            Assert.Equal(type, output!.GetType());
            Assert.Equal(type, parsedJavascriptOutput!.GetType());
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsBoolData_GivenBoolType(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<NestedSyntaxExpressionHandler> logger)
        {
            //Arrange
            string value = "true";
            string javascriptQuery = "activities[x].Output() == 'foobar'";
            Type type = typeof(bool);
            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value);
            ElsaProperty javascriptProperty = SampleProperty(SyntaxNames.JavaScript, type, javascriptQuery);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            bool result = value == "true";
            evaluator.Setup(x => x.TryEvaluateAsync<bool>(sampleProperty.Expressions![sampleProperty.Syntax!],
                sampleProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(result)));

            evaluator.Setup(x => x.TryEvaluateAsync<bool>(javascriptProperty.Expressions![javascriptProperty.Syntax!],
                javascriptProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(result)));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger);

            //Act

            var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);
            var parsedJavascriptOutput = await handler.EvaluateModel(javascriptProperty!, evaluator.Object, context, type);

            //Assert
            Assert.Equal(type, output!.GetType());
            Assert.Equal(type, parsedJavascriptOutput!.GetType());
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsStringData_GivenStringType(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<NestedSyntaxExpressionHandler> logger)
                {
                    //Arrange
                    string value = "Success";
                    string javascriptQuery = "activities[x].Output()";
                    Type type = typeof(string);
                    ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value);
                    ElsaProperty javascriptProperty = SampleProperty(SyntaxNames.JavaScript, type, javascriptQuery);

                    var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

                    string result = value;
                    evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleProperty.Expressions![sampleProperty.Syntax!],
                        sampleProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(value)));

                    evaluator.Setup(x => x.TryEvaluateAsync<string>(javascriptProperty.Expressions![javascriptProperty.Syntax!],
                        javascriptProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(value)));

                    NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger);

                    //Act

                    var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);
                    var parsedJavascriptOutput = await handler.EvaluateModel(javascriptProperty!, evaluator.Object, context, type);

                    //Assert
                    Assert.Equal(type, output!.GetType());
                    Assert.Equal(type, parsedJavascriptOutput!.GetType());
                }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsRadioData_GivenRadioType(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<NestedSyntaxExpressionHandler> logger)
        {
            //Arrange
            string value = SampleRadioJson();
            Type type = typeof(RadioModel);
            
            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            var javascriptValue = "First Value";
            var stringValue = "Second Value";

            var propertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue },
                {SyntaxNames.Literal, stringValue }
            };
            var firstProperty = SampleElsaProperty(propertyDictionary, SyntaxNames.JavaScript, "A");
            var secondProperty = SampleElsaProperty(propertyDictionary, SyntaxNames.Literal, "B");

            var propertyList = new List<ElsaProperty>()
            {
                firstProperty,
                secondProperty
            };

            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Json, type, JsonConvert.SerializeObject(propertyList));

            List<RadioRecord>? result = JsonConvert.DeserializeObject<List<RadioRecord>>(value);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(firstProperty.Expressions![firstProperty.Syntax!],
                firstProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(javascriptValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(secondProperty.Expressions![secondProperty.Syntax!],
                secondProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(stringValue)));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger);

            //Act
            RadioModel? output = (RadioModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);


            //Assert
            if(output != null)
            {
                var expectedOutput = JsonConvert.DeserializeObject<List<RadioRecord>>(value);
                Assert.Equal(type, output!.GetType());
                Assert.Equal(expectedOutput, output.Choices);
            }
            else
            {
                Assert.True(false, "Output did not produce a non-null object");
            }
            
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsCheckboxData_GivenCheckboxType(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<NestedSyntaxExpressionHandler> logger)
        {
            //Arrange
            string value = SampleCheckboxJson();
            Type type = typeof(CheckboxModel);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            var javascriptValue = "First Value";
            var stringValue = "Second Value";

            var checkedPropertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue},
                {SyntaxNames.Literal, stringValue },
                { CustomSyntaxNames.Checked, "false"}
            };

            var uncheckedPropertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue},
                {SyntaxNames.Literal, stringValue },
                { CustomSyntaxNames.Checked, "true"}
            };
            var firstProperty = SampleElsaProperty(checkedPropertyDictionary, SyntaxNames.JavaScript, "A");
            var secondProperty = SampleElsaProperty(uncheckedPropertyDictionary, SyntaxNames.Literal, "B");

            var propertyList = new List<ElsaProperty>()
            {
                firstProperty,
                secondProperty
            };

            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Json, type, JsonConvert.SerializeObject(propertyList));

            List<CheckboxRecord>? result = JsonConvert.DeserializeObject<List<CheckboxRecord>>(value);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(firstProperty.Expressions![firstProperty.Syntax!],
                firstProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(javascriptValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(secondProperty.Expressions![secondProperty.Syntax!],
                secondProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(stringValue)));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger);

            //Act
            CheckboxModel? output = (CheckboxModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);


            //Assert
            if (output != null)
            {
                var expectedOutput = JsonConvert.DeserializeObject<List<CheckboxRecord>>(value);
                Assert.Equal(type, output!.GetType());
                Assert.Equal(expectedOutput, output.Choices);
            }
            else
            {
                Assert.True(false, "Output did not produce a non-null object");
            }

        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsDefaultValue_WhenKeyNotFoundExpressionThrown(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<NestedSyntaxExpressionHandler> logger)
        {
            string value = "123";
            Type type = typeof(int);
            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            int result = Int32.Parse(value);
            evaluator.Setup(x => x.TryEvaluateAsync<int>(sampleProperty.Expressions![sampleProperty.Syntax!],
                SyntaxNames.JavaScript, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(result)));


            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger);

            //Act

            var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);

            //Assert
            Assert.Equal(type, output!.GetType());
            Assert.Equal(default(int), output);
        }




        #region Seed Data

        private ElsaProperty SampleProperty(string syntax, Type type, string value)
        {
            IDictionary<string, string> expressions = new Dictionary<string, string>();
            expressions.Add(syntax, value);

            return new ElsaProperty(expressions, syntax, string.Empty, "Sample Property");
        }

        private string SampleRadioJson()
        {
            var records = new List<RadioRecord>()
                {
                     new RadioRecord("A", "First Value") ,
                     new RadioRecord("B", "Second Value")
            };

            return JsonConvert.SerializeObject(records);
        }

        //private string SampleCheckboxElsaPropertyJson(string selectedSyntax)
        //{
        //    var dict = new Dictionary<string, string>{
        //            {SyntaxNames.Literal, "Test" },
        //            {SyntaxNames.JavaScript, "'Test'" }
        //        };
        //    var records = new List<ElsaProperty>();
        //    records.Add(SampleElsaProperty(dict, SyntaxNames.Literal, "Property 1"));
        //    records.Add(SampleElsaProperty(dict, SyntaxNames.JavaScript, "Property 2"));

        //    return JsonConvert.SerializeObject(records);
        //}

        private string SampleCheckboxJson()
        {
            var records = new List<CheckboxRecord>()
                {
                     new CheckboxRecord("A", "First Value", false) ,
                     new CheckboxRecord("B", "Second Value", true)
            };

            return JsonConvert.SerializeObject(records);
        }

        private ElsaProperty SampleElsaProperty(Dictionary<string, string> expressions, string syntax, string name)
        {
            return new ElsaProperty(expressions, syntax, "", name);
        }

        #endregion

    }
}

