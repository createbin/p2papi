using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ZFCTAPI.Core.ComponentModel;
using ZFCTAPI.Core.Enums;

namespace ZFCTAPI.Core.Helpers
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial class CommonHelper
    {
        #region Fields

        private static readonly Regex _emailRegex;

        //we use EmailValidator from FluentValidation. So let's keep them sync - https://github.com/JeremySkinner/FluentValidation/blob/master/src/FluentValidation/Validators/EmailValidator.cs
        private const string _emailExpression = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";

        #endregion Fields

        #region Ctor

        static CommonHelper()
        {
            _emailRegex = new Regex(_emailExpression, RegexOptions.IgnoreCase);
        }

        #endregion Ctor

        #region Methods

        /// <summary>
        /// 确定用户填写的邮件地址
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static string EnsureSubscriberEmailOrThrow(string email)
        {
            var output = EnsureNotNull(email);
            output = output.Trim();
            output = EnsureMaximumLength(output, 255);

            if (!IsValidEmail(output))
            {
                throw new Exception("Email is not valid.");
            }

            return output;
        }

        /// <summary>
        /// 判定邮箱是否合法
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();

            return _emailRegex.IsMatch(email);
        }

        /// <summary>
        /// 验证ip地址是否合法
        /// </summary>
        /// <param name="ipAddress">IPAddress to verify</param>
        /// <returns>true if the string is a valid IpAddress and false if it's not</returns>
        public static bool IsValidIpAddress(string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out IPAddress _);
        }

        /// <summary>
        /// 随机生成数字
        /// </summary>
        /// <param name="length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
                str = string.Concat(str, random.Next(10).ToString());
            return str;
        }

        /// <summary>
        /// 随机生成流水号
        /// </summary>
        /// <returns></returns>
        public static string GetMchntTxnSsn()
        {
            Random rad = new Random();//实例化随机数产生器rad；
            string value = rad.Next(100, 1000).ToString();//用rad生成大于等于1000，小于等于9999的随机数
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + value;
        }
        /// <summary>
        /// 随机生成批次号
        /// </summary>
        /// <returns></returns>
        public static string GetBatchNo()
        {
            Random rad = new Random();//实例化随机数产生器rad；
            string value = rad.Next(100, 1000).ToString();//用rad生成大于等于100，小于等于999的随机数
            return DateTime.Now.ToString("mmssfff") + value;
        }
        /// <summary>
        /// 生成一个最小值和最大值得限定
        /// </summary>
        /// <param name="min">Minimum number</param>
        /// <param name="max">Maximum number</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(int min = 0, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public static string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var pLen = postfix == null ? 0 : postfix.Length;

                var result = str.Substring(0, maxLength - pLen);
                if (!string.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }

            return str;
        }

        /// <summary>
        /// Ensures that a string only contains numeric values
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Input string with only numeric values, empty string if input is null/empty</returns>
        public static string EnsureNumericOnly(string str)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : new string(str.Where(p => char.IsDigit(p)).ToArray());
        }

        /// <summary>
        /// Ensure that a string is not null
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result</returns>
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// Indicates whether the specified strings are null or empty strings
        /// </summary>
        /// <param name="stringsToValidate">Array of strings to validate</param>
        /// <returns>Boolean</returns>
        public static bool AreNullOrEmpty(params string[] stringsToValidate)
        {
            return stringsToValidate.Any(p => string.IsNullOrEmpty(p));
        }

        /// <summary>
        /// Compare two arrasy
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="a1">Array 1</param>
        /// <param name="a2">Array 2</param>
        /// <returns>Result</returns>
        public static bool ArraysEqual<T>(T[] a1, T[] a2)
        {
            //also see Enumerable.SequenceEqual(a1, a2);
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 == null || a2 == null)
                return false;

            if (a1.Length != a2.Length)
                return false;

            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; i < a1.Length; i++)
            {
                if (!comparer.Equals(a1[i], a2[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Sets a property on an object to a valuae.
        /// </summary>
        /// <param name="instance">The object whose property to set.</param>
        /// <param name="propertyName">The name of the property to set.</param>
        /// <param name="value">The value to set the property to.</param>
        public static void SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            var instanceType = instance.GetType();
            var pi = instanceType.GetProperty(propertyName);
            if (pi == null)
                throw new Exception("No property '{0}' found on the instance of type '{1}'.");
            if (!pi.CanWrite)
                throw new Exception("The property '{0}' on the instance of type '{1}' does not have a setter.");
            if (value != null && !value.GetType().IsAssignableFrom(pi.PropertyType))
                value = To(value, pi.PropertyType);
            pi.SetValue(instance, value, new object[0]);
        }

        public static TypeConverter GetNopCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();
            return TypeDescriptor.GetConverter(type);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                var destinationConverter = TypeDescriptor.GetConverter(destinationType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);

                var sourceConverter = TypeDescriptor.GetConverter(sourceType);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);

                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);

                if (!destinationType.IsInstanceOfType(value))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }

        /// <summary>
        /// Convert enum for front-end
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Converted string</returns>
        public static string ConvertEnum(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var result = string.Empty;
            foreach (var c in str)
                if (c.ToString() != c.ToString().ToLower())
                    result += " " + c.ToString();
                else
                    result += c.ToString();

            //ensure no spaces (e.g. when the first letter is upper case)
            result = result.TrimStart();
            return result;
        }

        /// <summary>
        /// Set Telerik (Kendo UI) culture
        /// </summary>
        public static void SetTelerikCulture()
        {
            //little hack here
            //always set culture to 'en-US' (Kendo UI has a bug related to editing decimal values in other cultures)
            var culture = new CultureInfo("en-US");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        /// <summary>
        /// Get difference in years
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //source: http://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-in-c
            //this assumes you are looking for the western idea of age and not using East Asian reckoning.
            var age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
                age--;
            return age;
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(BaseDirectory ?? string.Empty, path);
        }

        /// <summary>
        /// Get private fields property value
        /// </summary>
        /// <param name="target">Target object</param>
        /// <param name="fieldName">Feild name</param>
        /// <returns>Value</returns>
        public static object GetPrivateFieldValue(object target, string fieldName)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target", "The assignment target cannot be null.");
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("fieldName", "The field name cannot be null or empty.");
            }

            var t = target.GetType();
            FieldInfo fi = null;

            while (t != null)
            {
                fi = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

                if (fi != null) break;

                t = t.BaseType;
            }

            if (fi == null)
            {
                throw new Exception($"Field '{fieldName}' not found in type hierarchy.");
            }

            return fi.GetValue(target);
        }

        /// <summary>
        ///  Depth-first recursive delete, with handling for descendant directories open in Windows Explorer.
        /// </summary>
        /// <param name="path">Directory path</param>
        public static void DeleteDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(path);

            //find more info about directory deletion
            //and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true

            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// 转换日期显示格式
        /// </summary>
        /// <returns></returns>
        public static string FormatDate(DateTime? dateStart, DateTime? dateEnd)
        {
            string strDate = string.Empty;
            string strStart = string.Format("{0:d}", dateStart).Replace('/', '-');
            string strEnd = string.Format("{0:d}", dateEnd).Replace('/', '-');
            strDate = strStart + "~" + strEnd;
            return strDate;
        }

        /// <summary>
        /// 字典类转参数1=2&3=4
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string NameValueCollectionToString(NameValueCollection model)
        {
            var result = string.Empty;
            var stringBuilder = new StringBuilder();
            foreach (var key in model.AllKeys)
            {
                stringBuilder.Append(key + "=" + model[key] + "&");
            }
            result = string.IsNullOrEmpty(stringBuilder.ToString()) ? stringBuilder.ToString().Substring(0, stringBuilder.ToString().Length - 1) : stringBuilder.ToString();
            return result;
        }

        /// <summary>
        /// 对象转键值集合
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isIgnoreNull"></param>
        /// <returns></returns>
        public static NameValueCollection ObjectToNameValueCollection(object obj, bool isIgnoreNull = false)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();

                if (m != null && m.IsPublic)
                {
                    if (m.Invoke(obj, new object[] { }) != null || !isIgnoreNull)
                    {
                        nameValueCollection.Add(p.Name, m.Invoke(obj, new object[] { }).ToString());
                    }
                }
            }
            return nameValueCollection;
        }

        /// <summary>
        /// 对象转键值集合
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isIgnoreNull"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObjectToDictionary(object obj, bool isIgnoreNull = false)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();

                if (m != null && m.IsPublic)
                {
                    if (m.Invoke(obj, new object[] { }) != null || !isIgnoreNull)
                    {
                        dictionary.Add(p.Name, m.Invoke(obj, new object[] { }).ToString());
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// 时间转换时间戳
        /// </summary>
        /// <returns></returns>
        public static int GenerateTimeStamp()//创建时间戳
        {
            DateTime dateStart = new DateTime(1970, 1, 1, 8, 0, 0);//格林威治时间1970，1，1，0，0，0
            return (int)(DateTime.Now - dateStart).TotalSeconds;
        }

        /// <summary>
        /// 生成指定长度的字符串
        /// </summary>
        /// <param name="count">字符串长度</param>
        /// <returns></returns>
        public static string CreateNonceStr(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串
            System.Random random = new Random();
            for (int i = 0; i < count; i++) //产生4位校验码
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                    number += 48;    //数字0-9编码在48-57
                else
                    number += 55;    //字母A-Z编码在65-90
                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        /// <summary>
        /// 创建结算平台号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string CreatePlatFormId(int id, UserAttributes attr)
        {
            switch (attr)
            {
                case UserAttributes.Invester:
                    return "10000" + id;
                case UserAttributes.Financer:
                    return "20000" + id;
                default:
                    return "";
            }
        }

        public static string SurplusPlatFormId(string formId)
        {
            if (string.IsNullOrEmpty(formId))
            {
                return "";
            }
            else
            {
                return formId.Substring(formId.Length - 4);
            }
        }

        /// <summary>
        /// 生成红包发放订单号
        /// </summary>
        /// <returns></returns>
        public static string RedBactOrderNo()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(100, 1000).ToString();
        }


        public static DateTime HandleStringTime(string dateInfo)
        {
            if (!string.IsNullOrEmpty(dateInfo)&&dateInfo.Length == 8)
            {
                var endTimeYear = dateInfo.Substring(0, 4);
                var endTimeMonth = dateInfo.Substring(4, 2);
                var endTimeDay = dateInfo.Substring(6, 2);
                var endTime = endTimeYear + "-" + endTimeMonth + "-" + endTimeDay;
                return Convert.ToDateTime(endTime);
            }
            else
            {
                return DateTime.Now;
            }
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets or sets application base path
        /// </summary>
        internal static string BaseDirectory { get; set; }

        #endregion Properties


        #region 

        #region 四舍五入处理
        /// <summary>
        /// 四舍五入处理
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="d">1.需要处理的数据</param>
        /// <param name="i">2.位数</param>
        /// <returns>四舍五入后的值</returns>
        public static decimal Round(decimal d, int i)
        {
            if (d >= 0)
            {
                d += (decimal)(5 * Math.Pow(10, -(i + 1)));
            }
            else
            {
                d += (decimal)(-5 * Math.Pow(10, -(i + 1)));
            }
            string str = d.ToString();
            string[] strs = str.Split('.');
            int idot = str.IndexOf('.');
            string prestr = strs[0];
            string poststr = strs[1];
            if (poststr.Length > i)
            {
                poststr = str.Substring(idot + 1, i);
            }
            string strd = prestr + "." + poststr;
            d = Decimal.Parse(strd);
            return d;
        }

        /// <summary>
        /// 金额类型转换为整型
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="d">decimal 型数据</param>
        /// <returns>int 型数据</returns>
        public static int RoundInt(decimal d)
        {
            return (int)Round(d, 0);
        }

        #endregion

        #region 截取小数位
        /// <summary>
        /// 截取小数位
        /// 创建者：吕小东
        /// 创建时间：2014年9月25日13:06:30
        /// </summary>
        /// <param name="decInterest">金额</param>
        /// <param name="nComma">类型</param>
        /// <returns>结果</returns>
        public static string getInterest(Decimal decInterest, int nComma)
        {
            //取整截尾，四舍五入到整，到分四舍五入
            if (nComma == 1)
            {
                return decInterest.ToString("#,###.#0");
            }
            else
            {
                return decInterest.ToString("###.#0");
            }
        }
        #endregion

        /// <summary>
        /// 根据日期获取年龄
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        public static int GetAgeByDate(DateTime birthDate)
        {
            var now = DateTime.Now;
            var age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
        /// <summary>
        /// 根据身份证获取性别
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns>true:女 false:男</returns>
        public static bool GetSexByIdCard(string idCard)
        {
            var sex = idCard.Substring(idCard.Length == 18 ? 14 : 12, 3);
            return int.Parse(sex) % 2 == 0;
        }

        /// <summary>
        /// 获取一个枚举值的中文描述
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <returns></returns>
        public static string GetEnumDescription(System.Enum obj)
        {
            FieldInfo fi = obj.GetType().GetField(obj.ToString());
            if (fi == null)
                return null;
            DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return arrDesc[0].Description;
        }

        public static string CreateCompanyName()
        {
            return "zfco" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }


        public static int DateDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateStart.ToShortDateString());

            TimeSpan sp = end.Subtract(start);

            return sp.Days;
        }

        #endregion
    }
}