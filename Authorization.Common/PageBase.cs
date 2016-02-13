using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FineUI;
using Authorization.Services;
using Authorization.Model;
using Authorization.Common;
namespace Authorization.Common
{
    public class PageBase : System.Web.UI.Page
    {
        #region 定义
        protected string BasePageName;
        protected System.Web.UI.Page BasePage;
        protected Nullable<DateTime> DateTimeNullValue = null;
        protected Window BaseWindow;
        /// <summary>
        /// 工具栏 用于生成Btn
        /// </summary>
        protected Toolbar BaseToolBar;
        /// <summary>
        /// Button单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BtnClick(object sender, System.EventArgs e) { }
        /// <summary>
        /// TwinTriggerBox按钮1单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TwinTriggerBoxClick1(object sender, System.EventArgs e) { }
        /// <summary>
        /// TwinTriggerBox按钮2单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void TwinTriggerBoxClick2(object sender, System.EventArgs e) { }
        private MenuService _baseMenuService;
        protected MenuService BaseMenuService
        {
            get { return _baseMenuService ?? (_baseMenuService = new MenuService()); }
        }
        private string PageUrl
        {
            get { return GetUrl(); }
        }
        /// <summary>
        /// 页面菜单信息
        /// </summary>
        private sys_Menu _basePageSysMenu;
        /// <summary>
        /// 获取页面对应的菜单信息
        /// </summary>
        private sys_Menu BasePageSysMenu
        {
            get { return _basePageSysMenu ?? (_basePageSysMenu = BaseMenuService.FirstOrDefault(p => p.Url.Contains(PageUrl))); }
        }
        /// <summary>
        /// 用户数据服务
        /// </summary>
        private MemberService _memberService;
        /// <summary>
        /// 获取用户数据服务
        /// </summary>
        private MemberService MemberService
        {
            get { return _memberService ?? (_memberService = new MemberService()); }
        }
        protected TwinTriggerBox BASEttb { get; set; }

        #endregion

        #region 页面加载
        /// <summary>
        /// 页面初始化
        /// </summary>
        protected virtual void Init_Page() { }
        /// <summary>
        /// 按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Init_Page();
            if (BasePageSysMenu != null && !BaseRightStr.Contains(string.Format("{0}-0", BasePageSysMenu.MenuCode)))
            {
                Response.Write("您没权限访问该页面！");
                Response.End();
                return;
            }          
            InitPageControl();
            if (BasePage != null && BaseToolBar == null)
                CheckControlsRight(BasePage);
        }
        #endregion

        #region 请求参数
        /// <summary>
        /// 是否带参数
        /// </summary>
        /// <param name="queryKey"></param>
        /// <returns></returns>
        protected bool CheckQueryKey(string queryKey)
        {
            object o = Request.QueryString[queryKey];
            if (o != null && o.ToString() != string.Empty)
                return true;
            else
                return false;

        }


        /// <summary>
        /// 获取查询字符串中的参数值
        /// </summary>
        protected string GetQueryValue(string queryKey)
        {
            return Request.QueryString[queryKey];
        }


        /// <summary>
        /// 获取查询字符串中的参数值
        /// </summary>
        protected int GetQueryIntValue(string queryKey)
        {
            int queryIntValue = -1;
            try
            {
                queryIntValue = Convert.ToInt32(Request.QueryString[queryKey]);
            }
            catch (Exception)
            {
                // TODO
            }

            return queryIntValue;
        }

        #endregion

        #region 生成控件和控件权限控件

