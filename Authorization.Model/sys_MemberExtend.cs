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
    
    public partial class sys_MemberExtend
    {
        public sys_MemberExtend()
        {
            this.sys_Member = new HashSet<sys_Member>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string IdCard { get; set; }
        public string QQ { get; set; }
        public string Tel { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public string LastLoginIP { get; set; }
        public bool IsOnline { get; set; }
    
        public virtual ICollection<sys_Member> sys_Member { get; set; }
    }
}
