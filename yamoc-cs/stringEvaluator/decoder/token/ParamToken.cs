
using System;
using System.Text.RegularExpressions;
using yamoc.stringEvaluator.value;

namespace yamoc.stringEvaluator.decoder.token
{
    public class ParamToken : IToken {

        public int getTokenEndIndex(string s) {
            var match = Regex.Match(s, @"^\s*param.[\[\].a-zA-Z0-9-_]+");
            return match.Success ? match.Length : -1;
        }

        public void setToken(string s) {
            this.Param = s.Trim().Substring(6);
        }

        public string Param { get; set; }

        public override string ToString() {
            return Param;
        }

        public static IToken[] followableTokens = TokenHelper.operatorTokens();

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return  new ParamToken();
        }
        
        public Type getCombinePair() {
            return null;
        }

        public IStringEvaluator getEvaluator() {
            ParamValueEvaluator evaluator = new ParamValueEvaluator();
            evaluator.Param = Param;
            return evaluator;
        }
    }
}