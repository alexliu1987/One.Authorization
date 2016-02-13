

using System.Data.Entity;
using Authorization.Model;
using Authorization.Framework.EntityRepository;
using Authorization.Framework.DataBase;
namespace Authorization.Services
{
    public class ActionService : EntityRepositoryBase<DbContext, sys_Action>
    {

        public ActionService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public ActionService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class AppService : EntityRepositoryBase<DbContext, sys_App>
    {

        public AppService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public AppService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class DictionaryService : EntityRepositoryBase<DbContext, sys_Dictionary>
    {

        public DictionaryService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public DictionaryService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class DictionaryTypeService : EntityRepositoryBase<DbContext, sys_DictionaryType>
    {

        public DictionaryTypeService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public DictionaryTypeService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class ErrorLogService : EntityRepositoryBase<DbContext, sys_ErrorLog>
    {

        public ErrorLogService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public ErrorLogService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class ForbidIpService : EntityRepositoryBase<DbContext, sys_ForbidIp>
    {

        public ForbidIpService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public ForbidIpService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class LoginLogService : EntityRepositoryBase<DbContext, sys_LoginLog>
    {

        public LoginLogService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public LoginLogService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class MemberService : EntityRepositoryBase<DbContext, sys_Member>
    {

        public MemberService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public MemberService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class MemberExtendService : EntityRepositoryBase<DbContext, sys_MemberExtend>
    {

        public MemberExtendService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public MemberExtendService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class MenuService : EntityRepositoryBase<DbContext, sys_Menu>
    {

        public MenuService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public MenuService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class OperLogService : EntityRepositoryBase<DbContext, sys_OperLog>
    {

        public OperLogService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public OperLogService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class OrgnizationService : EntityRepositoryBase<DbContext, sys_Orgnization>
    {

        public OrgnizationService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public OrgnizationService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class RoleService : EntityRepositoryBase<DbContext, sys_Role>
    {

        public RoleService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public RoleService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class ViewMemberRoleService : EntityRepositoryBase<DbContext, vw_MemberRole>
    {

        public ViewMemberRoleService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public ViewMemberRoleService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
    public class ViewAppMenuService : EntityRepositoryBase<DbContext, vw_AppMenu>
    {

        public ViewAppMenuService(DbContext context)
        {
            base.Context = context;
            base.IsOwnContext = false;
        }
        public ViewAppMenuService()
        {
            base.Context = DbContextHelper.CreateDbContext();
            IsOwnContext = true;
        }
    }
}