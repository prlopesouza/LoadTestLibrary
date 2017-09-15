using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTestLib.Auxiliary
{
    public static class RandomCreditCardNumberGenerator
    {
        public static string[] AMEX_PREFIX_LIST = new[] { "34", "37" };

        public static string[] GENERIC_PREFIX_LIST = new[] { "99", "00" };

        public static string[] DINERS_PREFIX_LIST = new[]
                                                        {
                                                            "300",
                                                            "301", "302", "303", "36", "38"
                                                        };

        public static string[] MASTERCARD_PREFIX_LIST = new[]
                                                            {
                                                                "51",
                                                                "52", "53", "54", "55",
                                                                "2221",
                                                                "2222",
                                                                "2223",
                                                                "2224",
                                                                "2225",
                                                                "2226",
                                                                "2227",
                                                                "2228",
                                                                "2229",
                                                                "223",
                                                                "224",
                                                                "225",
                                                                "226",
                                                                "227",
                                                                "228",
                                                                "229",
                                                                "23",
                                                                "24",
                                                                "25",
                                                                "26",
                                                                "270",
                                                                "271",
                                                                "2720"
                                                            };

        public static string[] VISA_PREFIX_LIST = new[]
                                                    {
                                                        "4539",
                                                        "4556", "4916", "4532", "4929", "40240071", "4485", "4716", "4"
                                                    };

        /*
      'prefix' is the start of the  CC number as a string, any number
        private of digits   'length' is the length of the CC number to generate.
     * Typically 13 or  16
        */
        private static string CreateFakeCreditCardNumber(this Random random, string prefix, int length)
        {
            string ccnumber = prefix;
            while (ccnumber.Length < (length - 1))
            {
                double rnd = (random.NextDouble() * 1.0f - 0f);

                ccnumber += Math.Floor(rnd * 10);
            }


            // reverse number and convert to int
            var reversedCCnumberstring = ccnumber.ToCharArray().Reverse();

            var reversedCCnumberList = reversedCCnumberstring.Select(c => Convert.ToInt32(c.ToString()));

            // calculate sum //Luhn Algorithm http://en.wikipedia.org/wiki/Luhn_algorithm
            int sum = 0;
            int pos = 0;
            int[] reversedCCnumber = reversedCCnumberList.ToArray();

            while (pos < length - 1)
            {
                int odd = reversedCCnumber[pos] * 2;

                if (odd > 9)
                    odd -= 9;

                sum += odd;

                if (pos != (length - 2))
                    sum += reversedCCnumber[pos + 1];

                pos += 2;
            }

            // calculate check digit
            int checkdigit =
                Convert.ToInt32((Math.Floor((decimal)sum / 10) + 1) * 10 - sum) % 10;

            ccnumber += checkdigit;

            return ccnumber;
        }


        public static IEnumerable<string> GetCreditCardNumbers(string[] prefixList, int length,
                                                  int howMany)
        {
            var result = new Stack<string>();
            var random = new Random();
            for (int i = 0; i < howMany; i++)
            {
                int randomPrefix = random.Next(0, prefixList.Length - 1);

                if (randomPrefix > 1)  //Why??, is it a bug ? it never will select last element
                {
                    randomPrefix--;
                }

                string ccnumber = prefixList[randomPrefix];

                result.Push(CreateFakeCreditCardNumber(random, ccnumber, length));
            }

            return result;
        }

        public static string GenerateMasterCardNumber()
        {
            return GetCreditCardNumbers(MASTERCARD_PREFIX_LIST, 16, 1).First();
        }

        public static string GenerateVisaNumber()
        {
            return GetCreditCardNumbers(VISA_PREFIX_LIST, 16, 1).First();
        }

        public static string GenerateAmexNumber()
        {
            return GetCreditCardNumbers(AMEX_PREFIX_LIST, 16, 1).First();
        }

        public static string GenerateDinersNumber()
        {
            return GetCreditCardNumbers(DINERS_PREFIX_LIST, 16, 1).First();
        }

        public static string GenerateGenericNumber()
        {
            return GetCreditCardNumbers(GENERIC_PREFIX_LIST, 16, 1).First();
        }

        public static bool IsValidCreditCardNumber(string creditCardNumber)
        {
            try
            {
                var reversedNumber = creditCardNumber.ToCharArray().Reverse();

                int mod10Count = 0;
                for (int i = 0; i < reversedNumber.Count(); i++)
                {
                    int augend = Convert.ToInt32(reversedNumber.ElementAt(i).ToString());

                    if (((i + 1) % 2) == 0)
                    {
                        string productstring = (augend * 2).ToString();
                        augend = 0;
                        for (int j = 0; j < productstring.Length; j++)
                        {
                            augend += Convert.ToInt32(productstring.ElementAt(j).ToString());
                        }
                    }
                    mod10Count += augend;
                }

                if ((mod10Count % 10) == 0)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
