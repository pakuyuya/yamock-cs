using yamoc.server;

namespace yamoc.stringEvaluator {
    public abstract class OperatorEvaluator : IStringEvaluator {
        public IStringEvaluator Left { get; set; }
        public IStringEvaluator Right { get; set; }

        abstract public string evoluate(RequestMatchingContext context);

        abstract public int getPriority();

        public override string ToString() {
            return "{" + (Left ?? null) + ", " + (Right ?? null) + "}";
        }

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            if (obj is OperatorEvaluator) {
                OperatorEvaluator ope = (OperatorEvaluator)obj;
                if (Left == null && ope.Left != null) {
                    return false;
                } 
                if (Left != null && ope.Left == null) {
                    return false;
                } 
                if (Right == null && ope.Right != null) {
                    return false;
                } 
                if (Right != null && ope.Right == null) {
                    return false;
                } 
                return ((Left == null && ope.Left == null) || Left.Equals(ope.Left)) && ((Right == null && ope.Right == null) || Right.Equals(ope.Right));
            }
            return false;
        }
    
        public override int GetHashCode() {
            return (Left != null ? Left.GetHashCode() : 0) ^ (Right != null ? Right.GetHashCode() : 0);
        }
    }
}