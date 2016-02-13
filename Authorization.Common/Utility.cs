using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
namespace Authorization.Common
{
	public class Utility : Page
	{
        #region ¼ì²éÊÇ·ñÎªIPµØÖ·
        /// <summary>
        /// ÊÇ·ñÎªip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion

        #region Ëæ»úÊý
        /// <summary>
        /// Ëæ»úÊý
        /// </summary>
        /// <param name="bit">Ëæ»úÊýÎ»Êý</param>
        /// <returns></returns>
        public static int Rad(int bit)
        {
            if (bit > 0)
            {
                int i = 10 ^ bit;
                Random r = new Random();
                return r.Next(i);
            }
            return 0;

        }
        public static string ChangeMoney(string money)
        {
            try
            {
                string s = double.Parse(money).ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");//d + "\n" +
                string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
                return Regex.Replace(d, ".", delegate(Match m) { return "¸ºÔª¿ÕÁãÒ¼·¡ÈþËÁÎéÂ½Æâ°Æ¾Á¿Õ¿Õ¿Õ¿Õ¿Õ¿Õ¿Õ·Ö½ÇÊ°°ÛÇªÍòÒÚÕ×¾©Ûòïöð¦"[m.Value[0] - '-'].ToString(); });
            }
            catch
            {
                return "";
            }

        }
        #endregion

