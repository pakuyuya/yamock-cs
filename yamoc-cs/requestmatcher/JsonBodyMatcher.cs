﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml;

namespace httpmock.requestmatcher
{
    public class JsonBodyMatcher : IRequestMatcher
    {
        public string Expression { get; set; }

        public string Path { get; set; }
        public string ValueExpression { get; set; }

        public bool match(FilterContext context)
        {
            if (context.requestBody == null) {
                return false;
            }
            XmlDictionaryReader xmlReader =JsonReaderWriterFactory.CreateJsonReader(
                Encoding.UTF8.GetBytes(context.requestBody), XmlDictionaryReaderQuotas.Max);

            return false;
        }
    }
}