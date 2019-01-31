using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Util
{
    public class NumberingFormatter
    {
        #region Arabic Numbers constants

        private const String thousand = "الف";
        private const String twoThousand = "الفان";
        private const String thousands = "آلاف";
        private const String million = "مليون";
        private const String twoMillion = "مليونان";
        private const String millions = "ملايين";
        private const String oneHundred = "المائة";
        private const String twoHundred = "المائتان";
        private const String threeHundred = "الثلاثمائة";
        private const String fourHundred = "الاربعمائة";
        private const String fiveHundred = "الخمسمائة";
        private const String sixHundred = "الستمائة";
        private const String sevenHundred = "السبعمائة";
        private const String eightHundred = "الثمانمائة";
        private const String nineHundred = "التسعمائة";
        private const String ten = "العاشره";
        private const String oneTens = "الحادية عشر";
        private const String twoTens = "الثانية عشر";
        private const String threeTens = "الثالثة عشر";
        private const String fourTens = "الرابعة عشر";
        private const String fiveTens = "الخامسة عشر";
        private const String sixTens = "السادسة عشر";
        private const String sevenTens = "السابعة عشر";
        private const String eightTens = "الثامنة عشر";
        private const String nineTens = "التاسعة عشر";
        private const String twenty = "العشرون";
        private const String thirty = "الثلاثون";
        private const String fourty = "الاربعون";
        private const String fifty = "الخمسون";
        private const String sixty = "الستون";
        private const String seventy = "السبعون";
        private const String eighty = "الثمانون";
        private const String ninty = "التسعون";
        private const String currency = "";//"ريال";
        private const String currency_Jama = "";// = "ريالات";
        private const String currency_Mothana = "";// = "ريالان";
        private const String connector = "و";
        private const String one = "الأولى";
        private const String two = "الثانية";
        private const String three = "الثالثة";
        private const String four = "الرابعة";
        private const String five = "الخامسة";
        private const String six = "السادسة";
        private const String seven = "السابعة";
        private const String eight = "الثامنة";
        private const String nine = "التاسعة";
        private const String suffix = "فقط";
        private const String tenMale = "العاشر";
        private const String oneTensMale = "الحادي عشر";
        private const String twoTensMale = "الثاني عشر";
        private const String threeTensMale = "الثالث عشر";
        private const String fourTensMale = "الرابع عشر";
        private const String fiveTensMale = "الخامس عشر";
        private const String sixTensMale = "السادس عشر";
        private const String sevenTensMale = "السابع عشر";
        private const String eightTensMale = "الثامن عشر";
        private const String nineTensMale = "التاسع عشر";
        private const String oneMale = "الأول";
        private const String twoMale = "الثاني";
        private const String threeMale = "الثالث";
        private const String fourMale = "الرابع";
        private const String fiveMale = "الخامس";
        private const String sixMale = "السادس";
        private const String sevenMale = "السابع";
        private const String eightMale = "الثامن";
        private const String nineMale = "التاسع";
        #endregion

        public String Number { get; set; }

        private Dictionary<String, String> ahaad;
        private Dictionary<String, String> asharat;
        private Dictionary<String, String> miaat;
        private Dictionary<String, String> wahdAshrat;


        private bool isMale = false;
        public NumberingFormatter(bool _isMale)
        {
            isMale = _isMale;
            IntialiazeDictionaries();
        }

        private void validateNumber(string number)
        {
            int zeroCounter = 0;
            if (number.Length > 9 || number.Length <= 0)
                throw new Exception("digits should be between one and nine digits");
            foreach (char c in number.ToCharArray())
            {
                if (!char.IsDigit(c))
                    throw new Exception("Characters are not allowed");
                else if (c == '0')
                    zeroCounter++;
            }
            if (zeroCounter == number.Length)
                throw new Exception("all zero is not allowed");
        }

        private void IntialiazeDictionaries()
        {
            ahaad = new Dictionary<string, string>();
            if (!isMale)
            {
                ahaad.Add("1", one);
                ahaad.Add("2", two);
                ahaad.Add("3", three);
                ahaad.Add("4", four);
                ahaad.Add("5", five);
                ahaad.Add("6", six);
                ahaad.Add("7", seven);
                ahaad.Add("8", eight);
                ahaad.Add("9", nine);
            }
            else
            {
                ahaad.Add("1", oneMale);
                ahaad.Add("2", twoMale);
                ahaad.Add("3", threeMale);
                ahaad.Add("4", fourMale);
                ahaad.Add("5", fiveMale);
                ahaad.Add("6", sixMale);
                ahaad.Add("7", sevenMale);
                ahaad.Add("8", eightMale);
                ahaad.Add("9", nineMale);
            }
            wahdAshrat = new Dictionary<string, string>();
            if (!isMale)
            {
                wahdAshrat.Add("11", oneTens);
                wahdAshrat.Add("12", twoTens);
                wahdAshrat.Add("13", threeTens);
                wahdAshrat.Add("14", fourTens);
                wahdAshrat.Add("15", fiveTens);
                wahdAshrat.Add("16", sixTens);
                wahdAshrat.Add("17", sevenTens);
                wahdAshrat.Add("18", eightTens);
                wahdAshrat.Add("19", nineTens);
            }
            else
            {
                wahdAshrat.Add("11", oneTensMale);
                wahdAshrat.Add("12", twoTensMale);
                wahdAshrat.Add("13", threeTensMale);
                wahdAshrat.Add("14", fourTensMale);
                wahdAshrat.Add("15", fiveTensMale);
                wahdAshrat.Add("16", sixTensMale);
                wahdAshrat.Add("17", sevenTensMale);
                wahdAshrat.Add("18", eightTensMale);
                wahdAshrat.Add("19", nineTensMale);
            }
            asharat = new Dictionary<string, string>();
            if(!isMale)
                asharat.Add("1", ten);
            else
                asharat.Add("1", tenMale);
            asharat.Add("2", twenty);
            asharat.Add("3", thirty);
            asharat.Add("4", fourty);
            asharat.Add("5", fifty);
            asharat.Add("6", sixty);
            asharat.Add("7", seventy);
            asharat.Add("8", eighty);
            asharat.Add("9", ninty);
            miaat = new Dictionary<string, string>();
            miaat.Add("1", oneHundred);
            miaat.Add("2", twoHundred);
            miaat.Add("3", threeHundred);
            miaat.Add("4", fourHundred);
            miaat.Add("5", fiveHundred);
            miaat.Add("6", sixHundred);
            miaat.Add("7", sevenHundred);
            miaat.Add("8", eightHundred);
            miaat.Add("9", nineHundred);
        }

        /// <summary>
        /// create a numbering string for any nine-digits string with currency suffix
        /// </summary>
        /// <returns>returns a numbering string for any nine-digits string with currency suffix</returns>
        public string getResultEnhanced(int number)
        {
            Number = number.ToString();
            validateNumber(Number);
            String result = "";

            if (Number.Length % 3 == 1)
                Number = "00" + Number;
            else if (Number.Length % 3 == 2)
                Number = "0" + Number;


            if (Number.Length == 3)
                result = getThreeNumberCase(Number);

            else if (Number.Length == 6)
            {
                result = getSixNumberCase(Number);
            }

            else if (Number.Length == 9)
            {
                result = getNineNumberCase(Number);
            }

            result = result.Replace("  ", " ");
            result = result.Replace("   ", " ");
            result = result.Replace(" و ريال", " ريال ");

            return result;
        }

        private string getNineNumberCase(string number)
        {
            string result = "";

            string millionCount = number.Substring(0, 3);
            string restOfTheNumber = number.Substring(3, 6);

            result = getThreeNumberCase(millionCount);

            result = removeCurrencySuffix(result);

            if (millionCount[0] == '0' && millionCount[1] == 0)
                result = result + " " + millions;
            else
                result = result + " " + million;

            if (restOfTheNumber != "000000")
                result += " " + connector + " " + getSixNumberCase(restOfTheNumber);
            else
                result += " " + currency;



            return result;

        }

        private string getSixNumberCase(String number)
        {
            string result = "";
            if (number == "000000")
                return result + " " + currency;


            string hundrendCount = number.Substring(3, 3);
            string thousandCount = number.Substring(0, 3);


            result = getThreeNumberCase(thousandCount);

            result = removeCurrencySuffix(result);

            int ashraatNumber = int.Parse(thousandCount.Substring(1, 2));

            if (ashraatNumber < 11 && ashraatNumber > 0 && thousandCount[0] == '0')
            {
                if (thousandCount[2] == '1')
                    result = thousand;
                else if (thousandCount[2] == '2')
                    result = twoThousand;
                else
                    result = result + " " + thousands;
            }
            else
            {
                result = result + " " + thousand;

            }

            if (thousandCount == "000")
                result = "";

            if (hundrendCount != "000")//to not call the function and add the currcny at the end
                if (thousandCount != "000")//to remove the additional conectors 
                    result += " " + connector + " " + getThreeNumberCase(hundrendCount);
                else
                    result += " " + getThreeNumberCase(hundrendCount);
            else
                result += " " + currency;

            return result;
        }

        /// <summary>
        /// used to remove the currency suffix from any three digits string
        /// </summary>
        /// <param name="result">the number to remove currency from</param>
        /// <returns>three digits string without currncy suffix</returns>
        private string removeCurrencySuffix(string result)
        {
            if (result.Contains(currency_Jama))
                result = result.Replace(currency_Jama, " ");
            else if (result.Contains(currency_Mothana))
                result = result.Replace(currency_Mothana, " ");
            else if (result.Contains(currency))
                result = result.Replace(currency, " ");
            return result;
        }

        /*
        public String getResult()
        {
            String result  ="";
            if (String.IsNullOrEmpty(Number))
                throw new Exception("Number to be formatted is Empty");
            if (Number.Length == 1)
            {
                result = getOneNumberCase(Number);
            }
            else if (Number.Length == 2)
            {
                result = getTwoNumberCase(Number);
            }
            else if (Number.Length == 3)
            {
                result = getThreeNumberCase(Number);
            }
            else if (Number.Length == 4)
            {
                result = getFourNumberCase(Number);
            }
            else if (Number.Length == 5)
            {
                Number = "0" + Number;
                result = getThreeNumberCase(Number.Substring(0, 3)) + " " + thousand + " " + connector + " " + getThreeNumberCase(Number.Substring(3, 3));
                result = result.Remove(result.IndexOf("ريال"), "ريال".Length);
                if (Number.Substring(3, 3) == "000")
                    result = result.Replace(" و ", " ");
            }
            else if (Number.Length == 6)
            {
                string toBeEval = Number;
                result = getThreeNumberCase(toBeEval.Substring(0, 3)) + " " + thousand + " " + currency + " " + connector + " " + getThreeNumberCase(toBeEval.Substring(3, 3));
                result = result.Remove(result.IndexOf("ريال"), "ريال".Length);
                if (result.Contains(" ات "))
                {
                    result = result.Remove(result.IndexOf(" ات "), " ات ".Length);
                }
            }

            return result + " " + suffix;
        }
        */
        /*
        private string getFourNumberCase(string number)
        {
            string result = "";
            if (number[0].ToString() == "1")
                result = thousand + " " + connector + " " + getThreeNumberCase(number.Substring(1, 3));
            else if (number[0].ToString() == "2")
                result = twoThousand + " " + connector + " " + getThreeNumberCase(number.Substring(1, 3));
            else
                result = ahaad[number[0] + ""] + " " + connector + " " + thousands + " " + getThreeNumberCase(number.Substring(1, 3));
            if (number.Substring(1, 3) == "000")
                result = result.Replace("و", "");
            return result;
        }*/

        /// <summary>
        /// Create the numbering string for any three digits string
        /// and adds currency suffix at the end
        /// </summary>
        /// <param name="number">three digits string</param>
        /// <returns>Numbering string with currency suffix</returns>
        private string getThreeNumberCase(string number)
        {
            string result = "";
            if (number == "000")
                return currency;
            if (number[2].ToString() == "0" && number[1].ToString() == "0")
            {
                result = miaat[number[0].ToString()] + " " + currency;
            }
            else if (number[0].ToString() == "0")
                return result = getTwoNumberCase(number.Substring(1, 2));
            else
            {
                string sub = number.Substring(1, 2);
                result = miaat[number[0].ToString()] + " " + connector + " " + getTwoNumberCase(sub);
            }
            return result;
        }

        private string getTwoNumberCase(string number)
        {
            String result = "";
            if ((number[1].ToString() == "0") && (number[0].ToString() != "0"))
                result = asharat[number[0] + ""];
            else if ((number[1].ToString() != "0") && (number[0].ToString() == "1"))
                result = wahdAshrat[number];
            else if ((number[0].ToString() == "0") && (number[1].ToString() != "0"))
                return getOneNumberCase(number[1].ToString());
            else
                result = ahaad[number[1].ToString()] + " " + connector + " " + asharat[number[0] + ""];
            return result + " " + currency;
        }

        private String getOneNumberCase(String number)
        {
            String result = "";
            switch (number)
            {
                case "1":
                    result = isMale == false ? one : oneMale + " " + currency;
                    break;
                case "2":
                    result = isMale == false ? two : twoMale + " " + currency;
                    break;
                case "3":
                    result = isMale == false ? three : threeMale + " " + currency_Jama;
                    break;
                case "4":
                    result = isMale == false ? four : fourMale + " " + currency_Jama;
                    break;
                case "5":
                    result = isMale == false ? five : fiveMale + " " + currency_Jama;
                    break;
                case "6":
                    result = isMale == false ? six : sixMale + " " + currency_Jama;
                    break;
                case "7":
                    result = isMale == false ? seven : sevenMale + " " + currency_Jama;
                    break;
                case "8":
                    result = isMale == false ? eight : eightMale + " " + currency_Jama;
                    break;
                case "9":
                    result = isMale == false ? nine : nineMale + " " + currency_Jama;
                    break;

                default:
                    break;
            }

            return result;
        }

    }
}
