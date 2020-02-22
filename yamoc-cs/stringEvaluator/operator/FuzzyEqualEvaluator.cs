using yamoc.server;

namespace yamoc.stringEvaluator.oparator
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
