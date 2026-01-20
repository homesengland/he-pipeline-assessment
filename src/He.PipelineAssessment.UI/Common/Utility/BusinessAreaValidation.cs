using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;

namespace He.PipelineAssessment.UI.Common.Utility
{

    public interface IBusinessAreaValidation
    {
        bool IsValidBusinessArea(string businessArea );
    }

    public class BusinessAreaValidation : IBusinessAreaValidation
    {
        public bool IsValidBusinessArea(string businessArea)
        {
            switch (businessArea)
            {
                case Constants.BusinessArea.MPP:
                    return true;
                case Constants.BusinessArea.Investment:
                    return true;
                case Constants.BusinessArea.Development:
                    return true;
                default: return false;
            }
        }
    }
}
