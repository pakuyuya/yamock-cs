
using System;
using System.Text.RegularExpressions;
using httpmock.stringEvaluator.oparator;
using httpmock.stringEvaluator.value;

namespace httpmock.stringEvaluator.decoder.token
{
    public class DoubleQuotedValueToken : IToken {
        public int getTokenEndIndex(string s) {
            int i = 0;

            while (i < s.Length && Char.IsWhiteSpace(s[i])) {
                i++;
            }

            if (i >= s.Length || s[i] != '"') {
                return -1;
            }
            i++;

            for (; i < s.Length; i++) {
                var c = s[i];
                if (c == '\\') {
                    ++i;
                    continue;
                }
                if (c == '"') {
                    break;
                }
            }

            return Math.Min(i + 1, s.Length);
        }

        public void setToken(string s) {
            Value = s.Trim().Trim('"');
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