        private void InitPageControl()
        {
            if (BasePageSysMenu == null)
                return;
            sys_Menu m = BasePageSysMenu;
            foreach (sys_Action a in m.sys_Action.OrderBy(p => p.SortIndex))
            {
                bool enable = BaseRightStr.Contains(string.Format("{0}-{1}", m.MenuCode, a.ActionCode));
                switch (a.ActionType)
                {
                    case "ToolBarButton":
                        CreateToolBarButton(a, enable);
                        break;
                    case "ToolBarTwinTriggerBox":
                        CreateToolBarTwinTriggerBox(a, enable);
                        break;
                }
            }

        }
        private void CreateToolBarButton(sys_Action a, bool enable)
        {
            if (BaseToolBar != null && a != null)
            {
                Button btn = new Button();
                btn.ID = string.Format("btn{0}", a.ActionCode);
                var defaulttype = AuthorizationButtonType.DEFAULT;
                AuthorizationButtonType btnType;
                if (System.Enum.IsDefined(typeof(AuthorizationButtonType), a.ActionCode.ToUpper()))
                    btnType = (AuthorizationButtonType)System.Enum.Parse(typeof(AuthorizationButtonType), a.ActionCode.ToUpper());
                else
                    btnType = defaulttype;
                btn.Icon = (Icon)(int)btnType;
                btn.Text = a.ActionName;
                btn.Click += new EventHandler(BtnClick);
                if (!enable)
                {
                    btn.ToolTip = "您没有权限操作！";
                    btn.Enabled = false;
                }
                switch (btnType)
                {
                    case AuthorizationButtonType.EXPORT:
                        btn.EnableAjax = false;
                        btn.DisableControlBeforePostBack = false;
                        break;
                    case AuthorizationButtonType.DELETE:
                        btn.ConfirmText = "您确定要删除所选数据吗？";
                        break;
                }

                BaseToolBar.Items.Add(btn);
            }
        }
        private void CreateToolBarTwinTriggerBox(sys_Action a, bool enable)
        {
            if (BaseToolBar != null && a != null)
            {
                TwinTriggerBox ttb = new TwinTriggerBox();
                ttb.ID = string.Format("ttb{0}", a.ActionCode);
                ttb.EmptyText = "请输入搜索内容";
                ttb.Trigger1Icon = TriggerIcon.Clear;
                ttb.Trigger2Icon = TriggerIcon.Search;
                ttb.Width = 200;
                ttb.ShowTrigger1 = false;
                ttb.Trigger1Click += new EventHandler(TwinTriggerBoxClick1);
                ttb.Trigger2Click += new EventHandler(TwinTriggerBoxClick2);
                if (!enable)
                {
                    ttb.Text = "您没有权限操作！";
                    ttb.Enabled = false;
                }
                ToolbarFill tf = new ToolbarFill();
                BaseToolBar.Items.Add(tf);

                BaseToolBar.Items.Add(ttb);
            }
        }
        /// <summary>
        /// 循环取出需要控制权限的控件
        /// </summary>
        /// <param name="page"></param>
        private void CheckControlsRight(System.Web.UI.Control page)
        {
            int nPageControls = page.Controls.Count;
            for (int i = 0; i < nPageControls; i++)
            {
                foreach (System.Web.UI.Control control in page.Controls[i].Controls)
                {
                    if (control.HasControls())
                    {
                        CheckControlsRight(control);
                    }
                    else
                    {

                        if (control is Button)
                        {
                            Button btn = (Button)control;
                            CheckButtonRight(btn);
                        }
                        //可扩展其它控件 目前只支持Button
                    }
                }
            }
        }
        private void CheckButtonRight(Button btn)
        {
            //过滤不对应菜单的页面
            if (BasePageSysMenu == null)
                return;
            string BtnEName = btn.ID.Replace("btn", "");
            sys_Action action = BasePageSysMenu.sys_Action.FirstOrDefault(a => a.ActionCode == BtnEName);

            //过滤不对功能名的按钮
            if (action == null)
                return;
            string btnRightStr = string.Format("{0}-{1}", BasePageSysMenu.MenuCode, action.ActionCode);
            if (!BaseRightStr.Contains(btnRightStr))
            {
                btn.Enabled = false;
                btn.ToolTip = "您没有权限操作！";
            }
        }
        #endregion

