using System;
using System.Collections;
using System.Reflection;

namespace Authorization.Common.Enum
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class EnumDescription : Attribute
    {
        private readonly string string_0;
        private readonly int int_0;
        private FieldInfo fieldInfo_0;
        private static readonly Hashtable hashtable_0 = new Hashtable();
        public string EnumDisplayText
        {
            get
            {
                return this.string_0;
            }
        }
        public int EnumRank
        {
            get
            {
                return this.int_0;
            }
        }
        public int EnumValue
        {
            get
            {
                return (int)this.fieldInfo_0.GetValue(null);
            }
        }
        public string FieldName
        {
            get
            {
                return this.fieldInfo_0.Name;
            }
        }
        public EnumDescription(string enumDisplayText, int enumRank)
        {
            this.string_0 = enumDisplayText;
            this.int_0 = enumRank;
        }
        public EnumDescription(string enumDisplayText)
            : this(enumDisplayText, 5)
        {
        }
        public static string GetEnumText(Type enumType)
        {
            EnumDescription[] array = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            if (array.Length != 1)
            {
                return string.Empty;
            }
            return array[0].EnumDisplayText;
        }
        public static string GetFieldText(object enumValue)
        {
            EnumDescription[] fieldTexts = EnumDescription.GetFieldTexts(enumValue.GetType(), SortType.Default);
            EnumDescription[] array = fieldTexts;
            for (int i = 0; i < array.Length; i++)
            {
                EnumDescription enumDescription = array[i];
                if (enumDescription.fieldInfo_0.Name == enumValue.ToString())
                {
                    return enumDescription.EnumDisplayText;
                }
            }
            return string.Empty;
        }
        public static EnumDescription[] GetFieldTexts(Type enumType)
        {
            return EnumDescription.GetFieldTexts(enumType, SortType.Default);
        }
        public static EnumDescription[] GetFieldTexts(Type enumType, SortType sortType)
        {
            if (!EnumDescription.hashtable_0.Contains(enumType.FullName))
            {
                FieldInfo[] fields = enumType.GetFields();
                ArrayList arrayList = new ArrayList();
                FieldInfo[] array = fields;
                for (int i = 0; i < array.Length; i++)
                {
                    FieldInfo fieldInfo = array[i];
                    object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(EnumDescription), false);
                    if (customAttributes.Length == 1)
                    {
                        ((EnumDescription)customAttributes[0]).fieldInfo_0 = fieldInfo;
                        arrayList.Add(customAttributes[0]);
                    }
                }
                EnumDescription.hashtable_0.Add(enumType.FullName, arrayList.ToArray(typeof(EnumDescription)));
            }
            EnumDescription[] array2 = (EnumDescription[])EnumDescription.hashtable_0[enumType.FullName];
            if (array2.Length <= 0)
            {
                int num = 0;
                while (num < array2.Length && sortType != SortType.Default)
                {
                    for (int j = num; j < array2.Length; j++)
                    {
                        bool flag = false;
                        switch (sortType)
                        {
                            case SortType.DisplayText:
                                if (string.CompareOrdinal(array2[num].EnumDisplayText, array2[j].EnumDisplayText) > 0)
                                {
                                    flag = true;
                                }
                                break;
                            case SortType.Rank:
                                if (array2[num].EnumRank > array2[j].EnumRank)
                                {
                                    flag = true;
                                }
                                break;
                        }
                        if (flag)
                        {
                            EnumDescription enumDescription = array2[num];
                            array2[num] = array2[j];
                            array2[j] = enumDescription;
                        }
                    }
                    num++;
                }
            }
            return array2;
        }
    }
}
