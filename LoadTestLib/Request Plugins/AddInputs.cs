using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;

namespace CorePlugins
{
    public class AddInputs : WebTestRequestPlugin
    {
        private string _Prefix;
        public string Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            base.PreRequest(sender, e);

            FormPostHttpBody body = (FormPostHttpBody)e.Request.Body;

            int count = int.Parse(e.WebTest.Context[_Prefix].ToString());

            for (int i = 1; i <= count; i++)
            {
                string name = e.WebTest.Context[_Prefix + "_NAME_" + i].ToString();
                string value = e.WebTest.Context[_Prefix + "_VALUE_" + i].ToString();
                if (!body.FormPostParameters.Contains(name))
                {
                    body.FormPostParameters.Add(name, value);
                }
            }

            e.Request.Body = body;
        }
    }
}
