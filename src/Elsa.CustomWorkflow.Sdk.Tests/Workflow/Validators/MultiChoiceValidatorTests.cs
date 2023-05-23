using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation.TestHelper;
using System.Text.RegularExpressions;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Workflow.Validators
{
    public class MultiChoiceValidatorTests
    {
        [Fact]
        public void Should_Have_Error_When_Multiple_Exclusive_Choices_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Id = 1,
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Id = 2,
                    Answer = "Test 2",
                    IsSingle = true
                },
                new Choice
                {
                    Id = 3,
                    Answer = "Test 3",
                    IsSingle = false
                   },
                new Choice
                {
                    Id = 4,
                    Answer = "Test 4",
                    IsSingle = true
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 2, 4 };

            //Act
            var expectedValidationResult = validator.TestValidate(multiChoice);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1, Test 2 and Test 4 cannot be selected with any other answer in the group.");
        }

        [Fact]
        public void Should_Have_Error_When_Exclusive_And_Nonexclusive_Choices_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Id = 1,
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Id = 2,
                    Answer = "Test 2",
                    IsSingle = false
                },
                new Choice
                {
                    Id = 3,
                    Answer = "Test 3",
                    IsSingle = true
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 2 };

            var result = validator.TestValidate(multiChoice);

            //Assert

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1 cannot be selected with any other answer in the group.");
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Single_Exclusive_Choice_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = true
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Multiple_Nonexclusive_Choices_Selected()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = false
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 2, 3 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);

        }

        [Fact]
        public void Should_Have_Error_When_SelectedChoicesEmpty()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox
            {
                Choices = new List<Choice>
                {
                    new Choice
                    {
                        Answer = "Test 1",
                        IsSingle = true
                    },
                    new Choice
                    {
                        Answer = "Test 2",
                        IsSingle = false
                    },
                    new Choice
                    {
                        Answer = "Test 3",
                        IsSingle = false
                    },
                },
                SelectedChoices = new List<int>()
            };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldHaveValidationErrorFor(c => c.SelectedChoices).WithErrorMessage("The question has not been answered");
        }

        [Fact]
        public void Should_Have_Error_When_Multiple_Exclusive_Choices_Selected_Within_Group()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };
            var groupB = new ChoiceGroup() { GroupIdentifier = "B" };
            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Id = 1,
                    Answer = "Test 1",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 2,
                    Answer = "Test 2",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 3,
                    Answer = "Test 3",
                    IsSingle = false,
                    QuestionChoiceGroup = groupA
                   },
                new Choice
                {
                    Id = 4,
                    Answer = "Test 4",
                    IsSingle = true,
                    QuestionChoiceGroup = groupB
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 2, 4 };

            //Act
            var expectedValidationResult = validator.TestValidate(multiChoice);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1 and Test 2 cannot be selected with any other answer in the group.");
        }

        [Fact]
        public void Should_Have_Error_When_Multiple_Exclusive_Choices_Selected_Within_Second_Group()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };
            var groupB = new ChoiceGroup() { GroupIdentifier = "B" };
            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Id = 1,
                    Answer = "Test 1",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 2,
                    Answer = "Test 2",
                    IsSingle = true,
                    QuestionChoiceGroup = groupB
                },
                new Choice
                {
                    Id = 3,
                    Answer = "Test 3",
                    IsSingle = false,
                    QuestionChoiceGroup = groupB
                },
                new Choice
                {
                    Id = 4,
                    Answer = "Test 4",
                    IsSingle = true,
                    QuestionChoiceGroup = groupB
                },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 2, 4 };

            //Act
            var expectedValidationResult = validator.TestValidate(multiChoice);

            //Assert
            expectedValidationResult.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 2 and Test 4 cannot be selected with any other answer in the group.");
        }

        [Fact]
        public void Should_Have_Error_When_Exclusive_And_Nonexclusive_Choices_Selected_Within_Group()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };
            var groupB = new ChoiceGroup() { GroupIdentifier = "B" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Id = 1,
                    Answer = "Test 1",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 2,
                    Answer = "Test 2",
                    IsSingle = false,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 3,
                    Answer = "Test 3",
                    IsSingle = true,
                    QuestionChoiceGroup = groupB
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 2 };

            var result = validator.TestValidate(multiChoice);

            //Assert

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1 cannot be selected with any other answer in the group.");
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Single_Exclusive_Choice_Selected_Within_Group()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Multiple_Nonexclusive_Choices_Selected_Within_Group()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = false,
                    QuestionChoiceGroup = groupA
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 2, 3 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);

        }

        [Fact]
        public void Should_Not_Have_Error_When_Multiple_Exclusive_Choices_Selected_From_Different_Groups()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = true,
                },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 3 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Have_Error_When_ExclusiveToQuestion_And_NonexclusiveToQuestion_Choices_Selected_Within_Question()
        {
            //TODO
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };
            var groupB = new ChoiceGroup() { GroupIdentifier = "B" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Id = 1,
                    Answer = "Test 1",
                    IsSingle = false,
                    IsExclusiveToQuestion = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 2,
                    Answer = "Test 2",
                    IsSingle = false,
                    IsExclusiveToQuestion = false,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 3,
                    Answer = "Test 3",
                    IsSingle = false,
                    IsExclusiveToQuestion = false,
                    QuestionChoiceGroup = groupB
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 2 };

            var result = validator.TestValidate(multiChoice);

            //Assert

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1 cannot be selected with any other answer in the question.");
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Have_Error_When_Multiple_ExclusiveToQuestion_Choices_Selected_Within_Question()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };
            var groupB = new ChoiceGroup() { GroupIdentifier = "B" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Id = 1,
                    Answer = "Test 1",
                    IsSingle = false,
                    IsExclusiveToQuestion = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 2,
                    Answer = "Test 2",
                    IsSingle = false,
                    IsExclusiveToQuestion = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Id = 3,
                    Answer = "Test 3",
                    IsSingle = false,
                    IsExclusiveToQuestion = true,
                    QuestionChoiceGroup = groupB
                },
            };
            multiChoice.SelectedChoices = new List<int>() { 1, 2 };

            var result = validator.TestValidate(multiChoice);

            //Assert

            result.ShouldHaveValidationErrorFor(c => c)
                .WithErrorMessage("Test 1 and Test 2 cannot be selected with any other answer in the question.");
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Single_ExclusiveToQuestion_Choice_Selected_Within_Question()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = false,
                    IsExclusiveToQuestion = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false,
                    IsExclusiveToQuestion = true,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 3",
                    IsSingle = false,
                    IsExclusiveToQuestion = true,
                    QuestionChoiceGroup = groupA
                   },
            };
            multiChoice.SelectedChoices = new List<int>() { 1 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Not_Have_Error_When__Single_Choice_SelectedFromDifferentGroup_Within_Question()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };
            var groupB = new ChoiceGroup() { GroupIdentifier = "B" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true,
                    IsExclusiveToQuestion = false,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = true,
                    IsExclusiveToQuestion = false,
                    QuestionChoiceGroup = groupB
                }
            };

            multiChoice.SelectedChoices = new List<int>() { 1, 2 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Single_Choice_SelectedOneGroup_Within_Question()
        {
            //Arrange
            MultiChoiceValidator validator = new MultiChoiceValidator();
            Checkbox multiChoice = new Checkbox();
            var groupA = new ChoiceGroup() { GroupIdentifier = "A" };
            var groupB = new ChoiceGroup() { GroupIdentifier = "B" };

            multiChoice.Choices = new List<Choice>
            {
                new Choice
                {
                    Answer = "Test 1",
                    IsSingle = true,
                    IsExclusiveToQuestion = false,
                    QuestionChoiceGroup = groupA
                },
                new Choice
                {
                    Answer = "Test 2",
                    IsSingle = false,
                    IsExclusiveToQuestion = false,
                    QuestionChoiceGroup = groupB
                }
            };

            multiChoice.SelectedChoices = new List<int>() { 1, 2 };

            //Act
            var result = validator.TestValidate(multiChoice);

            //Assert
            result.ShouldNotHaveValidationErrorFor(c => c);
            result.ShouldNotHaveValidationErrorFor(c => c.SelectedChoices);
        }

    }
}
