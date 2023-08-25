using FluentValidation;
using System.Reflection.Metadata.Ecma335;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class TextValidator : AbstractValidator<QuestionActivityData>
    {
        public TextValidator()
        {
            RuleFor(x => x.Answers).NotEmpty().WithMessage("The question has not been answered")
                .DependentRules(
                    () =>
                    {
                        RuleForEach(x => x.Answers).NotEmpty().WithMessage("The question has not been answered");
                        RuleForEach(x => x.Answers).Must(answer =>
                        {
                            var isNotNull = answer.AnswerText != null && answer.AnswerText != string.Empty;
                            return isNotNull;
                        }).WithMessage("The question has not been answered");
                    }
                );

            RuleFor(x => x).Must(BeLessCharactersThanMaxLength).WithMessage("The text area has exceeded the maximum allowed characters");


            bool BeLessCharactersThanMaxLength(QuestionActivityData data)
            {
                if (data.QuestionType == QuestionTypeConstants.TextAreaQuestion)
                {
                    if (data.CharacterLimit != null)
                    {
                        if(data.Answers.FirstOrDefault()!.AnswerText != null)
                        {
                            int lengthOfAnswer = data.Answers.FirstOrDefault()!.AnswerText!.ToCharArray().Length;
                            bool result = lengthOfAnswer <= data.CharacterLimit;
                            return result;
                        }
                    }
                    return false;
                }
                else return true;
            }
        }
    }
}
