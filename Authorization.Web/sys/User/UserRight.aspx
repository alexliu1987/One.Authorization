<%@ Page Language="C#"  EnableEventValidation="false" ViewStateEncryptionMode="Never" AutoEventWireup="true" CodeBehind="UserRight.aspx.cs" Inherits="Authorization.Web.sys.User.UserRight" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
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
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel2" />
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
                                        <f:Label ID="lbMsg" runat="server" Text="提示：灰色禁用部分为用户所属角色权限，只能通过角色授权更改！"></f:Label>
                                        <f:ToolbarFill ID="tf1" runat="server"></f:ToolbarFill>
                                        <f:Button ID="btnRight" Icon="UserKey" runat="server" Text="保存用户权限" OnClick="btnRight_Click">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
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
    </script>
</body>
</html>
