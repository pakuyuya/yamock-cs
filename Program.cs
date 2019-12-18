using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace yamledhttp
{
    class Program
    {
        static void Main(string[] args)
        {
            var yamlPath = args.Length > 1 ? args[1] : "setting.yaml";
            var idx = 0;

            if (!loadYaml(yamlPath)) {
                return;
            }

            try {
                string listenat = "http://localhost:" + (settings.port ?? "200") + "/";
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(listenat);
                listener.Start();

                log("");
                log("   HTTP started   " + listenat);
                log("");

                while (true){
                    HttpListenerContext context = listener.GetContext();
                    Task task = execContextAsync(context, ++idx);
                }

            } catch (Exception)
            {
                throw;
            }
        }

        static Task execContextAsync(HttpListenerContext context, int idx) {
            return Task.Run(() => {
                try {
                    var prefix = string.Format("{0:000000}:", idx);
                    var req = context.Request;

                    log(prefix + "--------------------------------------------------------------------------------");

                    log(prefix + DateTime.Now);
                    log(prefix + "===== Request =====", ConsoleColor.Yellow);
                    log(prefix + "[" + req.HttpMethod + "] " + req.Url.LocalPath);
                    log(prefix + "[Headers] " + stringify(req.Headers));
                    if (req.HasEntityBody) {
                        using(var reader = new StreamReader(req.InputStream, req.ContentEncoding)) {
                            log(prefix + "[Body]" + reader.ReadToEnd());
                        }
                    }

                    log(prefix + "===== Response =====", ConsoleColor.Yellow);
                    
                    var pathInfo = findPath(req.Url.LocalPath, req.HttpMethod);
                    if (!pathInfo.HasValue) {
                        var headers = new Dictionary<string, string>();
                        headers.Add("ContetType", "application/json");
                        writeResponse(context, prefix, 404, "{\"message\":\"404: not found\"}");
                        return;
                    }

                    var p = pathInfo.Value;

                    if (p.command != null) {
                        List<string> commands = ShellParser.split(p.command);
                        var psi = new System.Diagnostics.ProcessStartInfo();
                        psi.FileName = commands[0];
                        psi.Arguments = string.Join(" ",commands.GetRange(1, commands.Count - 1));
                        System.Diagnostics.Process.Start(psi).WaitForExit();
                    }

                    int statudCode = 200;
                    var respHeaders = new Dictionary<string, string>();
                    respHeaders["ContentType"] = "application/json";
                    string body = null;
                    var yamlres = p.response;
                    statudCode = int.Parse(yamlres.status ?? "200");
                    if (yamlres.headers != null) {
                        foreach (var e in yamlres.headers) {
                            respHeaders[e.Key] = e.Value;
                        }
                    }
                    body = (yamlres.bodyfile != null) ? readFile(yamlres.bodyfile) : null;

                    writeResponse(context, prefix, statudCode, body, respHeaders);
                } catch (Exception ex) {
                    Console.WriteLine("message: " + ex.Message);
                    Console.WriteLine("stack trace: ");
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine();
                }
            });
        }
        static YamlPathInfo? findPath(string path, string method) {
            method = method.ToLower();
            foreach (var p in settings.paths) {
                if(p.path == path && (string.IsNullOrEmpty(p.methods) || p.methods.ToLower().Split(",").Contains(method))) {
                    return p;
                }
            }
            return null;
        }
        static void writeResponse(HttpListenerContext context, string prefix, int statudCode, string body, Dictionary<string, string> headers = null) {
            HttpListenerResponse res = context.Response;
            res.StatusCode = statudCode;
            log(prefix + "[StatusCode] " + statudCode);
            if (headers != null) {
                foreach (var e in headers) {
                    res.Headers.Add(e.Key, e.Value);
                }
                log(prefix + "[Headers] " + stringify(res.Headers));
            }
            if (body != null) {
                byte[] content = Encoding.UTF8.GetBytes(body);
                res.OutputStream.Write(content, 0, content.Length);
                log(prefix + "[Body] " + body);
            }
            log(prefix + DateTime.Now);
            res.Close();
        }

        static string readFile(string path) {
            if (!File.Exists(path)) {
                logError(string.Format("{0} is not exists.", path));
                return null;
            }

            using (var input = new StreamReader(path, Encoding.UTF8)) {
                return input.ReadToEnd();
            }
        }

        static bool loadYaml(string path) {
            if (!File.Exists(path)) {
                logError(string.Format("{0} is not exists.", path));
                return false;
            }
            using(var input = new StreamReader(path, Encoding.UTF8)) {
                var deserializer = new DeserializerBuilder()
                    .Build();
                settings = deserializer.Deserialize<YamlSettings>(input);
            }

            return true;
        }

        static void log(string msg, ConsoleColor? color = null) {
            if (color.HasValue) {
                Console.ForegroundColor = color.Value;
            }
            Console.WriteLine("{0}", msg);
            Console.ResetColor();
        }

        static void logError(string msg) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] {0}", msg);
            Console.ResetColor();
        }

        static string stringify(NameValueCollection collection) {
            var sb = new StringBuilder();
            foreach (string key in collection) {
                sb.Append(key +":" + collection[key] + ", ");
            }
            return sb.ToString();
        }


        static YamlSettings settings;
    }
    
    public struct YamlSettings {
        public string port;
        public List<YamlPathInfo> paths;
    }
    public struct YamlPathInfo {
        public string path;
        public string methods;
        public string command;
        public YamlResponseInfo response;
    }
    public struct YamlResponseInfo {
        public string status;
        public Dictionary<string, string> headers;
        public string bodyfile;
    }
}
