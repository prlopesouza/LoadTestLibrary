using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.WebTesting;
using System.ComponentModel;

namespace LoadTestLib.Request_Plugins
{
    [DisplayName("Payment Data")]
    [Description("Este plugin gera os dados necessário para realizar um pagamento com cartão de crédito.")]
    public class PaymentData : WebTestRequestPlugin
    {
        private string _CardType;
        [Description("Tipo do cartão de crédito para a geração do número. (1=VISA, 2=Mastercard, 3=Amex, 4=Diners)")]
        public string CardType
        {
            get { return _CardType; }
            set { _CardType = value; }
        }

        private string _CardNumberVariable;
        [Description("Variável que irá armazenar o número do cartão.")]
        public string CardNumberVariable
        {
            get { return _CardNumberVariable; }
            set { _CardNumberVariable = value; }
        }

        private string _SecurityCodeVariable;
        [Description("Variável que irá armazenar o código de segurança do cartão.")]
        public string SecurityCodeVariable
        {
            get { return _SecurityCodeVariable; }
            set { _SecurityCodeVariable = value; }
        }

        private string _ExpirationMonthVariable;
        [Description("Variável que irá armazenar o mês de expiração do cartão.")]
        public string ExpirationMonthVariable
        {
            get { return _ExpirationMonthVariable; }
            set { _ExpirationMonthVariable = value; }
        }

        private string _ExpirationYearVariable;
        [Description("Variável que irá armazenar o ano de expiração do cartão.")]
        public string ExpirationYearVariable
        {
            get { return _ExpirationYearVariable; }
            set { _ExpirationYearVariable = value; }
        }

        private string _CPFVariable;
        [Description("Variável que irá armazenar o CPF do proprietário do cartão.")]
        public string CPFVariable
        {
            get { return _CPFVariable; }
            set { _CPFVariable = value; }
        }

        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            Random rd = new Random();

            string cardNumber = "";
            if (_CardType.Equals("1"))
            {
                cardNumber = Auxiliary.RandomCreditCardNumberGenerator.GenerateVisaNumber();
            }
            else if (_CardType.Equals("2"))
            {
                cardNumber = Auxiliary.RandomCreditCardNumberGenerator.GenerateMasterCardNumber();
            }
            else if (_CardType.Equals("3"))
            {
                cardNumber = Auxiliary.RandomCreditCardNumberGenerator.GenerateAmexNumber();
            }
            else if (_CardType.Equals("4"))
            {
                cardNumber = Auxiliary.RandomCreditCardNumberGenerator.GenerateDinersNumber();
            }
            else
            {
                cardNumber = Auxiliary.RandomCreditCardNumberGenerator.GenerateGenericNumber();
            }

            string securityCode = rd.Next(100, 1000).ToString();
            string expMonth = rd.Next(1, 13).ToString("00");
            string expYear = rd.Next(DateTime.Now.Year+1, DateTime.Now.Year+11).ToString();
            string CPF = Auxiliary.AuxFunctions.GerarCpf();

            e.WebTest.Context[_CardNumberVariable] = cardNumber;
            e.WebTest.Context[_SecurityCodeVariable] = securityCode;
            e.WebTest.Context[_ExpirationMonthVariable] = expMonth;
            e.WebTest.Context[_ExpirationYearVariable] = expYear;
            e.WebTest.Context[_CPFVariable] = CPF;

            base.PostRequest(sender, e);
        }
    }
}
