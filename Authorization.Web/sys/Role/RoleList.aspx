<%@ Page Language="C#" EnableEventValidation="false" ViewStateEncryptionMode="Never" AutoEventWireup="true" CodeBehind="RoleList.aspx.cs" Inherits="Authorization.Web.sys.Role.RoleList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>角色管理</title>
    <script src="../../js/jquery-1.10.2.min.js"></script>
    <style>
        ul.powers li {
            margin: 5px 15px 5px 0;
            display: inline-block;
            min-width: 80px;
        }

            ul.powers li input {
                vertical-align: middle;
            }

            ul.powers li label {
                margin-left: 5px;
            }

        /* 自动换行，放置权限列表过长 */
        .x-grid3-row .x-grid3-cell-inner {
            white-space: normal;
        }

        .True {
            text-decoration: none;
            color: #1c0bf8;
        }

        .False {
            text-decoration: none;
            color: #b0afaf;
        }

        .x-tree-checkbox-checked-1 {
            background-position: 0 -30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="true" ShowHeader="false"
                    Width="200px" Position="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Panel ID="Panel3" ShowBorder="false" Title="角色列表" Layout="Fit" AutoScroll="true" runat="server">
                            <Items>
                                <f:Grid ID="Grid1" runat="server" ShowBorder="false" ShowHeader="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true"
                                    DataKeyNames="Id">
                                    <Columns>
                                        <f:BoundField DataField="RoleName" HeaderText="角色名称" ExpandUnusedSpace="true" />
                                    </Columns>
                                </f:Grid>
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="true" ShowHeader="false" Position="Center"
                    BoxConfigAlign="Stretch" BoxConfigPosition="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableFrame="false"
                            EnableTabCloseMenu="false" EnableTitleBackgroundColor="true" ActiveTabIndex="0" AutoPostBack="true" OnTabIndexChanged="TabStrip1_TabIndexChanged"
                            runat="server">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="添加" Layout="Fit"
                                    runat="server">
                                    <Items>
                                        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server" Position="Footer">
                                                    <Items>
                                                        <f:Button ID="btnAdd" ValidateForms="SimpleForm1" Icon="Disk" OnClick="btnSave_Click"
                                                            runat="server" Text="保存">
                                                        </f:Button>
                                                        <f:ToolbarFill ID="ToolbarFill1" runat="server"></f:ToolbarFill>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Items>
                                                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" Width="400" BodyPadding="20px"
                                                    Title="SimpleForm">
                                                    <Items>
                                                        <f:TextBox ID="tbxRoleName" runat="server" Label="角色名称" Required="true" ShowRedStar="true" TabIndex="1">
                                                        </f:TextBox>
                                                        <f:TextBox ID="tbxRoleCode" runat="server" Label="角色标识" Required="true" ShowRedStar="true" TabIndex="1">
                                                        </f:TextBox>
                                                        <f:TextArea ID="tbxRemark" runat="server" Label="描述" TabIndex="3">
                                                        </f:TextArea>
                                                    </Items>
                                                </f:SimpleForm>
                                            </Items>
                                        </f:Panel>
                                    </Items>
                                </f:Tab>
                                <f:Tab ID="Tab2" Title="修改" runat="server" Layout="Fit">
                                    <Items>
                                        <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar2" runat="server" Position="Footer">
                                                    <Items>
                                                        <f:Button ID="btnEdit" ValidateForms="SimpleForm2" Icon="Disk" OnClick="btnSave_Edit_Click"
                                                            runat="server" Text="保存">
                                                        </f:Button>
                                                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                        </f:ToolbarSeparator>
                                                        <f:Button ID="btnDelete" Icon="Delete" OnClick="btnDel_Click"
                                                            runat="server" Text="删除">
                                                        </f:Button>
                                                        <f:ToolbarFill ID="ToolbarFill2" runat="server"></f:ToolbarFill>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Items>
                                                <f:SimpleForm ID="SimpleForm2" ShowBorder="false" ShowHeader="false" runat="server" Width="400"
                                                    BodyPadding="20px" Title="SimpleForm">
                                                    <Items>
                                                        <f:TextBox ID="tbxRoleName_Edit" runat="server" Label="角色名称" Required="true" ShowRedStar="true" TabIndex="1">
                                                        </f:TextBox>
                                                        <f:TextBox ID="tbxRoleCode_Edit" runat="server" Label="角色标识" Required="true" ShowRedStar="true" TabIndex="1">
                                                        </f:TextBox>
                                                        <f:TextArea ID="taRemark_Edit" runat="server" Label="描述" TabIndex="3">
                                                        </f:TextArea>
                                                    </Items>
                                                </f:SimpleForm>
                                            </Items>
                                        </f:Panel>
                                    </Items>
                                </f:Tab>
                                <f:Tab ID="Tab3" Title="角色授权" Layout="Fit"
                                    runat="server">
                                    <Items>
                                        <f:RegionPanel ID="RegionPanel2" ShowBorder="false" runat="server">
                                            <Regions>
                                                <f:Region ID="Region4" Position="Center" Title="菜单操作列表" ShowBorder="false" ShowHeader="false"
                                                    Layout="Fit"
                                                    runat="server">
                                                    <Items>
                                                        <f:Grid ID="Grid2" runat="server" ShowBorder="false" ShowHeader="false" OnRowDataBound="Grid2_RowDataBound" OnRowClick="Grid2_RowClick"
                                                            EnableRowClickEvent="true" DataKeyNames="Mid,IsOperRes">
                                                            <Toolbars>
                                                                <f:Toolbar ID="Toolbar3" runat="server">
                                                                    <Items>
                                                                        <f:Button ID="Button1" EnablePostBack="false" runat="server" Text="全选/取消">
                                                                            <Menu ID="Menu1" runat="server">
                                                                                <f:MenuButton ID="btnSelectAll" EnablePostBack="false" runat="server" Text="全选">
                                                                                </f:MenuButton>
                                                                                <f:MenuButton ID="btnSelectRows" EnablePostBack="false" runat="server" Text="全选选中行">
                                                                                </f:MenuButton>
                                                                                <f:MenuButton ID="btnUnselectAll" EnablePostBack="false" runat="server" Text="取消">
                                                                                </f:MenuButton>
                                                                                <f:MenuButton ID="btnUnselectRows" EnablePostBack="false" runat="server" Text="取消选中行">
                                                                                </f:MenuButton>
                                                                            </Menu>
                                                                        </f:Button>
                                                                        <f:ToolbarFill ID="tf1" runat="server"></f:ToolbarFill>
                                                                        <f:Button ID="btnSaveRoleRight" Icon="GroupEdit" runat="server" Text="保存角色权限" OnClick="btnSaveRoleRight_Click">
                                                                        </f:Button>
                                                                        <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                                                                        </f:ToolbarSeparator>
                                                                        <f:Button ID="btnCopeRoleRight" Icon="UserGo" runat="server" Text="克隆角色权限">
                                                                        </f:Button>
                                                                    </Items>
                                                                </f:Toolbar>
                                                            </Toolbars>
                                                            <Columns>
                                                                <f:BoundField DataField="AppName" HeaderText="模块名称" Width="150px" />
                                                                <f:BoundField DataField="MenuName" HeaderText="菜单标题" DataSimulateTreeLevelField="TreeLevel"
                                                                    Width="150px" />
                                                                <f:ImageField Width="50px" DataImageUrlField="Ico" DataImageUrlFormatString="{0}" TextAlign="Center"
                                                                    HeaderText="图标"></f:ImageField>
                                                                <f:TemplateField ExpandUnusedSpace="true" ColumnID="Powers" HeaderText="权限列表">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBoxList ID="cblPowers" CssClass="powers" RepeatDirection="Horizontal" RepeatColumns="8"
                                                                            runat="server">
                                                                        </asp:CheckBoxList>
                                                                    </ItemTemplate>
                                                                </f:TemplateField>
                                                                <f:TemplateField Width="100px" ColumnID="OpOperRes" HeaderText="" TextAlign="Center">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbtnOperRes" Enabled='<%# Convert.ToBoolean( Eval("IsOperRes") )%>' runat="server" Text='<%# Convert.ToBoolean( Eval("IsOperRes"))==true?"[有范围]":"[无范围]" %>' CssClass='<%#Eval("IsOperRes") %>' OnClick="lbtnOperRes_Click"></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </f:TemplateField>
                                                            </Columns>
                                                        </f:Grid>
                                                    </Items>
                                                </f:Region>
                                                <f:Region ID="Region5" EnableCollapse="true" Title="数据权限" ShowBorder="false" Split="true"
                                                    Width="200px" Position="Right" Layout="Fit"
                                                    runat="server">
                                                    <Items>
                                                        <f:Tree ID="trUser" runat="server" ShowHeader="false" Title="数据权限" OnNodeCheck="trUser_NodeCheck"></f:Tree>
                                                    </Items>
                                                </f:Region>
                                            </Regions>
                                        </f:RegionPanel>
                                    </Items>
                                </f:Tab>
                                <f:Tab ID="Tab4" Title="角色用户" Layout="Fit"
                                    runat="server">
                                    <Items>
                                        <f:Grid ID="Grid3" runat="server" EnableCheckBoxSelect="true" ShowBorder="false" ShowHeader="false" IsDatabasePaging="true" AllowPaging="true" AllowSorting="true" OnPageIndexChange="Grid3_PageIndexChange" OnSort="Grid3_Sort"
                                            SortField="id" SortDirection="DESC" DataKeyNames="id">
                                            <Toolbars>
                                                <f:Toolbar ID="tb1" runat="server">
                                                    <Items>
                                                        <f:Button ID="btnAddUser" Icon="GroupAdd" runat="server" Text="添加角色外用户" OnClick="btnAddUser_Click">
                                                        </f:Button>
                                                        <f:ToolbarSeparator ID="ToolbarSeparator4" runat="server">
                                                        </f:ToolbarSeparator>
                                                        <f:Button ID="btnRemoveUser" Icon="GroupDelete" runat="server" Text="删除角色中用户" OnClick="btnRemoveUser_Click">
                                                        </f:Button>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Columns>
                                                <f:RowNumberField />
                                                <f:BoundField DataField="UserName" HeaderText="用户名" SortField="UserName"
                                                    Width="100px" />
                                                <f:BoundField DataField="RealName" HeaderText="姓名" SortField="RealName"
                                                    Width="100px" />
                                                <f:BoundField DataField="sys_Orgnization.OrgnizationName" HeaderText="所属部门" SortField="sys_Orgnization.OrgnizationName"
                                                    Width="150px" />
                                            </Columns>
                                            <PageItems>
                                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                                </f:ToolbarSeparator>
                                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                                </f:ToolbarText>
                                                <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                                                    runat="server">
                                                    <f:ListItem Text="10" Value="10" />
                                                    <f:ListItem Text="20" Value="20" />
                                                    <f:ListItem Text="50" Value="50" />
                                                    <f:ListItem Text="100" Value="100" />
                                                </f:DropDownList>
                                            </PageItems>
                                        </f:Grid>
                                    </Items>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" Title="" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server" OnClose="Window1_Close"
            IsModal="true" Width="650px" EnableConfirmOnClose="true" Height="400px">
        </f:Window>
    </form>

    <script>
        var grid2ID = '<%= Grid2.ClientID %>';
        var selectAllID = '<%= btnSelectAll.ClientID %>';
        var selectRowsID = '<%= btnSelectRows.ClientID %>';
        var unselectAllID = '<%= btnUnselectAll.ClientID %>';
        var unselectRowsID = '<%= btnUnselectRows.ClientID %>';

        function createQtip() {
            Ext.select('.powers td span').each(function (el) {
                var qtip = el.getAttribute('data-qtip');
                el.select('input,label').set({ 'ext:qtip': qtip });
            });
        }

        Ext.onReady(function () {
            var grid = F(grid2ID);
            grid.addListener('viewready', function () {
                createQtip();
            });


            F(selectAllID).on('click', function () {
                Ext.select('.powers td span input').set({ checked: true }, false);
            });
            F(selectRowsID).on('click', function () {
                Ext.select('.x-grid-row-selected .powers td span input').set({ checked: true }, false);
            });

            F(unselectAllID).on('click', function () {
                Ext.select('.powers td span:not(.aspNetDisabled) input').set({ checked: false }, false);
            });
            F(unselectRowsID).on('click', function () {
                Ext.select('.x-grid-row-selected .powers td span:not(.aspNetDisabled) input').set({ checked: false }, false);
            });
        }
         )
        function onAjaxReady() {
            createQtip();
        }
        function aaa(id) {
            $("#treeview-1016-record-" + id + " td div input").addClass("x-tree-checkbox-checked-1");

        }
    </script>
</body>
</html>
