
using System;
using System.Text.RegularExpressions;
using yamoc.stringEvaluator.value;

namespace yamoc.stringEvaluator.decoder.token
{
    public class BodyJsonToken : IToken {

        public int getTokenEndIndex(string s) {
            var match = Regex.Match(s, @"^\s*bodyjson.[\[\].a-zA-Z0-9-_]+");
            return match.Success ? match.Length : -1;
        }

        public void setToken(string s) {
            this.Path = s.Trim().Substring(9);
        }

        public string Path { get; set; }

        public override string ToString() {
            return Path;
        }

        public static IToken[] followableTokens = TokenHelper.operatorTokens();

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return  new BodyJsonToken();
        }
        
        public Type getCombinePair() {
            return null;
        }

        public IStringEvaluator getEvaluator() {
            BodyJsonValueEvaluator evaluator = new BodyJsonValueEvaluator();
            evaluator.Path = Path;
            return evaluator;
        }
    }
}