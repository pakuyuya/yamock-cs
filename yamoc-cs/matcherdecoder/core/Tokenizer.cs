
using System;
using System.Text.RegularExpressions;

namespace httpmock.matcherdecoder.core
{
    public interface IToken {
        int getTokenEndIndex(string s);

        void setToken(string s);

        IToken[] getFollowableTokens();

        IToken newToken();
    }

    public class RootToken : IToken {
        public int getTokenEndIndex(string s) {
            return 0;
        }

        public void setToken(string s) {
            // do nothing
        }

        public static IToken[] followableTokens = new IToken[]{
            new NameToken()
        };

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return new RootToken();
        }
    }
    public class NameToken : IToken {

        public int getTokenEndIndex(string s) {
            var match = Regex.Match(s, @"^\s*[a-zA-Z0-9-_]+");
            return match.Success ? match.Length : -1;
        }

        public void setToken(string s) {
            this.Name = s.Trim();
        }

        public string Name { get; set; }

        public static IToken[] followableTokens = new IToken[]{
            new EqualToken()
        };

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return new NameToken();
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

        public static IToken[] followableTokens = new IToken[]{
            new DoubleQuotedValueToken()
        };

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return new EqualToken();
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

            return Math.Min(i, s.Length);
        }

        public void setToken(string s) {
            Value = s.Trim().Trim('"');
        }

        public string Value { get; set; }

        public static IToken[] followableTokens = new IToken[]{
        };

        public IToken[] getFollowableTokens() {
            return followableTokens;
        }

        public IToken newToken() {
            return new DoubleQuotedValueToken();
        }
    }
}