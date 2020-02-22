using yamoc.server;

namespace yamoc.stringEvaluator
{
    public interface IStringEvaluator
    {
       string evoluate(RequestMatchingContext context);
    }

    public class StringEvaluator {
        public static string TRUE = "true";
        public static string FALSE = "false";

        public static string TypeOperator = "ope";
        public static string TypeValue = "val";
        public static string TypePair = "p";
    }
}
