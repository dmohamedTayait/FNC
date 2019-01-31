using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TayaIT.Enterprise.EMadbatah.Util
{

    public enum CalendarTypes
    {
        Hijri,
        Gregorian
    }


    public class DateUtils
    {
        public static DateTimeFormatInfo ConvertDateCalendar(DateTime dateConv, CalendarTypes calendar, string dateLangCulture)
        {
            //dateConv = dateConv.Subtract(new TimeSpan(1, 0, 0, 0));
            DateTimeFormatInfo dateFormat;
            dateLangCulture = dateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error - LAITH - 11/13/2005 1:01:45 PM -

            if (calendar == CalendarTypes.Hijri && dateLangCulture.StartsWith("en-"))
            {
                dateLangCulture = "ar-sa";
            }

            /// Set the date time format to the given culture - LAITH - 11/13/2005 1:04:22 PM -
            dateFormat = new System.Globalization.CultureInfo(dateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar - LAITH - 11/13/2005 1:04:52 PM -
            switch (calendar)
            {
                case CalendarTypes.Hijri:
                    dateFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;

                case CalendarTypes.Gregorian:
                    dateFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;
            }

            /// We format the date structure to whatever we want - LAITH - 11/13/2005 1:05:39 PM -
            dateFormat.ShortDatePattern = "dd/MM/yyyy";
            dateConv.Date.ToString("f", dateFormat);
            return dateFormat;
            //return dateConv;//(dateConv.Date.ToString("f", dateFormat));
        }




                
         //       private const int startGreg=1900;
         //       private const int endGreg=2100;
         //       private string[] allFormats={"yyyy/MM/dd","yyyy/M/d",
         //               "dd/MM/yyyy","d/M/yyyy",
         //               "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
         //               "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
         //               "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
         //               "yyyy M d","dd MM yyyy","d M yyyy",
         //               "dd M yyyy","d MM yyyy"};
         //       private CultureInfo arCul;
         //       private CultureInfo enCul;
         //       private HijriCalendar h;
         //       private GregorianCalendar g;

         //       public DateUtils()
         //       {
         //               arCul=new CultureInfo("ar-SA");
         //               enCul=new CultureInfo("en-US");

         //               h=new  HijriCalendar();
         //               g=new GregorianCalendar(GregorianCalendarTypes.USEnglish);

         //               arCul.DateTimeFormat.Calendar=h;
                        
         //       }
                
         //          /// <summary>

         //       /// Check if string is hijri date and then return true 
         //       ///التحقق هل النص المدخل تاريخ هجري و إرجاع قيمة صحيحة 
         //       /// </summary>

         //       /// <PARAM name="hijri"></PARAM>

         //       /// <returns></returns>

         //       public bool IsHijri(string hijri)
         //       {

         //           try
         //           {
         //               DateTime tempDate = DateTime.ParseExact(hijri, allFormats,
         //                        arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
         //               if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
         //                   return true;
         //               else
         //                   return false;
         //           }
         //           catch (Exception ex)
         //           {
         //               //cur.Trace.Warn("IsHijri Error :" + hijri.ToString() + "\n" +
         //               //                                  ex.Message);
         //               return false;
         //           }

         //       }
         //       /// <summary>
         //       /// Check if string is Gregorian date and then return true 
         //       ///التحقق هل النص المدخل تاريخ ميلادي و إرجاع قيمة صحيحة 
         //       /// </summary>

         //       /// <PARAM name="greg"></PARAM>

         //       /// <returns></returns>

         //       public bool IsGreg(string greg)
         //       {
         //               try
         //               {       
         //                       DateTime tempDate=DateTime.ParseExact(greg,allFormats,
         //                               enCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);

         //                       if (tempDate.Year>=startGreg && tempDate.Year<=endGreg)
         //                               return true;
         //                       else
         //                               return false;
         //               }
         //               catch (Exception ex)
         //               {
         //                      //cur.Trace.Warn("IsGreg Error :"+greg.ToString()+"\n"+ex.Message);
         //                       return false;
         //               }

         //       }

         //       /// <summary>
         //       /// Return Today Gregorian date and return it in yyyy/MM/dd format
         //       /// yyyy/MM/dd إرجاع تاريخ اليوم بالميلادي بشكل  
         //       /// </summary>

         //       /// <returns></returns>

         //       public string GDateNow()
         //       {
         //               try
         //               {
         //                       return DateTime.Now.ToString("yyyy/MM/dd",enCul.DateTimeFormat);
         //               }
         //               catch (Exception ex)
         //               {
         //                      // cur.Trace.Warn("GDateNow :\n"+ex.Message);
         //                       return "";
         //               }
         //       }
         //       /// <summary>
         //       /// Return formatted today Gregorian date based on your format
         //       ///  ارجاع تاريخ اليوم بالميلادي بالِشكل الذي تحدده  
         //       /// </summary>

         //       /// <PARAM name="format"></PARAM>

         //       /// <returns></returns>

         //       public string GDateNow(string format)
         //       {
         //               try
         //               {
         //                       return DateTime.Now.ToString(format,enCul.DateTimeFormat);
         //               }
         //               catch (Exception ex)
         //               {
         //                      // cur.Trace.Warn("GDateNow :\n"+ex.Message);
         //                       return "";
         //               }
         //       } 
                
         //       /// <summary>
         //       /// Return Today Hijri date and return it in yyyy/MM/dd format
         //       /// yyyy/MM/dd إرجاع تاريخ اليوم  بالهجري  بشكل  
         //       /// </summary>
         //       /// <returns></returns>

         //       public string HDateNow()
         //       {
         //               try
         //               {
         //                       return DateTime.Now.ToString("yyyy/MM/dd",arCul.DateTimeFormat);
         //               }
         //               catch (Exception ex)
         //               {
         //                       //cur.Trace.Warn("HDateNow :\n"+ex.Message);
         //                       return "";
         //               }
         //       }
         //       /// <summary>
         //       /// Return formatted today hijri date based on your format
         //       /// إرجاع تاريخ اليوم بالهجري بالِشكل الذي تحدده  
         //       /// </summary>
         //       /// <PARAM name="format"></PARAM>
         //       /// <returns></returns>


         //       public string HDateNow(string format)
         //       {
         //               try
         //               {
         //                       return DateTime.Now.ToString(format,arCul.DateTimeFormat);
         //               }
         //               catch (Exception ex)
         //               {
         //                      // cur.Trace.Warn("HDateNow :\n"+ex.Message);
         //                       return "";
         //               }
                        
         //       }
                
         //       /// <summary>
         //       /// Convert Hijri Date to it's equivalent Gregorian Date
         //       ///تحويل التاريخ الهجري إلى ما يكافئه بالتاريخ الميلادي
         //       /// </summary>
         //       /// <PARAM name="hijri"></PARAM>
         //       /// <returns></returns>

         //       public string HijriToGreg(string hijri)
         //       {
                        
         //               try
         //               {
         //                       DateTime tempDate=DateTime.ParseExact(hijri,allFormats,
         //                          arCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
         //                       return tempDate.ToString("yyyy/MM/dd",enCul.DateTimeFormat);
         //               }
         //               catch (Exception ex)
         //               {
         //                       //cur.Trace.Warn("HijriToGreg :"+hijri.ToString()+"\n"+ex.Message);
         //                       return "";
         //               }
         //       }
         //       /// <summary>
         //       /// Convert Hijri Date to it's equivalent Gregorian Date
         //       /// and return it in specified format
         //       /// تحويل التاريخ الهجري إلى ما يكافئه بالتاريخ الميلادي
         //       ///و إعادته بشكل محدد
         //       /// </summary>
         //       /// <PARAM name="hijri"></PARAM>
         //       /// <PARAM name="format"></PARAM>
         //       /// <returns></returns>

         //       public string HijriToGreg(string hijri,string format)
         //       {
         //               try
         //               {
         //                       DateTime tempDate=DateTime.ParseExact(hijri,
         //                          allFormats,arCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
         //                       return tempDate.ToString(format,enCul.DateTimeFormat);
                                
         //               }
         //               catch (Exception ex)
         //               {
         //                       cur.Trace.Warn("HijriToGreg :"+hijri.ToString()+"\n"+ex.Message);
         //                       return "";
         //               }
         //       }
         //       /// <summary>
         //       /// Convert Gregoian Date to it's equivalent Hijir Date
         //       /// تحويل التاريخ الميلادي إلى ما يكافئه بالتاريخ الهجري 
         //       /// </summary>
         //       /// <PARAM name="greg"></PARAM>
         //       /// <returns></returns>

         //       public string GregToHijri(string greg)
         //       {
                        
         //               try
         //               {
         //                       DateTime tempDate=DateTime.ParseExact(greg,allFormats,
         //                               enCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
         //                       return tempDate.ToString("yyyy/MM/dd",arCul.DateTimeFormat);
                                
         //               }
         //               catch (Exception ex)
         //               {
         //                       //cur.Trace.Warn("GregToHijri :"+greg.ToString()+"\n"+ex.Message);
         //                       return "";
         //               }
         //       }
         //       /// <summary>
         //       /// Convert Hijri Date to it's equivalent Gregorian Date and
         //       /// return it in specified format
         //       /// تحويل التاريخ الهجري إلى ما يكافئه بالتاريخ الميلادي
         //       ///و اعادته بشكل محدد
         //       /// </summary>
         //       /// <PARAM name="greg"></PARAM>
         //       /// <PARAM name="format"></PARAM>

         //       /// <returns></returns>

         //       public string GregToHijri(string greg,string format)
         //       {
                        
         //               try
         //               {
                                
         //                       DateTime tempDate=DateTime.ParseExact(greg,allFormats,
         //                               enCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
         //                       return tempDate.ToString(format,arCul.DateTimeFormat);
                                
         //               }
         //               catch (Exception ex)
         //               {
         //                       //cur.Trace.Warn("GregToHijri :"+greg.ToString()+"\n"+ex.Message);
         //                       return "";
         //               }
         //       }
                
                                
         //       /// <summary>
         //       /// Compare two instaces of string date 
         //       /// and return indication of thier values 
         //       ///مقارنة بين تاريخين (سلسة حرفية)
         //       /// </summary>

         // /// <PARAM name="d1"></PARAM>
         // /// <PARAM name="d2"></PARAM>
         // /// <returns>positive d1 is greater than d2,
         // /// negative d1 is smaller than d2, 0 both are equal</returns>
         // ///إرجاع قيمة موجبة إذا كانت القيمة الأولى أكبر من القيمة الثانية ، 
         // /// إرجاع قيمة صفر إذا كانت القيمتان متكافئتان ، إرجاع قيمة سالبة إذا 
         ///// كانت القيمة الأولى أصغر من القيمة الثانية 


         //               public int Compare(string d1,string d2)
         //       {
         //               try
         //               {
         //                       DateTime date1=DateTime.ParseExact(d1,allFormats,
         //                               arCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
         //                       DateTime date2=DateTime.ParseExact(d2,allFormats,
         //                               arCul.DateTimeFormat,DateTimeStyles.AllowWhiteSpaces);
         //                       return DateTime.Compare(date1,date2);
         //               }
         //               catch (Exception ex)
         //               {
         //                      // cur.Trace.Warn("Compare :"+"\n"+ex.Message);
         //                       return -1;
         //               }

         //       }

    }
}
