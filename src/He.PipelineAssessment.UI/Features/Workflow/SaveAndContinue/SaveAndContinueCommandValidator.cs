﻿using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandValidator : AbstractValidator<SaveAndContinueCommand>
    {
        public SaveAndContinueCommandValidator()
        {

            RuleFor(x => x.Data.MultiQuestionActivityData)
                .ForEach(x =>
                {
                    x.SetValidator(new MultiQuestionActivityDataValidator());
                })
                .When(x => x.Data.ActivityType == ActivityTypeConstants.QuestionScreen); ;
        }
    }
}
