using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace httpmock.server {
    public class RequestProxy {
        public HttpListenerContext HttpContext { get; }

        private string bodyCache = null;
        private JObject jobjectCache = null;

        public RequestProxy(HttpListenerContext httpContext) {
            HttpContext = httpContext;
        }

        public string getBody() {
            var req = HttpContext.Request;
            if (bodyCache == null) {
                using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                {
                    bodyCache = reader.ReadToEnd();
                }
            }
            return bodyCache;
        }

        public JObject getJObject() {
            if (jobjectCache == null) {
                jobjectCache = JObject.Parse(getBody());
            }
            return jobjectCache;
        }

        public string getBodyJson(string key) {
            return (string) getJObject().SelectToken(key);
        }
    }
}