using httpmock.requestmatcher;


namespace httpmock.stringEvaluator.oparator
{
    public class FuzzyEqualEvaluator : OperatorEvaluator
    {
        override public string evoluate(FilterContext context)
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
