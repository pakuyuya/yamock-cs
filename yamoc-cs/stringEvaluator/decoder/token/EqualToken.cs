
using System;
using System.Text.RegularExpressions;
using yamoc.stringEvaluator.oparator;
using yamoc.stringEvaluator.value;

namespace yamoc.stringEvaluator.decoder.token
{
    public class EqualToken : IToken {
        public int getTokenEndIndex(string s) {
            var match = Regex.Match(s, @"^\s*==");
            return match.Success ? match.Length : -1;
        }

        public void setToken(string s) {
            // do nothing
        }

        public static IToken[] followableTokens = TokenHelper.valueTokens();

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return new EqualToken();
        }
        public Type getCombinePair() {
            return null;
        }

        public IStringEvaluator getEvaluator() {
            FuzzyEqualEvaluator evaluator = new FuzzyEqualEvaluator();
            return evaluator;
        }
    }
}