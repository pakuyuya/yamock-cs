using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpmock.stringEvaluator
{
    public class FuzzyEqualEvaluator : IStringEvoluator
    {
        public string Value { get; set; }

        public bool evoluate(string value)
        {
            return string.Equals(value, this.Value);
        }
    }
}
