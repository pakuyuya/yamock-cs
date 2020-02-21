
namespace httpmock.stringEvaluator.decoder.token
{
    class TokenHelper {
        public static IToken[] operatorTokens () {
            return new IToken[]{
                new EqualToken()
            };
        }
        public static IToken[] valueTokens () {
            return new IToken[]{
                new BodyJsonToken(),
                new DoubleQuotedValueToken(),
                new ValueToken()
            };
        }
    }
}