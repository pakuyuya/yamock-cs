
using System;
using System.Text;
using System.Collections.Specialized;

namespace yamoc
{
    public class Logging
    {

        public static Logging getLogger() {
            return new Logging();
        }

        private ConsoleColor _SectionColor = ConsoleColor.Blue;
        private ConsoleColor _KeywordColor = ConsoleColor.Yellow;
        public ConsoleColor SectionColor { get { return _SectionColor; } set { _SectionColor = value; } }
        public ConsoleColor KeywordColor { get { return _KeywordColor; } set { _KeywordColor = value; } }

        public void info(string msg, ConsoleColor? color = null) {
            if (color.HasValue) {
                Console.ForegroundColor = color.Value;
            }
            Console.WriteLine("{0}", msg);
            Console.ResetColor();
        }
        public void formatted(string prefix, string section, string msg) {
            Console.Write(prefix);
            Console.ForegroundColor = SectionColor;
            Console.Write(section);
            Console.ResetColor();
            Console.WriteLine(msg);
        }
        public void headers(string prefix, string section, NameValueCollection collection) {
            var sb = new StringBuilder();
            
            Console.Write(prefix);
            Console.ForegroundColor = SectionColor;
            Console.Write(section);
            Console.ResetColor();
            foreach (string key in collection) {
                Console.ForegroundColor = KeywordColor;
                Console.Write(key + ": ");
                Console.ResetColor();
                Console.Write(collection[key] + ", ");
            }
            Console.WriteLine("");
        }

        public void error(string msg) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] {0}", msg);
            Console.ResetColor();
        }
    }
}
