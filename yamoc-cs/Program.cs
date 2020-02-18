using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using httpmock.server;

namespace httpmock
{
    class Program
    {
        static Logging log = Logging.getLogger();

        static void Main()
        {
            var version = "1.0.0";
            log.info("httpmock version " + version);

            var args = Environment.GetCommandLineArgs();
            var yamlPath = args.Length > 1 ? args[1] : "setting.yaml";

            YamlSettings? settings = YamlSettingsUtil.loadYaml(yamlPath);

            if (!settings.HasValue)
            {
                return;
            }
            log.info("");
            log.info("   using `" + yamlPath + "`");

            new HttpServer(settings.Value).Run();
        }
    }

}
