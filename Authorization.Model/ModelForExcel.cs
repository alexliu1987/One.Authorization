using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Model
{
    public class DictionaryExcel
    {
        public string 字典值 { get; set; }
        public string 字典类型 { get; set; }
        public int 排序 { get; set; }
        public string 描述 { get; set; }
        public DateTime 创建时间 { get; set; }
    }
    public class MemberExcel
    {
        public string 用户名 { get; set; }
        public string 姓名 { get; set; }
        public string 性别 { get; set; }
        public string 邮箱 { get; set; }
        public string 身份证 { get; set; }
        public string 手机 { get; set; }
        public string QQ { get; set; }
        public DateTime? 生日 { get; set; }

    }
    public class LoginLogExcel
    {
        public DateTime 登录时间 { get; set; }
        public int 用户编号 { get; set; }
        public string 姓名 { get; set; }
        public string IP地址 { get; set; }
    }
    public class OperLogExcel
    {
        public DateTime 操作时间 { get; set; }
        public int 用户编号 { get; set; }
        public string 姓名 { get; set; }
        public string IP地址 { get; set; }
        public string 操作类型 { get; set; }
        public string 操作内容 { get; set; }
    }
    public class ErrorLogExcel
    {
        public DateTime 操作时间 { get; set; }
        public int 用户编号 { get; set; }
        public string 姓名 { get; set; }
        public string IP地址 { get; set; }

        public string 日志内容 { get; set; }
    }
    public class ActionExcel
    {
        public string 功能名称 { get; set; }
        public string 功能标识 { get; set; }
        public string 图标 { get; set; }
        public string 按钮类型 { get; set; }
        public bool 是否可用 { get; set; }
        public int 排序 { get; set; }
        public string 描述 { get; set; }
    }
    public class MenuExcel
    {
        public string 菜单ID { get; set; }
        public string 菜单名称 { get; set; }
        public string 菜单标识 { get; set; }
        public string 上级菜单ID { get; set; }
        public string 图标 { get; set; }
        public string 连接 { get; set; }
        public bool 是否可用 { get; set; }
        public bool 有无范围 { get; set; }
        public int 排序 { get; set; }
        public string 描述 { get; set; }
    }
    public class DictionaryTypeExcel
    {
        public string 字典类型名称 { get; set; }
        public bool 是否可用 { get; set; }
        public int 排序 { get; set; }
        public string 描述 { get; set; }
    }
    public class ForbidIpExcel
    {
        public string IP { get; set; }
        public bool 是否可用 { get; set; }
        public string 描述 { get; set; }
        public DateTime 操作时间 { get; set; }
        public string 操作人 { get; set; }
    }
    public class AppExcel
    {
        public string 模块名称 { get; set; }
        public string 模块标识 { get; set; }
        public string 图标 { get; set; }
        public bool 是否可用 { get; set; }
        public int 排序 { get; set; }
        public string 描述 { get; set; }
    }
}
