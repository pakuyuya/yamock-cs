using httpmock.requestmatcher;

namespace httpmock.stringEvaluator {
    public abstract class ValueEvaluator : IStringEvaluator {
        abstract public string evoluate(FilterContext context);
    }
}