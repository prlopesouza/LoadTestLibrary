using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace LoadTestLib.Extraction_Rules
{
    [DisplayName("Extract Response Code")]
    [Description("Extrai o status da resposta HTTP.")]
    public class ExtractResponseCode : ExtractionRule
    {
        public override void Extract(object sender, ExtractionEventArgs e)
        {
            var responseCode = e.Response.StatusCode.ToString();

            e.WebTest.Context.Add(ContextParameterName, responseCode);
        }
    }
}
