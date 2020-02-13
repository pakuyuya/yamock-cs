using System;
using System.Collections.Generic;

namespace httpmock
{
    public static class ShellParser {
        public static List<string> split(string command) {
            if (command == null) {
                throw new NullReferenceException();
            }

            List<string> list = new List<string>();

            string s = command;
            while (s != "") {
                var token = nextToken(s);
                list.Add(token);
                s = s.Substring(token.Length).Trim();
            }

            return list;
        }

        static string nextToken(string s) {
            bool quoted = false;
            bool escaped = false;
            int i = 0, l = s.Length;
            for (i = 0; i<l; i++) {
                var c = s[i];
                if (char.IsWhiteSpace(c) && !quoted) {
                    break;
                }
                if (c == '"' && !escaped) {
                    quoted ^= true;
                }
                escaped = (!escaped) && c == '\\';
            }

            return s.Substring(0, i);
        }
    }
}
