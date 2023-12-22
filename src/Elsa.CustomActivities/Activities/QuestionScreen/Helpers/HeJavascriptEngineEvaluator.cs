//using Elsa.CustomInfrastructure.Data.Repository;
//using Elsa.CustomWorkflow.Sdk;
//using Elsa.Scripting.JavaScript.Messages;
//using Elsa.Scripting.JavaScript.Options;
//using Elsa.Scripting.JavaScript.Services;
//using Elsa.Services.Models;
//using Jint;
//using Jint.Native;
//using Jint.Runtime.Interop;
//using MediatR;
//using Microsoft.Extensions.Options;

//namespace Elsa.CustomActivities.Activities.QuestionScreen.Helpers
//{
//    public class HeJintJavaScriptEvaluator : IJavaScriptService
//    {
//        private readonly IMediator _mediator;
//        private readonly IConvertsJintEvaluationResult _resultConverter;
//        private readonly ScriptOptions _scriptOptions;

//        public HeJintJavaScriptEvaluator(
//            IMediator mediator,
//            IOptions<ScriptOptions> scriptOptions,
//            IConvertsJintEvaluationResult resultConverter, 
//            IElsaCustomRepository elsaCustomRepository)
//        {
//            if (scriptOptions is null)
//                throw new ArgumentNullException(nameof(scriptOptions));

//            _mediator = mediator;
//            _resultConverter = resultConverter;
//            _scriptOptions = scriptOptions.Value;
//        }

//        public async Task<object?> EvaluateAsync(
//            string expression,
//            Type returnType,
//            ActivityExecutionContext context,
//            Action<Engine>? configureEngine = default,
//            CancellationToken cancellationToken = default)
//        {
//            var engine = await GetConfiguredEngine(configureEngine, context, cancellationToken);
//            var result = ExecuteExpressionAndGetResult(engine, expression);

//            return _resultConverter.ConvertToDesiredType(result, returnType);
//        }

//        private async Task<Engine> GetConfiguredEngine(Action<Engine>? configureEngine, ActivityExecutionContext context, CancellationToken cancellationToken)
//        {
//            var engine = new Engine(async opts =>
//            {
//                await _mediator.Publish(new ConfigureJavaScriptOptions(opts, context));

//                opts.AddObjectConverter<ByteArrayConverter>();
//                if (_scriptOptions.AllowClr)
//                    opts.AllowClr();
//            });

//            configureEngine?.Invoke(engine);

//            // Listeners invoked by the mediator might further-configure the engine
//            await _mediator.Publish(new HeEvaluatingJavaScriptExpression(engine, context), cancellationToken);

//            return engine;
//        }

//        private static object? ExecuteExpressionAndGetResult(Engine engine, string expression)
//        {
//            return engine.Evaluate(expression).ToObject();
//        }
//    }



//    internal class ByteArrayConverter : IObjectConverter
//    {
//        public bool TryConvert(Engine engine, object value, out JsValue result)
//        {
//            result = JsValue.Null;

//            if (value is not byte[] buffer)
//                return false;

//            result = new ObjectWrapper(engine, buffer);

//            return true;
//        }
//    }

//public class HeEvaluatingJavaScriptExpression : INotification
//{
//    public HeEvaluatingJavaScriptExpression(Engine engine, ActivityExecutionContext activityExecutionContext)
//    {
//        Engine = engine;
//        ActivityExecutionContext = activityExecutionContext;
//    }

//    public Engine Engine { get; }
//    public ActivityExecutionContext ActivityExecutionContext { get; }
//}
//}
