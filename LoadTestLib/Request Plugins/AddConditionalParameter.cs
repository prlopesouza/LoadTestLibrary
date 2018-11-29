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
    [DisplayName("Add Conditional Parameter")]
    [Description("Adiciona um parâmetro à requisição de acordo com uma condição específica.")]
    public class AddConditionalParameter : WebTestRequestPlugin
    {
        private string _ParameterName;
        [Description("")]
        public string ParameterName
        {
            get { return _ParameterName; }
            set { _ParameterName = value; }
        }

        private string _ParameterValue;
        [Description("")]
        public string ParameterValue
        {
            get { return _ParameterValue; }
            set { _ParameterValue = value; }
        }

        private string _ConditionValue1;
        [Description("")]
        public string ConditionValue1
        {
            get { return _ConditionValue1; }
            set { _ConditionValue1 = value; }
        }

        private string _ConditionValue2;
        [Description("")]
        public string ConditionValue2
        {
            get { return _ConditionValue2; }
            set { _ConditionValue2 = value; }
        }

        private string _ConditionOperator;
        [Description("")]
        public string ConditionOperator
        {
            get { return _ConditionOperator; }
            set { _ConditionOperator = value; }
        }

        public override void PreRequest(object sender, PreRequestEventArgs e)
        {
            FormPostHttpBody body = new FormPostHttpBody();
            if (e.Request.Body != null) body = e.Request.Body as FormPostHttpBody;
            
            _ParameterName = ReplaceContextVar(_ParameterName, e);
            _ParameterValue = ReplaceContextVar(_ParameterValue, e);
            _ConditionValue1 = ReplaceContextVar(_ConditionValue1, e);
            _ConditionValue2 = ReplaceContextVar(_ConditionValue2, e);
            _ConditionOperator = ReplaceContextVar(_ConditionOperator, e);

            bool ok = false;

            switch (_ConditionOperator)
            {
                case "==":
                    if (_ConditionValue1 == _ConditionValue2) ok = true;
                    break;
                case "!=":
                    if (_ConditionValue1 != _ConditionValue2) ok = true;
                    break;
                case ">":
                    
                    break;
                case "<":
                    
                    break;
                case ">=":
                    
                    break;
                case "<=":
                    
                    break;
            }

            if (ok)
            {
                body.FormPostParameters.Add(_ParameterName, _ParameterValue);
                e.Request.Body = body;
            }

            base.PreRequest(sender, e);
        }

        private string ReplaceContextVar(string text, PreRequestEventArgs e)
        {
            Regex rx = new Regex("\\{\\{([^{}]+)\\}\\}");

            if (rx.IsMatch(text))
            {
                Match m = rx.Match(text);
                string varName = m.Groups[1].Value;
                text = text.Replace("{{" + varName + "}}", e.WebTest.Context[varName].ToString());
            }

            return text;
        }
    }
}
