namespace Elsa.CustomModels
{

    public class ActivityDataModel
    {
        public ActivityData activityData { get; set; }
        public string ActivityId { get; set; }
    }
    public class ActivityData
    {
        public string Title { get; set; }
        public Choice[] Choices { get; set; }
        public string QuestionID { get; set; }
        public string Question { get; set; }
        public Case[] Cases { get; set; }
        public object Mode { get; set; }
        public object Output { get; set; }
    }

    public class Choice
    {
        public string answer { get; set; }
        public bool isSingle { get; set; }
        public bool isSelected { get; set; }
    }

    public class Case
    {
        public string name { get; set; }
        public bool condition { get; set; }
    }
}