        #region 控件通用方法
        /// <summary>
        /// 通用编辑单击事件
        /// </summary>
        /// <param name="Grid1">Grid</param>
        /// <param name="Window1">Window Id</param>
        /// <param name="EditPage">编辑页面名称</param>
        /// <param name="WindowTitle">窗体标题</param>
        protected void EditClick(Grid Grid1, Window Window1, string EditPage, string WindowTitle)
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    string id = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("{0}.aspx?id={1}&action=2", EditPage, id), WindowTitle));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败，请重试！", MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="Window1">WindowId</param>
        /// <param name="AddPage">编辑页面</param>
        /// <param name="WindowTitle">窗体标题</param>
        protected void AddClick(Window Window1, string AddPage, string WindowTitle)
        {
            PageContext.RegisterStartupScript(Window1.GetShowReference(string.Format("{0}.aspx?action=1", AddPage), WindowTitle));
        }
        #endregion

        #region 用户登录信息
        protected sys_Member BaseMember
        {
            get
            {
                if (Session["USERINFO"] != null)
                    return Session["USERINFO"] as sys_Member;

                int uid = Convert.ToInt32(User.Identity.Name);
                sys_Member m = MemberService.FirstOrDefault(p => p.id == uid);
                Session["USERINFO"] = m;
                return m;
            }
        }
        protected string BaseUserName
        {
            get { return BaseMember.UserName; }
        }
        protected string BaseRealName
        {
            get { return BaseMember.RealName; }
        }
        protected int BaseUid
        {
            get { return BaseMember.id; }
        }
        protected string BaseRightStr
        {
            get
            {
                sys_Member m = BaseMember;
                string rightstr = m.RoleRightStr;
                foreach (sys_Role r in m.sys_Role)
                {
                    rightstr += r.RoleRightStr + ",";
                }
                return rightstr.Trim(',');
            }
        }
        private string BaseOperResStr
        {
            get
            {
                var m = BaseMember;
                string operresstr = m.RoleOperResStr ?? "";
                foreach (sys_Role r in m.sys_Role)
                {
                    operresstr += "," + r.RoleOperResStr;
                }
                return operresstr.Trim(',');
            }
        }
        #endregion

        #region 数据权限
        /// <summary>
        /// 数据权限解析 菜单ID对应可操作的用户ID
        /// </summary>
        private Dictionary<string, List<int>> MenuOperRes
        {
            get
            {
                if (Session["MENUOPERRES"] != null)
                    return Session["MENUOPERRES"] as Dictionary<string, List<int>>;
                string OperResStr = BaseOperResStr;
                Dictionary<string, List<int>> dic = new Dictionary<string, List<int>>();
                foreach (string s in OperResStr.Split(','))
                {
                    string[] mid_aid = s.Split('-');
                    if (mid_aid.Length == 2)
                    {
                        if (dic.ContainsKey(mid_aid[0]))
                        {
                            int re = 0;
                            int.TryParse(mid_aid[1], out re);
                            dic[mid_aid[0]].Add(re);
                        }
                        else
                        {
                            List<int> l = new List<int>();
                            l.Add(BaseUid);
                            int re = 0;
                            int.TryParse(mid_aid[1], out re);
                            l.Add(re);
                            dic.Add(mid_aid[0], l);
                        }
                    }
                }
                Session["MENUOPERRES"] = dic;
                return dic;
            }
        }
        /// <summary>
        /// 返回可操作的用户ID列表
        /// </summary>
        protected List<int> BaseOperResUidList
        {
            get
            {
                if (BasePageSysMenu == null)
                    return new List<int>() { BaseUid };//返回用户自己的ID

                if (MenuOperRes.ContainsKey(BasePageSysMenu.Id))
                    return MenuOperRes[BasePageSysMenu.Id];

                return new List<int>() { BaseUid };
            }
        }
        #endregion

        #region 日志管理
        private OperLogService operlogservice;
        private OperLogService BaseOperLogService
        {
            get { return operlogservice ?? (operlogservice = new OperLogService()); }
        }
        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="logtype">操作类型</param>
        /// <param name="content">操作内容</param>
        /// <param name="moduleName">模块名称</param>
        protected void Log(LogType logtype, string content, string moduleName)
        {
            try
            {
                var log = new sys_OperLog
                {
                    OperType = logtype.ToString(),
                    ModuleName = moduleName,
                    OperContent = content,
                    Ip = BaseIp,
                    Uid = BaseUid,
                    RealName = BaseRealName,
                    LogTime = DateTime.Now,
                    Address = "",
                    Mac = ""
                };
                BaseOperLogService.Add(log);
            }
            catch { }
        }
        #endregion

        #region 其它
        protected IEnumerable<int> ArrStrToInt(List<string> l)
        {
            List<int> n_l = new List<int>();
            foreach (string s in l)
            {
                int i = 0;
                int.TryParse(s, out i);
                n_l.Add(i);
            }
            return n_l;
        }
        /// <summary>
        /// 获取页面文件名称
        /// </summary>
        /// <returns></returns>
        private string GetUrl()
        {
            if (BasePageName != null)
                return BasePageName;
            string url = HttpContext.Current.Request.Url.PathAndQuery.ToString();
            return url.Substring(url.LastIndexOf("/") + 1);
        }
        protected string BaseIp
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(result) || !Utility.IsIP(result))
                    return "127.0.0.1";
                return result;
            }
        }
        #endregion
    }
}
