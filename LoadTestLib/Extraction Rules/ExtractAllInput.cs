using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Xml.XPath;

namespace CorePlugins
{
    public class ExtractAllInput : ExtractionRule
    {
        public override string RuleName => "Extract All Inputs";
        public override string RuleDescription => "Extract All Inputs and Selects from the form";

        private bool _Cumulative;
        public bool Cumulative
        {
            get { return _Cumulative; }
            set { _Cumulative = value; }
        }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            Dictionary<string, string> Inputs = new Dictionary<string, string>();

            if (_Cumulative)
            {
                if (e.WebTest.Context.ContainsKey(this.ContextParameterName))
                {
                    int currentSize = int.Parse(e.WebTest.Context[this.ContextParameterName].ToString());
                    for (int j = 1; j <= currentSize; j++)
                    {
                        string n = e.WebTest.Context[this.ContextParameterName + "_NAME_" + j].ToString();
                        string v = e.WebTest.Context[this.ContextParameterName + "_VALUE_" + j].ToString();
                        Inputs.Add(n, v);
                    }
                }
            }
            Clear(e);

            Regex rx = new Regex("<input([^>]+)name=\\\\?\"([^>]+?)\\\\?\"([^>]+)");
            MatchCollection mcInputs = rx.Matches(e.Response.BodyString);

            List<string> excludedTypes = new List<string>();
            excludedTypes.Add("submit");

            foreach (Match inp in mcInputs)
            {
                string name = inp.Groups[2].Value;
                string type = "";
                string value = "";

                rx = new Regex("type=\\\\?\"(.*?)\\\\?\"");
                Match m = rx.Match(inp.Groups[1].Value);
                if (!m.Success) m = rx.Match(inp.Groups[3].Value);
                if (m.Success)
                {
                    type = m.Groups[1].Value;
                }

                if (excludedTypes.Contains(type))
                {
                    continue;
                }

                rx = new Regex("value=\\\\?\"(.*?)\\\\?\"");
                m = rx.Match(inp.Groups[1].Value);
                if (!m.Success) m = rx.Match(inp.Groups[3].Value);
                if (m.Success)
                {
                    value = m.Groups[1].Value;
                }

                if (type.Equals("radio"))
                {
                    rx = new Regex("checked=\\\\?\"checked\\\\?\"");
                    m = rx.Match(inp.Groups[3].Value);
                    if (!m.Success)
                    {
                        continue;
                    }
                }
                else if (type.Equals("checkbox"))
                {
                    rx = new Regex("checked=\\\\?\"checked\\\\?\"");
                    m = rx.Match(inp.Groups[3].Value);
                    if (m.Success)
                    {
                        value = "on";
                    }
                    else
                    {
                        value = "off";
                    }
                }

                if (Inputs.ContainsKey(name))
                {
                    Inputs[name] = value;
                }
                else
                {
                    Inputs.Add(name, value);
                }
            }

            rx = new Regex("<select[^>]+name=\\\\?\"([^>]+?)\\\\?\"[^>]+>([\\s\\S]+?)<\\\\?\\/select>");
            MatchCollection mcSelects = rx.Matches(e.Response.BodyString);

            foreach (Match s in mcSelects)
            {
                string name = s.Groups[1].Value;
                string value = "";

                rx = new Regex("<option[^>]+selected[^>]+value=\\\\?\"(.+?)\\\\?\"");
                Match m = rx.Match(s.Groups[2].Value);
                if (!m.Success)
                {
                    rx = new Regex("<option[^>]+value=\\\\?\"(.+?)\\\\?\"");
                    m = rx.Match(s.Groups[2].Value);
                }
                if (m.Success)
                {
                    value = m.Groups[1].Value;
                }

                if (Inputs.ContainsKey(name))
                {
                    Inputs[name] = value;
                }
                else
                {
                    Inputs.Add(name, value);
                }
            }

            e.WebTest.Context.Add(this.ContextParameterName, Inputs.Count);

            int i = 1;
            foreach (string key in Inputs.Keys)
            {
                e.WebTest.Context.Add(this.ContextParameterName + "_NAME_" + i, key);
                e.WebTest.Context.Add(this.ContextParameterName + "_VALUE_" + i, Inputs[key]);
                i++;
            }
        }

        private void Clear(ExtractionEventArgs e)
        {
            int currentSize = 0;
            if (e.WebTest.Context.ContainsKey(this.ContextParameterName))
            {
                currentSize = int.Parse(e.WebTest.Context[this.ContextParameterName].ToString());
            }

            for (int i = 1; i <= currentSize; i++)
            {
                e.WebTest.Context.Remove(this.ContextParameterName + "_NAME_" + i);
                e.WebTest.Context.Remove(this.ContextParameterName + "_VALUE_" + i);
            }

            e.WebTest.Context.Remove(this.ContextParameterName);
        }
    }
}
