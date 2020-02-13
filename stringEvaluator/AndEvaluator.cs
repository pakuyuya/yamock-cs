using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpmock.stringEvaluator
{
    public class AndEvaluator : IStringEvoluator
    {
        public IStringEvoluator Left { get; set; }
        public IStringEvoluator Right { get; set; }

        public bool evoluate(string value)
        {
            return Left.evoluate(value) && Right.evoluate(value);
        }
    }
}
