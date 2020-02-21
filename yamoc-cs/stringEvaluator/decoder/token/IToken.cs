
using System;
using System.Text.RegularExpressions;
using httpmock.stringEvaluator;
using httpmock.stringEvaluator.oparator;
using httpmock.stringEvaluator.value;

namespace httpmock.stringEvaluator.decoder.token
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