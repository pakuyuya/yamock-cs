
using System;
using System.Text.RegularExpressions;
using httpmock.stringEvaluator;
using httpmock.stringEvaluator.oparator;
using httpmock.stringEvaluator.value;

namespace httpmock.matcherdecoder.core
{
    public interface IToken {
        int getTokenEndIndex(string s);

        void setToken(string s);

        IToken[] getFollowableTokens();

        IToken newToken();
        
        Type getCombinePair();

        IStringEvaluator getEvaluator();
    }

    public class RootToken : IToken {
        public int getTokenEndIndex(string s) {
            return 0;
        }

        public void setToken(string s) {
            // do nothing
        }

        public static IToken[] followableTokens = TokenHelper.valueTokens();

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return new RootToken();
        }
        
        public Type getCombinePair() {
            return null;
        }

        public IStringEvaluator getEvaluator() {
            throw new Exception("this code is not run");
        }
    }

    public class BodyJsonToken : IToken {

        public int getTokenEndIndex(string s) {
            var match = Regex.Match(s, @"^\s*bodyjson.[\[\].a-zA-Z0-9-_]+");
            return match.Success ? match.Length : -1;
        }

        public void setToken(string s) {
            this.Path = s.Trim();
        }

        public string Path { get; set; }

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