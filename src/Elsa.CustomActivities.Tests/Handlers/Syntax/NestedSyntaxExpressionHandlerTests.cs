using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Handlers.Models;
using Elsa.CustomActivities.Handlers.Syntax;
using Elsa.Expressions;
using Elsa.Serialization;
using Elsa.Services.Models;
using He.PipelineAssessment.Tests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Elsa.CustomActivities.Tests.Handlers.Syntax
{
    public class NestedSyntaxExpressionHandlerTests
    {
        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsIntData_GivenIntType(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            IContentSerializer serializer,
            ILogger<IExpressionHandler> logger)
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

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

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
            ILogger<IExpressionHandler> logger,
            IContentSerializer serializer)
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

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

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
            ILogger<IExpressionHandler> logger,
            IContentSerializer serializer)
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

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

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
            ILogger<IExpressionHandler> logger,
            IContentSerializer serializer)
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
                {SyntaxNames.Literal, stringValue },
                {RadioSyntaxNames.PrePopulated, "false" },
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

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            RadioModel? output = (RadioModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);


            //Assert
            if (output != null)
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
            ILogger<IExpressionHandler> logger,
            IContentSerializer serializer)
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
                {CustomSyntaxNames.Checked, "false"},
                {CheckboxSyntaxNames.Single, "false"},
                {CheckboxSyntaxNames.PrePopulated, "false"}
            };

            var uncheckedPropertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue},
                {SyntaxNames.Literal, stringValue },
                {CustomSyntaxNames.Checked, "true"},
                {CheckboxSyntaxNames.Single, "true"},
                {CheckboxSyntaxNames.PrePopulated, "false"}
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

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            CheckboxModel? output = (CheckboxModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);

        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsTextModel_GivenTextType(
           Mock<IServiceProvider> provider,
           Mock<IExpressionEvaluator> evaluator,
           ILogger<IExpressionHandler> logger,
            IContentSerializer serializer)
        {
            //Arrange
            string value = SampleTextJson();
            Type type = typeof(TextModel);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            var javascriptValue = "First Text Value";
            var stringValue = "Link Text Value";

            var textJsParagraphPropertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue},
                {SyntaxNames.Literal, stringValue },
                {TextActivitySyntaxNames.Paragraph, "true"},
                {TextActivitySyntaxNames.Guidance, "true"},
                {TextActivitySyntaxNames.Hyperlink, "false"},
                {TextActivitySyntaxNames.Url, ""},
            };

            var textLiteralNoParagraphPropertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue},
                {SyntaxNames.Literal, stringValue },
                {TextActivitySyntaxNames.Paragraph, "true"},
                {TextActivitySyntaxNames.Guidance, "false"},
                {TextActivitySyntaxNames.Hyperlink, "true"},
                {TextActivitySyntaxNames.Url, "https://www.foo.bar"},
            };
            var firstProperty = SampleElsaProperty(textJsParagraphPropertyDictionary, SyntaxNames.JavaScript, "A");
            var secondProperty = SampleElsaProperty(textLiteralNoParagraphPropertyDictionary, SyntaxNames.Literal, "B");

            var propertyList = new List<ElsaProperty>()
            {
                firstProperty,
                secondProperty
            };

            ElsaProperty sampleProperty = SampleProperty(TextActivitySyntaxNames.TextActivity, type, JsonConvert.SerializeObject(propertyList));

            List<TextRecord>? result = JsonConvert.DeserializeObject<List<TextRecord>>(value);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(firstProperty.Expressions![firstProperty.Syntax!],
                firstProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(javascriptValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(secondProperty.Expressions![secondProperty.Syntax!],
                secondProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(stringValue)));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            TextModel? output = (TextModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);


            //Assert
            if (output != null)
            {
                var expectedOutput = JsonConvert.DeserializeObject<List<TextRecord>>(value);
                Assert.Equal(type, output!.GetType());
                Assert.Equal(expectedOutput, output.TextRecords);
            }
            else
            {
                Assert.True(false, "Output did not produce a non-null object");
            }
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsDoubleModel_GivenDoubleType(
        Mock<IServiceProvider> provider,
        Mock<IExpressionEvaluator> evaluator,
        ILogger<IExpressionHandler> logger,
        IContentSerializer serializer)
        {
            //Arrange
            double value = 2.0;
            string javascriptQuery = "activities[x].Output()";
            Type type = typeof(double);
            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value.ToString());
            ElsaProperty javascriptProperty = SampleProperty(SyntaxNames.JavaScript, type, javascriptQuery);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            string result = value.ToString();
            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleProperty.Expressions![sampleProperty.Syntax!],
                sampleProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(value.ToString())));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(javascriptProperty.Expressions![javascriptProperty.Syntax!],
                javascriptProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(value.ToString())));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act

            var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);
            var parsedJavascriptOutput = await handler.EvaluateModel(javascriptProperty!, evaluator.Object, context, type);

            //Assert
            Assert.Equal(type, output!.GetType());
            Assert.Equal(type, parsedJavascriptOutput!.GetType());
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsDecimalModel_GivenDecimalType(
        Mock<IServiceProvider> provider,
        Mock<IExpressionEvaluator> evaluator,
        ILogger<IExpressionHandler> logger,
        IContentSerializer serializer)
        {
            //Arrange
            decimal value = new Decimal(2.0);
            Type type = typeof(decimal);
            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value.ToString());

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            evaluator.Setup(x => x.TryEvaluateAsync<decimal?>("2", SyntaxNames.Literal, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(new decimal?(2))));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);

            //Assert
            Assert.Equal(type, output!.GetType());
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsPotScoreRadioData_GivenPotScoreRadioType(
        Mock<IServiceProvider> provider,
        Mock<IExpressionEvaluator> evaluator,
        ILogger<IExpressionHandler> logger,
        IContentSerializer serializer)
        {
            //Arrange
            var javascriptValue = "First Value";
            var stringValue = "Second Value";
            var potValue = "High";
            string value = PotScoreRadioJson(javascriptValue, stringValue, potValue);
            Type type = typeof(PotScoreRadioModel);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);



            var propertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue },
                {SyntaxNames.Literal, stringValue },
                {RadioSyntaxNames.PrePopulated, "false" },
                {CustomSyntaxNames.PotScore, potValue }
            };
            var firstProperty = SampleElsaProperty(propertyDictionary, SyntaxNames.JavaScript, "A");
            var secondProperty = SampleElsaProperty(propertyDictionary, SyntaxNames.Literal, "B");

            var propertyList = new List<ElsaProperty>()
            {
                firstProperty,
                secondProperty
            };

            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Json, type, JsonConvert.SerializeObject(propertyList));

            List<PotScoreRadioRecord>? result = JsonConvert.DeserializeObject<List<PotScoreRadioRecord>>(value);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(firstProperty.Expressions![firstProperty.Syntax!],
                firstProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(javascriptValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(secondProperty.Expressions![secondProperty.Syntax!],
                secondProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(stringValue)));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            PotScoreRadioModel? output = (PotScoreRadioModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);


            //Assert
            if (output != null)
            {
                var expectedOutput = JsonConvert.DeserializeObject<List<PotScoreRadioRecord>>(value);
                Assert.Equal(type, output!.GetType());
                Assert.Equal(expectedOutput, output.Choices);
            }
            else
            {
                Assert.True(false, "Output did not produce a non-null object");
            }

        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsWeighedScoreRadioData_GivenWeighedScoreRadioType(
        Mock<IServiceProvider> provider,
        Mock<IExpressionEvaluator> evaluator,
        ILogger<IExpressionHandler> logger,
        IContentSerializer serializer)
        {
            //Arrange
            var javascriptValue = "First Value";
            var stringValue = "Second Value";
            var scoreValue = 10;
            string value = WeightedScoreRadioJson(javascriptValue, stringValue, scoreValue);
            Type type = typeof(WeightedRadioModel);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            var propertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue },
                {SyntaxNames.Literal, stringValue },
                {RadioSyntaxNames.PrePopulated, "false" },
                {ScoringSyntaxNames.Score, "10" },
            };
            var firstProperty = SampleElsaProperty(propertyDictionary, SyntaxNames.JavaScript, "A");
            var secondProperty = SampleElsaProperty(propertyDictionary, SyntaxNames.Literal, "B");

            var propertyList = new List<ElsaProperty>()
            {
                firstProperty,
                secondProperty
            };

            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Json, type, JsonConvert.SerializeObject(propertyList));

            List<PotScoreRadioRecord>? result = JsonConvert.DeserializeObject<List<PotScoreRadioRecord>>(value);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(firstProperty.Expressions![firstProperty.Syntax!],
                firstProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(javascriptValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(secondProperty.Expressions![secondProperty.Syntax!],
                secondProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(stringValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<decimal>("10", SyntaxNames.Literal, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(new Decimal(10))));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            WeightedRadioModel? output = (WeightedRadioModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);


            //Assert
            if (output != null)
            {
                var expectedOutput = JsonConvert.DeserializeObject<List<WeightedRadioRecord>>(value);
                Assert.Equal(type, output!.GetType());
                Assert.Equal(expectedOutput, output.Choices);
            }
            else
            {
                Assert.True(false, "Output did not produce a non-null object");
            }

        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsWeighedCheckboxData_GivenWeighedCheckboxType(
        Mock<IServiceProvider> provider,
        Mock<IExpressionEvaluator> evaluator,
        ILogger<IExpressionHandler> logger,
        IContentSerializer serializer)
        {
            //Arrange
            var javascriptValue = "First Value";
            var stringValue = "Second Value";
            var maxScoreValue = new Decimal?(10);
            var choiceScoreValue = new Decimal(4);
            string value = WeightedScoreCheckboxJson(javascriptValue, stringValue, false, maxScoreValue, choiceScoreValue);
            Type type = typeof(WeightedCheckboxModel);
            Type choiceType = typeof(WeightedCheckboxRecord);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            ElsaProperty groupArrayScore = CreateGroupArrayProperty();

            var choicesPropertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue },
                {SyntaxNames.Literal, stringValue },
                {RadioSyntaxNames.PrePopulated, "false" },
                {ScoringSyntaxNames.Score, "4" },
            };
            var firstChoiceProperty = SampleElsaProperty(choicesPropertyDictionary, SyntaxNames.JavaScript, "A");
            var secondChoiceProperty = SampleElsaProperty(choicesPropertyDictionary, SyntaxNames.Literal, "B");

            var choicesPropertyList = new List<ElsaProperty>()
            {
                firstChoiceProperty,
                secondChoiceProperty
            };

            var propertyDictionary = new Dictionary<string, string>()
            {
                {SyntaxNames.JavaScript, javascriptValue },
                {SyntaxNames.Literal, stringValue },
                {ScoringSyntaxNames.MaxGroupScore, "10" },
                {ScoringSyntaxNames.GroupArrayScore, JsonConvert.SerializeObject(groupArrayScore) },
                {SyntaxNames.Json, JsonConvert.SerializeObject(choicesPropertyList) }
            };

            var groupProperty = SampleElsaProperty(propertyDictionary, SyntaxNames.JavaScript, "GroupA");
            var groupPropertyList = new List<ElsaProperty>()
            {
                groupProperty
            };

            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Json, type, JsonConvert.SerializeObject(groupPropertyList));

            List<PotScoreRadioRecord>? result = JsonConvert.DeserializeObject<List<PotScoreRadioRecord>>(value);
            evaluator.Setup(x => x.TryEvaluateAsync<string>(firstChoiceProperty.Expressions![firstChoiceProperty.Syntax!],
                firstChoiceProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(javascriptValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<string>(secondChoiceProperty.Expressions![secondChoiceProperty.Syntax!],
                secondChoiceProperty.Syntax!, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(stringValue)));

            evaluator.Setup(x => x.TryEvaluateAsync<decimal>("4", SyntaxNames.Literal, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(new Decimal(4))));
            evaluator.Setup(x => x.TryEvaluateAsync<decimal?>("10", SyntaxNames.Literal, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(new Decimal?(10))));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            WeightedCheckboxModel? output = (WeightedCheckboxModel?)await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);


            //Assert
            if (output != null)
            {
                var expectedOutput = JsonConvert.DeserializeObject<List<WeightedCheckboxGroup>>(value);
                Assert.Equal(type, output!.GetType());
                Assert.Equal(expectedOutput![0].MaxGroupScore, output.Groups["GroupA"].MaxGroupScore);
                Assert.Equal(expectedOutput![0].GroupIdentifier, output.Groups["GroupA"].GroupIdentifier);
                Assert.Equal(expectedOutput![0].Choices, output.Groups["GroupA"].Choices);
                Assert.Equal(expectedOutput![0].GroupArrayScore, output.Groups["GroupA"].GroupArrayScore);
            }
            else
            {
                Assert.True(false, "Output did not produce a non-null object");
            }

        }

        private ElsaProperty CreateGroupArrayProperty()
        {
            var groupArrayScoreProperty = new Dictionary<string, string>()
            {
                {SyntaxNames.Json, "[1.0, 2.0, 3.0]" }
            };
            var groupArrayScore = SampleElsaProperty(groupArrayScoreProperty, ScoringSyntaxNames.GroupArrayScore, "GroupArrayScore");
            return groupArrayScore;
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsDefaultValue_WhenKeyNotFoundExpressionThrown(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<IExpressionHandler> logger,
            IContentSerializer serializer)
        {
            string value = "123";
            Type type = typeof(int);
            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            int result = Int32.Parse(value);
            evaluator.Setup(x => x.TryEvaluateAsync<int>(sampleProperty.Expressions![sampleProperty.Syntax!],
                SyntaxNames.JavaScript, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success(result)));


            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act

            var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);

            //Assert
            Assert.Equal(type, output!.GetType());
            Assert.Equal(default(int), output);
        }

        [Theory, AutoMoqData]
        public async void EvaluateFromExpression_ReturnsValueAsString_GivenTypeNotHandled(
            Mock<IServiceProvider> provider,
            Mock<IExpressionEvaluator> evaluator,
            ILogger<IExpressionHandler> logger,
            IContentSerializer serializer)
        {
            string value = "123";
            Type type = typeof(List<string>);
            ElsaProperty sampleProperty = SampleProperty(SyntaxNames.Literal, type, value);

            var context = new ActivityExecutionContext(provider.Object, default!, default!, default!, default, default);

            evaluator.Setup(x => x.TryEvaluateAsync<string>(sampleProperty.Expressions![sampleProperty.Syntax!],
                SyntaxNames.Literal, context, CancellationToken.None)).Returns(Task.FromResult(Models.Result.Success<string?>(value)));

            NestedSyntaxExpressionHandler handler = new NestedSyntaxExpressionHandler(logger, serializer);

            //Act
            var output = await handler.EvaluateModel(sampleProperty!, evaluator.Object, context, type);

            //Assert
            Assert.Equal(typeof(string), output!.GetType());
            Assert.Equal(value, output);
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
                     new RadioRecord("A", "First Value", false) ,
                     new RadioRecord("B", "Second Value",false)
            };

            return JsonConvert.SerializeObject(records);
        }

        private string PotScoreRadioJson(string firstValue, string secondValue, string potScore)
        {
            var records = new List<PotScoreRadioRecord>()
                {
                     new PotScoreRadioRecord("A", firstValue, potScore, false) ,
                     new PotScoreRadioRecord("B", secondValue, potScore, false)
            };

            return JsonConvert.SerializeObject(records);
        }


        private string WeightedScoreRadioJson(string firstValue, string secondValue, decimal score)
        {
            var records = new List<WeightedRadioRecord>()
                {
                     new WeightedRadioRecord("A", firstValue, score, false) ,
                     new WeightedRadioRecord("B", secondValue, score, false)
            };

            return JsonConvert.SerializeObject(records);
        }

        private string WeightedScoreCheckboxJson(string firstValue, string secondValue, bool isSingle, decimal? maxGroupScore, decimal choiceScore)
        {
            var groups = new List<WeightedCheckboxGroup>();
            var group = new WeightedCheckboxGroup();
            group.Choices = new List<WeightedCheckboxRecord>()
            {
                new WeightedCheckboxRecord("A", firstValue, isSingle,choiceScore, false) ,
                new WeightedCheckboxRecord("B", secondValue, isSingle,choiceScore , false)
            };
            group.MaxGroupScore = maxGroupScore;
            group.GroupIdentifier = "GroupA";
            group.GroupArrayScore = new List<decimal>() { 1, 2, 3 };
            groups.Add(group);


            return JsonConvert.SerializeObject(groups);
        }

        private string SampleCheckboxJson()
        {
            var records = new List<CheckboxRecord>()
                {
                     new CheckboxRecord("A", "First Value", false,false, false) ,
                     new CheckboxRecord("B", "Second Value", true,false, true)
            };

            return JsonConvert.SerializeObject(records);
        }

        private string SampleTextJson()
        {
            var records = new List<TextRecord>()
                {
                     new TextRecord("First Text Value", true, true, false, string.Empty),
                     new TextRecord("Link Text Value", true, false, true, "https://www.foo.bar")
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

