using System.Net;

namespace httpmock.server {
    public class RequestMatchingContext
    {
        public YamlPathInfo PathInfo { get; set; }
        public HttpListenerContext HttpContext { get; set; }
        public RequestProxy Request { get; set; }
    }
}