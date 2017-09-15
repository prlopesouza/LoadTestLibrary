using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace LoadTestLib.Extraction_Rules
{
    [DisplayName("Extract Random Regex")]
    [Description("Extrai um match aleatório retornado pela Regex, ou todos os matches se ExtractAll=True. Ao retornar todos os matches o nome das variáveis tera o formato 'Nome_{i}_g{j}', sendo {i} o índice da variável e {j} o índice do grupo de captura da Regex.")]
    public class ExtractRandomRegex : ExtractionRule
    {
        private string _Regex;
        [Description("A Regex utilizada para a busca.")]
        public string Regex
        {
            get { return _Regex; }
            set { _Regex = value; }
        }

        private bool _ExtractAll;
        [Description("Se True, todas as correspondências serão retornadas. Se False, será retornada uma correspondência aleatória.")]
        public bool ExtractAll
        {
            get { return _ExtractAll; }
            set { _ExtractAll = value; }
        }

        private bool _EscapeEspecial;
        [Description("Se True, os caracteres especiais na string da Regex serão 'escapados'. Ex: '.' para '\\.'")]
        public bool EscapeEspecial
        {
            get { return _EscapeEspecial; }
            set { _EscapeEspecial = value; }
        }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            string response = e.Response.BodyString;
            if (_EscapeEspecial)
            {
                _Regex = _Regex.Replace("|", "\\|");
                _Regex = _Regex.Replace(".", "\\.");
                _Regex = _Regex.Replace("\\", "\\\\");
                _Regex = _Regex.Replace("+", "\\+");
                _Regex = _Regex.Replace("*", "\\*");
                _Regex = _Regex.Replace("?", "\\?");
                _Regex = _Regex.Replace("^", "\\^");
                _Regex = _Regex.Replace("$", "\\$");
                _Regex = _Regex.Replace("[", "\\[");
                _Regex = _Regex.Replace("]", "\\]");
                _Regex = _Regex.Replace("{", "\\{");
                _Regex = _Regex.Replace("}", "\\}");
                _Regex = _Regex.Replace("(", "\\(");
                _Regex = _Regex.Replace(")", "\\)");
                _Regex = _Regex.Replace("/", "\\/");
            }

            Regex rx = new Regex(_Regex);

            MatchCollection matches = rx.Matches(response);

            if (matches.Count == 0)
            {
                if (_ExtractAll)
                {
                    e.WebTest.Context.Add(this.ContextParameterName + "_Nr", matches.Count);
                    e.WebTest.Context.Add(this.ContextParameterName + "_Gr", 0);
                }
                e.Success = true;
                return;
            }

            if (_ExtractAll)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    string name = this.ContextParameterName;
                    if (matches.Count > 1) name = name + "_" + (i + 1);
                    addContextParameters(matches[i], name, e);
                }
                e.WebTest.Context.Add(this.ContextParameterName + "_Nr", matches.Count);
                e.WebTest.Context.Add(this.ContextParameterName + "_Gr", matches[0].Groups.Count - 1);
            }
            else
            {
                Random rd = new Random();
                int rdMatch = rd.Next(matches.Count);
                addContextParameters(matches[rdMatch], this.ContextParameterName, e);
                e.WebTest.Context.Add(this.ContextParameterName + "_Nr", "1");
                e.WebTest.Context.Add(this.ContextParameterName + "_Gr", matches[rdMatch].Groups.Count - 1);
            }

            e.Success = true;
            return;
        }

        private void addContextParameters(Match m, string name, ExtractionEventArgs e)
        {
            if (m.Groups.Count > 2)
            {
                for (int g = 1; g < m.Groups.Count; g++)
                {
                    e.WebTest.Context.Add(name + "_g" + g, m.Groups[g].Value);
                }
            }
            else
            {
                e.WebTest.Context.Add(name, m.Groups[1].Value);
            }
        }
    }
}
