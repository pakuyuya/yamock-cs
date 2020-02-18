using httpmock.requestmatcher;


namespace httpmock.stringEvaluator.oparator
{
    public class OrEvaluator : OperatorEvaluator
    {

        override public string evoluate(FilterContext context)
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
