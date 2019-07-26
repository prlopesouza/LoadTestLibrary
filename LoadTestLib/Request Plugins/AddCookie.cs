using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Add Cookie")]
    [Description("Adiciona um cookie à requisição.")]
    public class AddCookie : WebTestRequestPlugin
    {
        private string _Name;
        [Description("Nome do cookie que será adicionado.")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Value;
        [Description("Valor do cookie que será adicionado.")]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            try
            {
                if (_Value.Contains("{{"))
                {
                    _Value = e.WebTest.Context[_Value.Replace("{{", "").Replace("}}", "")].ToString();
                }
            }
            catch(Exception ex)
            {
                e.WebTest.Context["EXCEPTION"] = ex.Message;
            }

            e.Request.Cookies.Add(new System.Net.Cookie(_Name, _Value));

            base.PreRequest(sender, e);
        }
    }
}
