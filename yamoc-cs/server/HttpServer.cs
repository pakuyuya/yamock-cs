using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;
using httpmock.stringEvaluator;
using httpmock.stringEvaluator.decoder;

namespace httpmock.server
{
    public class HttpServer
    {
        static Logging log = Logging.getLogger();

        YamlSettings settings;

        public HttpServer(YamlSettings settings)
        {
            this.settings = settings;
        }

        public void Run()
        {
            var idx = 0;
            try
            {
                string listenat = "http://localhost:" + (settings.port ?? "80") + "/";
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(listenat);
                listener.Start();

                log.info("");
                log.info("   HTTP started   " + listenat);
                log.info("   Ctrl + C to stop   ");
                log.info("");

                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    Task task = execContextAsync(context, ++idx);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        Task execContextAsync(HttpListenerContext context, int idx)
        {
            return Task.Run(() =>
            {
                try
                {
                    var prefix = string.Format("{0:000}: ", idx);
                    var req = context.Request;
                    var requestProxy = new RequestProxy(context);

                    log.info(prefix + "--------------------------------------------------------------------------------");

                    log.info(prefix + DateTime.Now, ConsoleColor.DarkGray);
                    log.info(prefix + "===== Request =====", ConsoleColor.DarkGreen);
                    log.info(prefix + req.HttpMethod + " " + req.Url.LocalPath, ConsoleColor.White);
                    log.headers(prefix, "[Headers] ", req.Headers);
                    if (req.HasEntityBody)
                    {
                        log.formatted(prefix, "[Body   ] ", requestProxy.getBody());
                    }

                    log.info(prefix + "===== Response =====", ConsoleColor.DarkGreen);

                    var pathInfo = findPath(requestProxy, context);
                    if (!pathInfo.HasValue)
                    {
                        pathInfo = new YamlPathInfo{
                            response = (YamlResponseInfo) settings.defaultResponse
                        };
                    }
                    var p = pathInfo.Value;

                    Task taskWait = Task.Delay(p.wait > 0 ? p.wait : 0);

                    if (p.command != null)
                    {
                        log.info(prefix + "command: " + p.command);

                        List<string> commands = ShellParser.split(p.command);
                        var process = new Process();
                        var startInfo = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = commands[0],
                            Arguments = string.Join(" ", commands.GetRange(1, commands.Count - 1))
                        };
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                    }

                    int statudCode = 200;
                    var respHeaders = new Dictionary<string, string>();
                    var yamlres = p.response;
                    statudCode = int.Parse(yamlres.status ?? "200");
                    if (yamlres.headers != null)
                    {
                        foreach (var e in yamlres.headers)
                        {
                            respHeaders[e.Key] = e.Value;
                        }
                    }
                    string body = "";
                    body += yamlres.bodytext ?? "";
                    body += readFile(yamlres.bodyfile) ?? "";

                    taskWait.Wait();

                    writeResponse(context, prefix, statudCode, body, respHeaders);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("message: " + ex.Message);
                    Console.WriteLine("stack trace: ");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine();
                }
            });
        }

        YamlPathInfo? findPath(RequestProxy requestProxy, HttpListenerContext httpContext)
        {
            var request = httpContext.Request;
            string path = request.Url.LocalPath;
            string method = request.HttpMethod.ToLower();

            foreach (var p in settings.paths)
            {
                if (p.path != path) {
                    continue;
                }
                if (!string.IsNullOrEmpty(p.methods) && !p.methods.ToLower().Split(',').Contains(method))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(p.filter)) {
                    try {
                        IStringEvaluator evoluator = EvaluatorDecoder.decode(p.filter);
                        var context = new RequestMatchingContext();
                        context.PathInfo = p;
                        context.Request = requestProxy;
                        context.HttpContext = httpContext;
                        if (evoluator.evoluate(context) != StringEvaluator.TRUE) {
                            continue;
                        }
                    } catch (FormatException ex) {
                        log.error("ERROR! filter syntax is broken. filter: " + p.filter + " reason:" + ex.ToString());
                    }
                }
                return p;
            }
            return null;
        }

        void writeResponse(HttpListenerContext context, string prefix, int statudCode, string body, Dictionary<string, string> headers = null)
        {
            HttpListenerResponse res = context.Response;
            res.StatusCode = statudCode;
            log.formatted(prefix, "[StatusCode] ", statudCode.ToString());
            if (headers != null)
            {
                foreach (var e in headers)
                {
                    res.Headers.Add(e.Key, e.Value);
                }
                log.headers(prefix, "[Headers   ] ", res.Headers);
            }
            if (body != null)
            {
                byte[] content = Encoding.UTF8.GetBytes(body);
                res.OutputStream.Write(content, 0, content.Length);
                log.formatted(prefix, "[Body      ] ", body);
            }
            log.info(prefix + DateTime.Now, ConsoleColor.DarkGray);
            res.Close();
        }

        string readFile(string path)
        {
            if (path == null) {
                return null;
            }

            if (!File.Exists(path))
            {
                log.error(string.Format("{0} is not exists. responsing with no body.", path));
                return null;
            }

            using (var input = new StreamReader(path, Encoding.UTF8))
            {
                return input.ReadToEnd();
            }
        }
    }

}
