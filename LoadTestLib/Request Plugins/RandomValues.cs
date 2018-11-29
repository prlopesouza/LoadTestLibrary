using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Random Values Generator")]
    [Description("Pega um valor aleatório a partir de um conjunto pré-estabelecido.")]
    public class RandomValues : WebTestRequestPlugin
    {
        private string _contextVariable;
        [Description("Variável que irá armazenar o valor.")]
        public string ContextVariable
        {
            get { return _contextVariable; }
            set { _contextVariable = value; }
        }

        private string _values;
        [Description("Conjunto de valores possíveis.")]
        public string Values
        {
            get { return _values; }
            set { _values = value; }
        }

        private char _separator;
        [Description("Separador dos valores.")]
        public char Separator
        {
            get { return _separator; }
            set { _separator = value; }
        }

        private bool _preRequest;
        [Description("Aplicar antes do request?")]
        public bool PreRequest
        {
            get { return _preRequest; }
            set { _preRequest = value; }
        }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            if (!_preRequest)
            {
                string[] values = _values.Split(_separator);

                Random rd = new Random();
                int i = rd.Next(0, values.Length);

                if (values.Length > 1)
                {
                    int aux = i;
                    while (aux == i) i = rd.Next(0, values.Length);
                }

                string value = values[i];
                value = ReplaceContextVar(value, e.WebTest);
                e.WebTest.Context[_contextVariable] = value;
            }

            base.PostRequest(sender, e);
        }

        public override void PreRequestDataBinding(object sender, PreRequestDataBindingEventArgs e)
        {
            if (_preRequest)
            {
                string[] values = _values.Split(_separator);

                Random rd = new Random();
                int i = rd.Next(0, values.Length);

                if (values.Length > 1)
                {
                    int aux = i;
                    while (aux == i) i = rd.Next(0, values.Length);
                }

                string value = values[i];
                value = ReplaceContextVar(value, e.WebTest);
                e.WebTest.Context[_contextVariable] = value;
            }

            base.PreRequestDataBinding(sender, e);
        }

        private string ReplaceContextVar(string text, WebTest w)
        {
            Regex rx = new Regex("\\{\\{([^{}]+)\\}\\}");

            if (rx.IsMatch(text))
            {
                Match m = rx.Match(text);
                string varName = m.Groups[1].Value;
                text = text.Replace("{{" + varName + "}}", w.Context[varName].ToString());
            }

            return text;
        }
    }
}
