﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class CheckYourAnswersSaveAndContinueCommandDto
    {
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;
    }
}
