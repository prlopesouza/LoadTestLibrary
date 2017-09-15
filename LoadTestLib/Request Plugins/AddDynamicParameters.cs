using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Add Dynamic Parameters")]
    [Description("Adiciona parâmetros dinâmicos à requisição com base em um vetor de variáveis (criado com o plugin ExtractRandomRegex).")]
    public class AddDynamicParameters : WebTestRequestPlugin
    {
        private string _NameTemplate;
        [Description("Template para o nome do parâmetro ('$i$' será substituído pelo valor do grupo i).")]
        public string NameTemplate
        {
            get { return _NameTemplate; }
            set { _NameTemplate = value; }
        }

        private string _ValueTemplate;
        [Description("Template para o valor do parâmetro ('$i$' será substituído pelo valor do grupo i).")]
        public string ValueTemplate
        {
            get { return _ValueTemplate; }
            set { _ValueTemplate = value; }
        }

        private string _ParameterVariable;
        [Description("O prefixo do vetor de variáveis, sem incluir indíces e grupos, que será adicionado à requisição.")]
        public string ParameterVariable
        {
            get { return _ParameterVariable; }
            set { _ParameterVariable = value; }
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            if (!e.WebTest.Context.ContainsKey(_ParameterVariable + "_Nr")) return;

            int count = int.Parse(e.WebTest.Context[_ParameterVariable + "_Nr"].ToString());

            if (count != 0)
            {

                int g_count = int.Parse(e.WebTest.Context[_ParameterVariable + "_Gr"].ToString());

                FormPostHttpBody body = e.Request.Body as FormPostHttpBody;

                Regex rx = new Regex("\\{\\{([^{}]+)\\}\\}");

                string paramName = _ParameterVariable;
                string paramName2 = _ParameterVariable;

                for (int i = 1; i <= count; i++)
                {
                    string name = _NameTemplate;
                    string value = _ValueTemplate;

                    if (count > 1)
                    {
                        paramName = _ParameterVariable + "_" + i;
                        paramName2 = paramName;
                    }

                    for (int g = 1; g <= g_count; g++)
                    {
                        if (g_count > 1) paramName2 = paramName + "_g" + g;

                        name = name.Replace("$" + g + "$", e.WebTest.Context[paramName2].ToString());
                        value = value.Replace("$" + g + "$", e.WebTest.Context[paramName2].ToString());

                        if (rx.IsMatch(value))
                        {
                            Match m = rx.Match(value);
                            string varName = m.Groups[1].Value;
                            value = value.Replace("{{" + varName + "}}", e.WebTest.Context[varName].ToString());
                        }
                    }
                    body.FormPostParameters.Add(name, value);
                }

                e.Request.Body = body;

            }
            base.PreRequest(sender, e);
        }
    }
}
