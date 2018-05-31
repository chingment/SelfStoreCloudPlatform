using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Lumos.Common
{
    public class PersonalIncomeTaxLevel
    {
        public static List<PersonalIncomeTaxLevel> lsTax = new List<PersonalIncomeTaxLevel>(); //用来存储税率

        public static int iStartPoint = 0; //起征点


        private int taxRate = 0; //税率
        private int takeOut = 0; //扣除值
        private int taxValue = 0;//含税所得额判断税率值

        public PersonalIncomeTaxLevel(int TaxRate, int TakeOut, int TaxValue)
        {
            taxRate = TaxRate;
            takeOut = TakeOut;
            taxValue = TaxValue;
        }

        public int TaxRate
        {
            get { return taxRate; }
        }

        public int TakeOut
        {
            get { return takeOut; }
        }

        public int TaxValue
        {
            get { return taxValue; }
        }


        private static PersonalIncomeTaxLevel GetCurTaxLevel(double dMoney)
        {
            double iMoney = dMoney;
            PersonalIncomeTaxLevel taxResult = null;

            if (iMoney <= lsTax[0].TaxValue) taxResult = lsTax[0];
            if (iMoney > lsTax[lsTax.Count - 1].TaxValue) taxResult = lsTax[lsTax.Count - 1];

            for (int i = 1; i < lsTax.Count - 1; i++)
            {
                if (iMoney > lsTax[i - 1].TaxValue && iMoney <= lsTax[i].TaxValue)
                    taxResult = lsTax[i];
            }

            return taxResult;
        }


        public static double GetTaxedMoney(double d)
        {
            if (d > 0)
            {
                double dMoney = d - double.Parse(iStartPoint.ToString());
                if (dMoney > 0)
                {
                    PersonalIncomeTaxLevel curTax = GetCurTaxLevel(dMoney);
                    double dTax = dMoney * curTax.TaxRate / 100 - curTax.TakeOut;
                    return dTax;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        public static void SetTaxLevel(string xmlurl)
        {
            if (string.IsNullOrEmpty(xmlurl))
            {
                throw new Exception("PersonalIncomeTax没有设置xmlurl");
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(xmlurl);

            XmlNode xmlRoot = xmldoc.SelectSingleNode("Config");
            XmlNode xmlTax = xmlRoot.SelectSingleNode("Tax");

            iStartPoint = Convert.ToInt32(xmlTax.Attributes["StartPoint"].Value);
            int iRate = 0, iTakeOut = 0, iTaxValue = 0;

            foreach (XmlNode xmlnode in xmlTax.ChildNodes)
            {
                iRate = Convert.ToInt32(xmlnode.Attributes["Rate"].Value);
                iTakeOut = Convert.ToInt32(xmlnode.Attributes["TakeOut"].Value);
                iTaxValue = Convert.ToInt32(xmlnode.InnerText);

                lsTax.Add(new PersonalIncomeTaxLevel(iRate, iTakeOut, iTaxValue));
            }

        }
    }
}
