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
    
    public partial class sys_Member
    {
        public sys_Member()
        {
            this.sys_Role = new HashSet<sys_Role>();
        }
    
        public int id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Email { get; set; }
        public Nullable<bool> IsUsing { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string RoleRightStr { get; set; }
        public string RoleOperResStr { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateUserName { get; set; }
        public string CreateRealName { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyUserName { get; set; }
        public string ModifyRealName { get; set; }
        public int sys_MemberExtend_Id { get; set; }
        public int sys_Orgnization_Id { get; set; }
    
        public virtual sys_MemberExtend sys_MemberExtend { get; set; }
        public virtual sys_Orgnization sys_Orgnization { get; set; }
        public virtual ICollection<sys_Role> sys_Role { get; set; }
    }
}
