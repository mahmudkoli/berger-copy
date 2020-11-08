using Microsoft.EntityFrameworkCore.Migrations;

namespace Berger.Data.Migrations
{
    public partial class seedDefaultData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"SET IDENTITY_INSERT [dbo].[Menus] ON 
GO
INSERT[dbo].[Menus]([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [Name], [Controller], [Action], [Url], [IconClass], [ParentId], [IsParent], [IsTitle], [Sequence]) VALUES(1, 0, CAST(N'2020-10-25T16:40:31.2002896' AS DateTime2), 0, CAST(N'2020-10-25T18:24:46.7177526' AS DateTime2), 1, NULL, 0, N'User Management', N'', N'', N'', N'pe-7s-paint-bucket', 6, 1, 0, 0)
GO
INSERT[dbo].[Menus] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [Name], [Controller], [Action], [Url], [IconClass], [ParentId], [IsParent], [IsTitle], [Sequence]) VALUES(2, 0, CAST(N'2020-10-25T16:41:31.1348258' AS DateTime2), 0, CAST(N'2020-10-25T18:16:10.4524300' AS DateTime2), 1, NULL, 0, N'Application User', N'', N'', N'/users-info', N'', 1, 0, 0, 1)
GO
INSERT[dbo].[Menus] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [Name], [Controller], [Action], [Url], [IconClass], [ParentId], [IsParent], [IsTitle], [Sequence]) VALUES(3, 0, CAST(N'2020-10-25T16:44:38.4081028' AS DateTime2), 0, CAST(N'2020-10-25T18:27:17.1720972' AS DateTime2), 1, NULL, 0, N'Menu', N'', N'', N'', N'pe-7s-umbrella', 6, 1, 0, 0)
GO
INSERT[dbo].[Menus] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [Name], [Controller], [Action], [Url], [IconClass], [ParentId], [IsParent], [IsTitle], [Sequence]) VALUES(4, 0, CAST(N'2020-10-25T16:44:58.2429834' AS DateTime2), 0, CAST(N'2020-10-25T18:57:10.1971577' AS DateTime2), 1, NULL, 0, N'Menus', N'', N'', N'/menu/menu-list', N'', 3, 0, 0, 0)
GO
INSERT[dbo].[Menus] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [Name], [Controller], [Action], [Url], [IconClass], [ParentId], [IsParent], [IsTitle], [Sequence]) VALUES(5, 0, CAST(N'2020-10-25T16:45:24.6352672' AS DateTime2), 0, CAST(N'2020-10-25T18:07:57.8575702' AS DateTime2), 1, NULL, 0, N'Menu Permission', N'', N'', N'/menu/menu-permissions', N'', 3, 0, 0, 1)
GO
INSERT[dbo].[Menus] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [Name], [Controller], [Action], [Url], [IconClass], [ParentId], [IsParent], [IsTitle], [Sequence]) VALUES(6, 0, CAST(N'2020-10-25T18:18:32.1009280' AS DateTime2), 0, CAST(N'2020-10-25T18:18:32.1009280' AS DateTime2), 1, NULL, 0, N'Main Navigation', N'', N'', N'', N'', 0, 0, 0, 0)
GO
SET IDENTITY_INSERT[dbo].[Menus] OFF
GO
SET IDENTITY_INSERT[dbo].[Roles] ON
GO
INSERT[dbo].[Roles]([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [Name]) VALUES(1, 1, CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), NULL, NULL, 1, NULL, 1, N'Admin')
GO
SET IDENTITY_INSERT[dbo].[Roles] OFF
GO
SET IDENTITY_INSERT[dbo].[MenuPermissions] ON
GO
INSERT[dbo].[MenuPermissions]([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [RoleId], [MenuId]) VALUES(1, 0, CAST(N'2020-10-25T16:52:32.1369802' AS DateTime2), 0, CAST(N'2020-10-25T16:52:32.1369802' AS DateTime2), 1, NULL, 0, 1, 2)
GO
INSERT[dbo].[MenuPermissions] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [RoleId], [MenuId]) VALUES(2, 0, CAST(N'2020-10-25T16:52:32.1369802' AS DateTime2), 0, CAST(N'2020-10-25T16:52:32.1369802' AS DateTime2), 1, NULL, 0, 1, 4)
GO
INSERT[dbo].[MenuPermissions] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [RoleId], [MenuId]) VALUES(3, 0, CAST(N'2020-10-25T16:52:32.1369802' AS DateTime2), 0, CAST(N'2020-10-25T16:52:32.1369802' AS DateTime2), 1, NULL, 0, 1, 5)
GO
INSERT[dbo].[MenuPermissions] ([Id], [CreatedBy], [CreatedTime], [ModifiedBy], [ModifiedTime], [Status], [WorkflowId], [WFStatus], [RoleId], [MenuId]) VALUES(4, 0, CAST(N'2020-10-25T18:54:15.8407284' AS DateTime2), 0, CAST(N'2020-10-25T18:54:15.8407284' AS DateTime2), 1, NULL, 0, 1, 6)
GO
SET IDENTITY_INSERT[dbo].[MenuPermissions] OFF
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
