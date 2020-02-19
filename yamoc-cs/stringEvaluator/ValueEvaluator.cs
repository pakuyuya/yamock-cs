using httpmock.server;

namespace httpmock.stringEvaluator {
    public abstract class ValueEvaluator : IStringEvaluator {
        abstract public string evoluate(RequestMatchingContext context);
    }
}