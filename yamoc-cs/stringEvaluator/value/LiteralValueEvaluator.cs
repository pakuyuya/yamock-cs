using httpmock.server;

namespace httpmock.stringEvaluator.value
{
    public class LiteralValueEvaluator : ValueEvaluator {
        public string Value { get; set; }

        override public string evoluate(RequestMatchingContext context)
        {
            return Value;
        }
        
        public override string ToString() {
            return "{" + (Value ?? null) + "}";
        }

        public override bool Equals(object obj) {
            if (obj is LiteralValueEvaluator) {
                var val = (LiteralValueEvaluator)obj;
                return Value == val.Value;
            }
            return false;
        }
        
        public override int GetHashCode() {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
