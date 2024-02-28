using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.Dataverse
{
    public class DataverseResults
    {
        public DataverseResults() {
            this.Columns = new string[0];
            this.Rows = new Dictionary<string, object>[0];        
        }
        public int RowCount
        {
            get
            {
                int result = this.Rows != null ? this.Rows.Length : 0;
                return result;
            }
        }

        public string[] Columns { get; set; } = new string[0];
        public Dictionary<string, object>? FirstRow
        {
            get
            {
                var result = this.Rows?.Length > 0 ? this.Rows[0] : null;
                return result;
            }
        }
        public Dictionary<string, object>[] Rows { get; set; }
    }
}
