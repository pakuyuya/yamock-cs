using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using httpmock.requestmatcher;
using httpmock.matcherdecoder.core;

namespace httpmock.matcherdecoder
{
    public class MatcherDecoder {
        public static IRequestMatcher decode(string syntax) {
            List<IToken> tokens = parseToTokens(syntax);

            return null;
        }

        private static List<IToken> parseToTokens(string syntax) {
            string s = syntax ?? "";
            var tokens = new List<IToken>();

            IToken rurrentToken = new RootToken();

            int iPos = 0;

            while (s.Length > 0) {
                IToken nextToken = null;
                foreach (var token in rurrentToken.getFollowableTokens()) {
                    int i = token.getTokenEndIndex(s);
                    if (i >= 0) {
                        nextToken = token.newToken();
                        nextToken.setToken(s);
                        s = s.Substring(0, i);
                        iPos += i;
                        break;
                    }
                }

                if (nextToken == null) {
                    throw new FormatException("Unknown token. position: " + iPos + ". Unkown token: `" + s + "`");
                }
                tokens.Add(nextToken);
            }

            return tokens;
        }
        
    }
}
