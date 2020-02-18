using httpmock.requestmatcher;

namespace httpmock.stringEvaluator.oparator
{
    public class AndEvaluator : OperatorEvaluator
    {
        override public string evoluate(FilterContext context)
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
