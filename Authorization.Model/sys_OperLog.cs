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
    
    public partial class sys_OperLog
    {
        public int Id { get; set; }
        public string OperType { get; set; }
        public string OperContent { get; set; }
        public string ModuleName { get; set; }
        public int Uid { get; set; }
        public string RealName { get; set; }
        public string Address { get; set; }
        public string Ip { get; set; }
        public string Mac { get; set; }
        public System.DateTime LogTime { get; set; }
    }
}