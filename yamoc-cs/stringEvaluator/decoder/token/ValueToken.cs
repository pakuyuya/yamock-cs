
using System;
using System.Text.RegularExpressions;
using httpmock.stringEvaluator.oparator;
using httpmock.stringEvaluator.value;

namespace httpmock.stringEvaluator.decoder.token
{
    public class ValueToken : IToken {
        public int getTokenEndIndex(string s) {
            var match = Regex.Match(s, @"^\s*\S+");
            return match.Success ? match.Length : -1;
        }

        public void setToken(string s) {
            Value = s.Trim();
        }

        public string Value { get; set; }

        public static IToken[] followableTokens = TokenHelper.operatorTokens();
        
        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return new DoubleQuotedValueToken();
        }

        public Type getCombinePair() {
            return null;
        }

        public IStringEvaluator getEvaluator() {
            LiteralValueEvaluator evaluator = new LiteralValueEvaluator();
            evaluator.Value = this.Value;
            return evaluator;
        }
    }

}