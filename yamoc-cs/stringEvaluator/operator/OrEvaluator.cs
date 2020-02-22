using yamoc.server;

namespace yamoc.stringEvaluator.oparator
{
    public class OrEvaluator : OperatorEvaluator
    {

        override public string evoluate(RequestMatchingContext context)
        {
            if (Left.evoluate(context) == StringEvaluator.TRUE) {
                return StringEvaluator.TRUE;
            }
            if (Right.evoluate(context) == StringEvaluator.TRUE) {
                return StringEvaluator.TRUE;
            }
            return StringEvaluator.FALSE;
        }
        
        override public int getPriority() {
            return 12;
        }
    }
}
