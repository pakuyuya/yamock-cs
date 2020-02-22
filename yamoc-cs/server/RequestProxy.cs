using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace yamoc.server {
    public class RequestProxy {
        public HttpListenerContext HttpContext { get; }

        private string bodyCache = null;
        private JObject jobjectCache = null;

        public RequestProxy(HttpListenerContext httpContext) {
            HttpContext = httpContext;
        }

        public string getBody() {
            var req = HttpContext.Request;
            if (bodyCache == null && req.HasEntityBody) {
                using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                {
                    bodyCache = reader.ReadToEnd();
                }
            }
            return bodyCache;
        }

        public JObject getJObject() {
            if (jobjectCache == null && getBody() != null) {
                try {
                    jobjectCache = JObject.Parse(getBody());
                } catch (JsonReaderException) {
                    return null;
                }
            }
            return jobjectCache;
        }

        public string readBodyJson(string key) {
            JObject o = getJObject();
            if (o == null) {
                return null;
            }
            return (string) getJObject().SelectToken(key);
        }

        public string getQueryParameter(string param) {
            var req = HttpContext.Request;
            return req.QueryString.Get(param); 
        }
    }
}