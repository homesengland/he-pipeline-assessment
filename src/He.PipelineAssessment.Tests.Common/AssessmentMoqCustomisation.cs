using AutoFixture;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.UI.Tests.Common.Utility
{
    public class ValidAssessmentCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<AssessmentDataViewModel>(c => c.With(x => x.ValidData, true));
            fixture.Customize<Assessment>(c => c.With(x => x.ValidData, true));
            fixture.Customize<AssessmentInterventionViewModel>(c => c.With(x => x.ValidData, true));
        }
    }
}
