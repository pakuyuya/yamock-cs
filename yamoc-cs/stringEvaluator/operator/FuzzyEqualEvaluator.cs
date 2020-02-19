﻿using httpmock.server;

namespace httpmock.stringEvaluator.oparator
{
    public class FuzzyEqualEvaluator : OperatorEvaluator
    {
        override public string evoluate(RequestMatchingContext context)
        {
            return (Left.evoluate(context) == Right.evoluate(context))
                ? StringEvaluator.TRUE
                : StringEvaluator.FALSE;
        }

        override public int getPriority() {
            return 7;
        }
    }
}
