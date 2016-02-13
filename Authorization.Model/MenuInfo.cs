using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Model
{
    public class MenuInfo
    {
        public string Mid { get; set; }
        public int AppId { get; set; }
        public string AppName{ get; set; }
        public string AppCode{ get; set; }
        public string MenuName { get; set; }
        public string MenuCode { get; set; }
        public string PMid { get; set; }
        public int TreeLevel { get; set; }
        public string Ico { get; set; }
        public string Url { get; set; }
        public bool IsUsing { get; set; }
        public bool IsOperRes { get; set; }
        public int SortIndex { get; set; }
        public string ActionList { get; set; }

        public string Describe { get; set; }
    }
}
