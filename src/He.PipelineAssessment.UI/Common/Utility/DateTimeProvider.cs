namespace He.PipelineAssessment.UI.Common.Utility
{    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
