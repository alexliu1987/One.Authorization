using System;

namespace Authorization.Common.Enum
{
    public static class EnumHelper
    {
        public static T ParseEnumByString<T>(string enumString) where T : struct
        {
            if (!string.IsNullOrEmpty(enumString) && typeof(T).IsEnum)
            {
                return (T)((object)System.Enum.Parse(typeof(T), enumString));
            }
            return default(T);
        }
        public static T ParseEnumByDesc<T>(string enumDesc) where T : struct
        {
            EnumDescription[] fieldTexts = EnumDescription.GetFieldTexts(typeof(T), SortType.Default);
            EnumDescription[] array = fieldTexts;
            for (int i = 0; i < array.Length; i++)
            {
                EnumDescription enumDescription = array[i];
                if (enumDescription.EnumDisplayText.Equals(enumDesc))
                {
                    return EnumHelper.ParseEnumByString<T>(enumDescription.FieldName);
                }
            }
            return default(T);
        }
        public static int ParseEnumValueByString<T>(string enumString) where T : struct
        {
            EnumDescription[] fieldTexts = EnumDescription.GetFieldTexts(typeof(T), SortType.Default);
            EnumDescription[] array = fieldTexts;
            for (int i = 0; i < array.Length; i++)
            {
                EnumDescription enumDescription = array[i];
                if (enumDescription.FieldName.Equals(enumString))
                {
                    return enumDescription.EnumValue;
                }
            }
            return -1;
        }
        public static int ParseEnumValueByDesc<T>(string enumDesc) where T : struct
        {
            EnumDescription[] fieldTexts = EnumDescription.GetFieldTexts(typeof(T), SortType.Default);
            EnumDescription[] array = fieldTexts;
            for (int i = 0; i < array.Length; i++)
            {
                EnumDescription enumDescription = array[i];
                if (enumDescription.EnumDisplayText.Equals(enumDesc))
                {
                    return enumDescription.EnumValue;
                }
            }
            return -1;
        }
    }
}
