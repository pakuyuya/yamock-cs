using yamoc.server;

namespace yamoc.requestmatcher
{
    public interface IRequestMatcher
    {
        bool match(RequestMatchingContext context);
    }
}
