using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.Web;
using System.Globalization;
using System.ComponentModel;

namespace PromocaoZaad_Plugin
{
    [DisplayName("Extract Random Select Option")]
    [Description("Extrai uma opção aleatória de um Select.")]
    public class ExtractRandomSelectOption : ExtractionRule
    {
        private string _SelectName;
        [Description("O nome do select que será buscado.")]
        public string SelectName
        {
            get { return _SelectName; }
            set { _SelectName = value; }
        }

        private string _ExcludeOption;
        [Description("Opções do select que devem ser excluídas, separadas por '|'. Ex: \"Opcao1|Opcao2\"")]
        public string ExcludeOption
        {
            get { return _ExcludeOption; }
            set { _ExcludeOption = value; }
        }

        public override void Extract(object sender, ExtractionEventArgs e)
        {
            string response = e.Response.BodyString;

            Regex rxSelect = new Regex("<select[^>]+[name|id]=\"" + _SelectName + "\"[^>]*>([\\s\\S]+?<\\/select>)");
            if (!_ExcludeOption.Equals("")) _ExcludeOption = "(?!" + _ExcludeOption + ")";
            Regex rxOption = new Regex("<option[^>]+value=\"" + _ExcludeOption + "([^\"]+)");
            

            MatchCollection select = rxSelect.Matches(response);
            if (select.Count==0)
            {
                e.Success = false;
                e.Message = String.Format(CultureInfo.CurrentCulture, "Select \"{0}\" Not Found.", _SelectName);
                return;
            }

            MatchCollection options = rxOption.Matches(select[0].Value);
            if (options.Count==0)
            {
                e.Success = false;
                e.Message = String.Format(CultureInfo.CurrentCulture, "No Valid Options On Select \"{0}\".", _SelectName);
                return;
            }

            Random rd = new Random();
            string rdOption = options[rd.Next(options.Count)].Groups[1].Value;
            e.WebTest.Context[ContextParameterName] = rdOption;
            e.Success = true;
        }
    }
}
