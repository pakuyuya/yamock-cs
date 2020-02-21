﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
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
                        var headers = new Dictionary<string, string>();
                        headers.Add("Content-Type", "application/json");
                        writeResponse(context, prefix, 404, "{\"message\":\"404: not found\"}", headers);
                        return;
                    }

                    var p = pathInfo.Value;

                    if (p.command != null)
                    {
                        List<string> commands = ShellParser.split(p.command);
                        var psi = new System.Diagnostics.ProcessStartInfo();
                        psi.FileName = commands[0];
                        psi.Arguments = string.Join(" ", commands.GetRange(1, commands.Count - 1));
                        System.Diagnostics.Process.Start(psi).WaitForExit();
                    }

                    int statudCode = 200;
                    var respHeaders = new Dictionary<string, string>();
                    respHeaders["Content-Type"] = "application/json";
                    string body = null;
                    var yamlres = p.response;
                    statudCode = int.Parse(yamlres.status ?? "200");
                    if (yamlres.headers != null)
                    {
                        foreach (var e in yamlres.headers)
                        {
                            respHeaders[e.Key] = e.Value;
                        }
                    }
                    body = (yamlres.bodyfile != null) ? readFile(yamlres.bodyfile) : null;

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
