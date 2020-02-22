
using System;
using System.Text.RegularExpressions;
using yamoc.stringEvaluator;
using yamoc.stringEvaluator.oparator;
using yamoc.stringEvaluator.value;

namespace yamoc.stringEvaluator.decoder.token
{
    public interface IToken {
        int getTokenEndIndex(string s);

        void setToken(string s);

        IToken[] getFollowableTokens();

        IToken newToken();
        
        Type getCombinePair();

        IStringEvaluator getEvaluator();
    }
}