		public static string ToObjectString(object _object)
		{
			if (_object != null)
			{
				return _object.ToString();
			}
			return string.Empty;
		}
		public static int ToInt(object _object)
		{
			int result;
			try
			{
				result = int.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = -1;
			}
			return result;
		}
		public static int ToInt(object _object, int returnValue)
		{
			int result;
			try
			{
				result = int.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static long ToLong(object _object)
		{
			long result;
			try
			{
				result = long.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = -1L;
			}
			return result;
		}
		public static long ToLong(object _object, long returnValue)
		{
			long result;
			try
			{
				result = long.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static decimal ToDecimal(object _object)
		{
			decimal result;
			try
			{
				result = decimal.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = -1m;
			}
			return result;
		}
		public static decimal ToDecimal(object _object, decimal returnValue)
		{
			decimal result;
			try
			{
				result = decimal.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static double ToDouble(object _object)
		{
			double result;
			try
			{
				result = double.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = -1.0;
			}
			return result;
		}
		public static double ToDouble(object _object, double returnValue)
		{
			double result;
			try
			{
				result = double.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static float ToFloat(object _object)
		{
			float result;
			try
			{
				result = float.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = -1f;
			}
			return result;
		}
		public static float ToFloat(object _object, float returnValue)
		{
			float result;
			try
			{
				result = float.Parse(Utility.ToObjectString(_object));
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static DateTime ToDateTime(object _object)
		{
			DateTime result;
			try
			{
				DateTime dateTime = DateTime.Parse(Utility.ToObjectString(_object));
				if (dateTime > DateTime.MinValue && DateTime.MaxValue > dateTime)
				{
					result = dateTime;
				}
				else
				{
					result = DateTime.Now;
				}
			}
			catch
			{
				result = DateTime.Now;
			}
			return result;
		}
		public static DateTime ToDateTime(object _object, DateTime returnValue)
		{
			DateTime result;
			try
			{
				DateTime dateTime = DateTime.Parse(Utility.ToObjectString(_object));
				if (dateTime > DateTime.MinValue && DateTime.MaxValue > dateTime)
				{
					result = dateTime;
				}
				else
				{
					result = returnValue;
				}
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static byte ToByteByBool(object _object)
		{
			string text = Utility.ToObjectString(_object).Trim();
			if (text == string.Empty)
			{
				return 0;
			}
			byte result;
			try
			{
				result = Convert.ToByte(((text.ToLower() == "true") ?  1 : 0));
			}
			catch
			{
				result = 0;
			}
			return result;
		}
		public static byte ToByteByBool(object _object, byte returnValue)
		{
			string text = Utility.ToObjectString(_object).Trim();
			if (text == string.Empty)
			{
				return returnValue;
			}
			byte result;
			try
			{
				result = Convert.ToByte((text.ToLower() == "true") ? 1 : 0);
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static bool ToBoolByByte(object _object)
		{
			bool result;
			try
			{
				result = (Utility.ToObjectString(_object).ToLower() == "1" || Utility.ToObjectString(_object).ToLower() == "true");
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool ToBoolByByte(object _object, bool returnValue)
		{
			bool result;
			try
			{
				result = (Utility.ToObjectString(_object).ToLower() == "1" || Utility.ToObjectString(_object).ToLower() == "true");
			}
			catch
			{
				result = returnValue;
			}
			return result;
		}
		public static bool IsEmpty(string _object)
		{
			return Utility.ToObjectString(_object).Trim() == string.Empty;
		}
		public static bool IsDateTime(object _object)
		{
			bool result;
			try
			{
				DateTime dateTime = DateTime.Parse(Utility.ToObjectString(_object));
				if (dateTime > DateTime.MinValue && DateTime.MaxValue > dateTime)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool IsInt(object _object)
		{
			bool result;
			try
			{
				Convert.ToInt32(Utility.ToObjectString(_object));
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool IsLong(object _object)
		{
			bool result;
			try
			{
				long.Parse(Utility.ToObjectString(_object));
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool IsFloat(object _object)
		{
			bool result;
			try
			{
				float.Parse(Utility.ToObjectString(_object));
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool IsDouble(object _object)
		{
			bool result;
			try
			{
				double.Parse(Utility.ToObjectString(_object));
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool IsDecimal(object _object)
		{
			bool result;
			try
			{
				decimal.Parse(Utility.ToObjectString(_object));
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static string StringTrimAll(string text)
		{
			string text2 = Utility.ToObjectString(text);
			string text3 = string.Empty;
			char[] array = text2.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ToString() != string.Empty)
				{
					text3 += array[i].ToString();
				}
			}
			return text3;
		}
		public static string NumricTrimAll(string numricString)
		{
			string text = Utility.ToObjectString(numricString).Trim();
			string text2 = string.Empty;
			char[] array = text.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ToString() == "+" || array[i].ToString() == "-" || Utility.IsDouble(array[i].ToString()))
				{
					text2 += array[i].ToString();
				}
			}
			return text2;
		}
		public static bool ArrayFind(Array array, object _object)
		{
			bool result = false;
			foreach (object current in array)
			{
				if (_object.Equals(current))
				{
					result = true;
					break;
				}
			}
			return result;
		}
		public static bool ArrayFind(Array array, string _object, bool unUpLower)
		{
			bool result = false;
			foreach (string text in array)
			{
				if (!unUpLower)
				{
					if (!_object.Trim().Equals(text.ToString().Trim()))
					{
						continue;
					}
					result = true;
				}
				else
				{
					if (!_object.Trim().ToUpper().Equals(text.ToString().Trim().ToUpper()))
					{
						continue;
					}
					result = true;
				}
				break;
			}
			return result;
		}
		public static string ReplaceInvertedComma(string inputString)
		{
			return inputString.Replace("'", "''");
		}
		public static bool CompareByteArray(byte[] bytea, byte[] byteb)
		{
			if (bytea == null || byteb == null)
			{
				return false;
			}
			int num = bytea.Length;
			int num2 = byteb.Length;
			if (num != num2)
			{
				return false;
			}
			bool result = true;
			for (int i = 0; i < num; i++)
			{
				if (bytea[i].CompareTo(byteb[i]) != 0)
				{
					result = false;
					return result;
				}
			}
			return result;
		}
		public static string BuildDate(string inputText)
		{
			string result;
			try
			{
				result = DateTime.Parse(inputText).ToShortDateString();
			}
			catch
			{
				string text = Utility.NumricTrimAll(inputText);
				string text2 = DateTime.Now.Year.ToString();
				string text3 = DateTime.Now.Month.ToString();
				string text4 = DateTime.Now.Day.ToString();
				int length = text.Length;
				if (length != 0)
				{
					if (length <= 2)
					{
						text4 = text;
					}
					else
					{
						if (length <= 4)
						{
							text3 = text.Substring(0, 2);
							text4 = text.Substring(2, length - 2);
						}
						else
						{
							if (length <= 6)
							{
								text2 = text.Substring(0, 4);
								text3 = text.Substring(4, length - 4);
							}
							else
							{
								if (length > 6)
								{
									text2 = text.Substring(0, 4);
									text3 = text.Substring(4, 2);
									text4 = text.Substring(6, length - 6);
								}
							}
						}
					}
					try
					{
						result = DateTime.Parse(string.Concat(new string[]
						{
							text2,
							"-",
							text3,
							"-",
							text4
						})).ToShortDateString();
						return result;
					}
					catch
					{
						result = string.Empty;
						return result;
					}
				}
				result = string.Empty;
			}
			return result;
		}
		public static bool IsFileExists(string path)
		{
			bool result;
			try
			{
				result = File.Exists(path);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool IsDirectoryExists(string path)
		{
			bool result;
			try
			{
				result = Directory.Exists(Path.GetDirectoryName(path));
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool FindLineTextFromFile(FileInfo fi, string lineText, bool lowerUpper)
		{
			bool result = false;
			try
			{
				if (fi.Exists)
				{
					StreamReader streamReader = new StreamReader(fi.FullName);
					do
					{
						string @object = streamReader.ReadLine();
						if (lowerUpper)
						{
							if (Utility.ToObjectString(@object).Trim() == Utility.ToObjectString(lineText).Trim())
							{
								goto Block_5;
							}
						}
						else
						{
							if (Utility.ToObjectString(@object).Trim().ToLower() == Utility.ToObjectString(lineText).Trim().ToLower())
							{
								goto IL_79;
							}
						}
					}
					while (streamReader.Peek() != -1);
					goto IL_7B;
					Block_5:
					result = true;
					goto IL_7B;
					IL_79:
					result = true;
					IL_7B:
					streamReader.Close();
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public static bool IsRightParent(DataTable table, string columnName, string parentColumnName, string inputString, string compareString)
		{
			ArrayList arrayList = new ArrayList();
			Utility.smethod_0(arrayList, table, columnName, parentColumnName, inputString, compareString);
			return arrayList.Count == 0;
		}
		private static void smethod_0(ArrayList arrayList_0, DataTable dataTable_0, string string_0, string string_1, string string_2, string string_3)
		{
			DataView dataView = new DataView(dataTable_0);
			dataView.RowFilter = string_1 + "='" + Utility.ReplaceInvertedComma(string_2.Trim()) + "'";
			for (int i = 0; i < dataView.Count; i++)
			{
				if (Utility.ToObjectString(dataView[i][string_0]).ToLower() == string_3.Trim().ToLower())
				{
					arrayList_0.Add("1");
					return;
				}
				Utility.smethod_0(arrayList_0, dataTable_0, string_0, string_1, Utility.ToObjectString(dataView[i][string_0]), string_3);
			}
		}
		public static string Fomatdate(DateTime dtime, string s)
		{
			return string.Concat(new object[]
			{
				dtime.Year,
				s,
				dtime.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'),
				s,
				dtime.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0')
			});
		}
		public static int Datediff(DateTime sdmin, DateTime sdmax)
		{
			int result;
			try
			{
				double num = 0.0;
				while (sdmin.AddDays(num) < sdmax)
				{
					num += 1.0;
				}
				result = Utility.ToInt(num);
			}
			catch
			{
				result = -1;
			}
			return result;
		}
		public static int Datediff(string sdmin, string sdmax)
		{
			int result;
			try
			{
				DateTime dateTime = DateTime.Parse(sdmin);
				DateTime t = DateTime.Parse(sdmax);
				double num = 0.0;
				while (dateTime.AddDays(num) < t)
				{
					num += 1.0;
				}
				result = Utility.ToInt(num);
			}
			catch
			{
				result = -1;
			}
			return result;
		}
		public static string ConvertStr(string inputString)
		{
			string text = inputString.Replace("\"", "&quot;");
			text = text.Replace("<", "&lt;");
			text = text.Replace(">", "&gt;");
			text = text.Replace(" ", "&nbsp;");
			text = text.Replace("  ", "&nbsp;&nbsp;");
			text = text.Replace("\t", "&nbsp;&nbsp;");
			return text.Replace("\r", "<br>");
		}
		public static string InputText(string inputString)
		{
			string text = Utility.ConvertStr(inputString);
			text = text.Replace("[url]", string.Empty);
			return text.Replace("[/url]", string.Empty);
		}
		public static string OutputText(string inputString)
		{
			string text = HttpUtility.HtmlDecode(inputString);
			text = text.Replace("<br>", string.Empty);
			text = text.Replace("&amp", "&;");
			text = text.Replace("&quot;", "\"");
			text = text.Replace("&lt;", "<");
			text = text.Replace("&gt;", ">");
			text = text.Replace("&nbsp;", " ");
			return text.Replace("&nbsp;&nbsp;", "  ");
		}
		public static string ToUrl(string inputString)
		{
			string input = Utility.ConvertStr(inputString);
			return Regex.Replace(input, "\\[url](?<x>[^\\]]*)\\[/url]", "<a href=\"$1\" target=\"_blank\">$1</a>", RegexOptions.IgnoreCase);
		}
		public static string GetSafeCode(string str)
		{
			str = str.Replace("'", string.Empty);
			str = str.Replace(char.Parse("34"), ' ');
			str = str.Replace(";", string.Empty);
			return str;
		}
		public static void SetTableRows(DataTable myDataTable, int intPageCount)
		{
			int num = myDataTable.Rows.Count % intPageCount;
			if (myDataTable.Rows.Count == 0 || num != 0)
			{
				for (int i = 0; i < intPageCount - num; i++)
				{
					DataRow row = myDataTable.NewRow();
					myDataTable.Rows.Add(row);
				}
			}
		}
		public static string GetGuid(string guid)
		{
			return guid.Replace("-", string.Empty);
		}
		public static string ReadConfig(string filePath)
		{
			return ConfigurationManager.AppSettings[filePath];
		}
		public static string GetSubString(string str, int length)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < str.Length; i++)
			{
				if (Regex.IsMatch(str.Substring(i, 1), "[\\u4e00-\\u9fa5]+"))
				{
					num += 2;
				}
				else
				{
					num++;
				}
				if (num <= length)
				{
					num2++;
				}
				if (num > length)
				{
					return str.Substring(0, num2) + "...";
				}
			}
			return str;
		}
	}
}
