using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace httpmock.requestmatcher
{
    public interface IRequestMatcher
    {
        bool match(FilterContext context);
    }

    public struct FilterContext
    {
        public YamlPathInfo pathIUnfo;
        public HttpListenerContext httpContext;
        public string requestBody;
    }
}
