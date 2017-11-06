using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Increment")]
    [Description("Incrementa o valor de uma variável numérica.")]
    public class Increment : WebTestRequestPlugin
    {
        private string _Variable;
        [Description("Nome da variável que será incrementada.")]
        public string Variable
        {
            get { return _Variable; }
            set { _Variable = value; }
        }

        private int _incValue;
        [Description("Valor que será incrementado na variável.")]
        public int incValue
        {
            get { return _incValue; }
            set { _incValue = value; }
        }

        private bool _preRequest;
        [Description("Deve ser executado antes do request.")]
        public bool preRequest
        {
            get { return _preRequest; }
            set { _preRequest = value; }
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            if (_preRequest)
            {
                int val = int.Parse(e.WebTest.Context[_Variable].ToString());

                e.WebTest.Context[_Variable] = val + _incValue;
            }
            base.PreRequest(sender, e);
        }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            if (!_preRequest && e.Response.StatusCode!=System.Net.HttpStatusCode.Redirect)
            {
                int val = int.Parse(e.WebTest.Context[_Variable].ToString());

                e.WebTest.Context[_Variable] = val + _incValue;
            }
            base.PostRequest(sender, e);
        }
    }
}
