//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Authorization.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class sys_Dictionary
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Describe { get; set; }
        public int SortIndex { get; set; }
        public bool IsUsing { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUserName { get; set; }
        public string CreateRealName { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyUserName { get; set; }
        public string ModifyRealName { get; set; }
        public int sys_DictionaryType_Id { get; set; }
    
        public virtual sys_DictionaryType sys_DictionaryType { get; set; }
    }
}
