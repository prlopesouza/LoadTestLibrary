using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Get Var by Index")]
    [Description("Retorna o valor de uma variável vetor em um índice específico e armazena em uma variável comum.")]
    public class GetVarByIndex : WebTestRequestPlugin
    {
        private string _Variable;
        [Description("Prefixo do vetor de variáveis, sem incluir o índice.")]
        public string Variable
        {
            get { return _Variable; }
            set { _Variable = value; }
        }

        private string _Index;
        [Description("Índice do valor que será retornado.")]
        public string Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        private string _ResultVariable;
        [Description("Variável que irá armazenar o valor retornado.")]
        public string ResultVariable
        {
            get { return _ResultVariable; }
            set { _ResultVariable = value; }
        }

        private bool _preRequest;
        [Description("Deve ser executado antes do request.")]
        public bool preRequest
        {
            get { return _preRequest; }
            set { _preRequest = value; }
        }

        private bool _useGroups;
        [Description("Deve ser utilizado um sufixo de grupo em um indice específico. Ex: _g1")]
        public bool useGroups
        {
            get { return _useGroups; }
            set { _useGroups = value; }
        }

        private int _groupIndex;
        [Description("Índice do grupo a ser utilizado.")]
        public int groupIndex
        {
            get { return _groupIndex; }
            set { _groupIndex = value; }
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            if (_preRequest)
            {
                string ind = _Index;
                if (_Index.Contains("{{"))
                {
                    ind = e.WebTest.Context[_Index.Replace("{{", "").Replace("}}", "")].ToString();
                }
                string val = "";
                if (_useGroups)
                {
                    val = e.WebTest.Context[_Variable + "_" + ind + "_g" + _groupIndex].ToString();
                }
                else
                {
                    val = e.WebTest.Context[_Variable + "_" + ind].ToString();
                }

                e.WebTest.Context[_ResultVariable] = val;
                e.Request.UrlWithQueryString.Replace("{{" + _ResultVariable + "}}", val);
            }

            base.PreRequest(sender, e);
        }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            if (!_preRequest)
            {
                string ind = _Index;
                if (_Index.Contains("{{"))
                {
                    ind = e.WebTest.Context[_Index.Replace("{{", "").Replace("}}", "")].ToString();
                }
                string val = "";
                if (_useGroups)
                {
                    val = e.WebTest.Context[_Variable + "_" + ind + "_g" + _groupIndex].ToString();
                }
                else
                {
                    val = e.WebTest.Context[_Variable + "_" + ind].ToString();
                }

                e.WebTest.Context[_ResultVariable] = val;
            }

            base.PostRequest(sender, e);
        }
    }
}
