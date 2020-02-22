
using System;
using System.Text.RegularExpressions;
using yamoc.stringEvaluator.oparator;
using yamoc.stringEvaluator.value;

namespace yamoc.stringEvaluator.decoder.token
{
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
}