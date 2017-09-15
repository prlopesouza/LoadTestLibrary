using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Unix Time")]
    [Description("Este plugin armazena em uma variável o total de milisegundos desde 01/01/1970 (época) até o horário atual.")]
    public class UnixTime : WebTestRequestPlugin
    {
        private string _contextVariable;
        [Description("Variável que irá armazenar o UNIX Time em milisegundos.")]
        public string ContextVariable
        {
            get { return _contextVariable; }
            set { _contextVariable = value; }
        }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            DateTime epoch = new DateTime(1970, 1, 1);
            UInt64 unix = (UInt64)DateTime.UtcNow.Subtract(epoch).TotalMilliseconds;
            e.WebTest.Context[_contextVariable] = unix.ToString();

            base.PostRequest(sender, e);
        }
    }
}
