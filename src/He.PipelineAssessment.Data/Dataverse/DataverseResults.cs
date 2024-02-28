using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.Dataverse
{
    public class DataverseResults
    {
        public int RowCount
        {
            get
            {
                int result = this.Rows != null ? this.Rows.Length : 0;
                return result;
            }
        }

        public string[]? Columns { get; set; }
        public Dictionary<string, object>? FirstRow
        {
            get
            {
                var result = this.Rows?.Length > 0 ? this.Rows[0] : null;
                return result;
            }
        }
    public Dictionary<string, object>[]? Rows { get; set; }

        public static DataverseResults GetSampleData()
        { 
            DataverseResults result = new DataverseResults();

            result.Columns = new string[] { 
                "accountid",
                "name"
            };

            result.Rows = new Dictionary<string, object>[2];
            result.Rows[0] = new Dictionary<string, object>();
            result.Rows[0]["accountid"] = Guid.NewGuid();
            result.Rows[0]["name"] = "account 1";

            result.Rows[1] = new Dictionary<string, object>();
            result.Rows[1]["accountid"] = Guid.NewGuid();
            result.Rows[1]["name"] = "account 2";

            return result;
        }
    }
}
