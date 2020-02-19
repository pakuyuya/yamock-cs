using System.Net;

namespace httpmock.server {
    public struct RequestMatchingContext
    {
        public YamlPathInfo pathInfo;
        public HttpListenerContext httpContext;
        public string requestBody;
    }
}