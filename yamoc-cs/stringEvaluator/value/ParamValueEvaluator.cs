using httpmock.server;

namespace httpmock.stringEvaluator.value
{
    public class ParamValueEvaluator : ValueEvaluator {
        public string Param { get; set; }

        override public string evoluate(RequestMatchingContext context)
        {
            return context.Request.getQueryParameter(Param);
        }

        public override string ToString() {
            return "{" + (Param ?? null) + "}";
        }

        public override bool Equals(object obj) {
            if (obj is BodyJsonValueEvaluator) {
                var val = (BodyJsonValueEvaluator)obj;
                return Param == val.Path;
            }
            return false;
        }
        
        public override int GetHashCode() {
            return (Param != null ? Param.GetHashCode() : 0);
        }
    }
}
