using System.Net;
using System.IO;

namespace httpmock.server {
    public class RequestResolver {
        public HttpListenerContext HttpContext { get; }

        private string body = null;

        public RequestResolver(HttpListenerContext httpContext) {
            HttpContext = httpContext;
        }

        public string getBody() {
            var req = HttpContext.Request;
            if (body == null) {
                using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                {
                    body = reader.ReadToEnd();
                }
            }
            return body;
        }

        public string getBodyJson(string key) {
            // TODO:
            return null;   
        }
    }
}