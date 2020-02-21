using httpmock.server;

namespace httpmock.stringEvaluator.value
{
    public class BodyJsonValueEvaluator : ValueEvaluator {
        public string Path { get; set; }

        override public string evoluate(RequestMatchingContext context)
        {
            string value = context.Request.readBodyJson(Path);
            return value;
        }

        public override string ToString() {
            return "{" + (Path ?? null) + "}";
        }

        public override bool Equals(object obj) {
            if (obj is BodyJsonValueEvaluator) {
                var val = (BodyJsonValueEvaluator)obj;
                return Path == val.Path;
            }
            return false;
        }
        
        public override int GetHashCode() {
            return (Path != null ? Path.GetHashCode() : 0);
        }
    }
}
