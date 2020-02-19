using httpmock.server;

namespace httpmock.requestmatcher
{
    public interface IRequestMatcher
    {
        bool match(RequestMatchingContext context);
    }
}
