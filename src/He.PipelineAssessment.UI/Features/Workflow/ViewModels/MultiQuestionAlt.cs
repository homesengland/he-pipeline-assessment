﻿using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels.Alt
{
    public class MultiQuestion
    {
        public int Index { get; set; }
        public bool IsValid { get; set; }
        public string QuestionId { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public bool DisplayComments { get; set; }
        public string? Comments { get; set; }
        public string? Answer { get; set; }
        public bool IsReadOnly { get; set; }
        public Date? Date { get; set; }
        public decimal? Decimal { get; set; }
        public Radio Radio { get; set; } = new Radio();
        public Checkbox Checkbox { get; set; } = new Checkbox();
        public Information Information { get; set; } = new Information();
        public int? CharacterLimit { get; set; }
    }

    public class Information
    {
        public List<InformationText> Text { get; set; } = new List<InformationText>();
    }

    public class Checkbox
    {
        public List<string> SelectedChoices { get; set; } = null!;
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    public class Radio
    {
        public List<Choice> Choices { get; set; } = new List<Choice>();
        public string SelectedAnswer { get; set; } = null!;
    }

    public class InformationText
    {
        public string Text { get; set; } = null!;
        public bool IsParagraph { get; set; } = true;
        public bool IsGuidance { get; set; } = false;
        public bool IsHyperlink { get; set; } = false;
        public string? Url { get; set; }
    }


    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
    }

    public class Date
    {
        [Range(1, 31)]
        public int? Day { get; set; }
        [Range(1, 12)]
        public int? Month { get; set; }
        [Range(1, 3000)]
        public int? Year { get; set; }
    }
}
