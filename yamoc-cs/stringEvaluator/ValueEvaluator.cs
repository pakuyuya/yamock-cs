using yamoc.server;

namespace yamoc.stringEvaluator {
    public abstract class ValueEvaluator : IStringEvaluator {
        abstract public string evoluate(RequestMatchingContext context);
    }
}