using System.IO;
using System.Text;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace httpmock
{
    public static class YamlSettingsUtil
    {
        static Logging log = Logging.getLogger();

        public static YamlSettings? loadYaml(string path) {
            if (!File.Exists(path)) {
                log.error(string.Format("{0} is not exists.", path));
                return null;
            }
            using(var input = new StreamReader(path, Encoding.UTF8)) {
                var deserializer = new DeserializerBuilder()
                    .Build();
                return deserializer.Deserialize<YamlSettings>(input);
            }
        }
    }

    public struct YamlSettings {
        public string port;
        public List<YamlPathInfo> paths;
    }
    public struct YamlPathInfo {
        public string path;
        public string methods;
        public string command;
        public string filter;
        public YamlResponseInfo response;
    }
    public struct YamlResponseInfo {
        public string status;
        public Dictionary<string, string> headers;
        public string bodyfile;
    }
}
