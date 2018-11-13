using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Random Number Generator")]
    [Description("Gera um número aleatório entre MinValue e MaxValue.")]
    public class RandomNumber : WebTestRequestPlugin
    {
        private string _contextVariable;
        [Description("Variável que irá armazenar o valor.")]
        public string ContextVariable
        {
            get { return _contextVariable; }
            set { _contextVariable = value; }
        }

        private int _minValue;
        [Description("Valor mínimo (inclusivo).")]
        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        private int _maxValue;
        [Description("Valor máximo (inclusivo).")]
        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            Random rd = new Random();
            int n = rd.Next(_minValue, _maxValue + 1);
            int aux = n;
            while (aux==n) {
                n = rd.Next(_minValue, _maxValue + 1);
            }
            e.WebTest.Context[_contextVariable] = n;

            base.PostRequest(sender, e);
        }
    }
}
