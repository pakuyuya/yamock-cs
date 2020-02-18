using httpmock.requestmatcher;


namespace httpmock.stringEvaluator.value
{
    public class BodyJsonValueEvaluator : ValueEvaluator {
        public string Path { get; set; }

        override public string evoluate(FilterContext context)
        {
            // TODO:
            return StringEvaluator.FALSE;
        }

        public override string ToString() {
            return "{" + (Path ?? null) + "}";
        }

        public override bool Equals(object? obj) {
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
