using yamoc.server;

namespace yamoc.stringEvaluator.oparator
{
    public class AndEvaluator : OperatorEvaluator
    {
        override public string evoluate(RequestMatchingContext context)
        {
            if (Left.evoluate(context) != StringEvaluator.TRUE) {
                return StringEvaluator.FALSE;
            }
            if (Right.evoluate(context) != StringEvaluator.TRUE) {
                return StringEvaluator.FALSE;
            }
            return StringEvaluator.TRUE;
        }

        override public int getPriority() {
            return 11;
        }
    }
}
