import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkFlowAddComponent } from './work-flow-add/work-flow-add.component';
import { WorkFlowListComponent } from './work-flow-list/work-flow-list.component';
import { WorkFlowTreeviewComponent } from './work-flow-treeview/work-flow-treeview.component';
import { WorkflowConfigurationListComponent } from './workflow-configuration-list/workflow-configuration-list.component';
import { WorkflowConfigurationAddComponent } from './workflow-configuration-add/workflow-configuration-add.component';
import { OrganizationRoleListComponent } from './organization-role-list/organization-role-list.component';
import { OrganizationRoleAddComponent } from './organization-role-add/organization-role-add.component';
import { OrgRoleLinkWithUserComponent } from './org-role-link-with-user/org-role-link-with-user.component';
import { OrganizationRoleEditComponent } from './organization-role-edit/organization-role-edit.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [

  {
    path: '',
    children: [
        { path: '', redirectTo: 'work-flow-list' },
        { path: 'work-flow-list', component: WorkFlowListComponent,canActivate: [PermissionGuard], data: { extraParameter: 'workflow', permissionType: 'view', permissionGroup: '/work-flow/work-flow-list' }},
        { path: 'work-flow-add', component: WorkFlowAddComponent,canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: '/work-flow/work-flow-list' }},
        { path: "work-flow-add/:id", component: WorkFlowAddComponent,canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: '/work-flow/work-flow-list' }},
        { path: 'work-flow-treeview', component: WorkFlowTreeviewComponent,canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: '/work-flow/work-flow-treeview' } },
        { path: 'workflow-configuration-list', component: WorkflowConfigurationListComponent,canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: '/work-flow/workflow-configuration-list' }},
        { path: 'workflow-configuration-add', component: WorkflowConfigurationAddComponent ,canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: '/work-flow/workflow-configuration-list' }},
        { path: 'workflow-configuration-add/:id', component: WorkflowConfigurationAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: '/work-flow/workflow-configuration-list'} },
        { path: 'organization-role-list', component: OrganizationRoleListComponent,canActivate: [PermissionGuard], data: {extraParameter: 'orgrole', permissionType: 'view', permissionGroup: '/work-flow/organization-role-list' } },
        { path: 'organization-role-add', component: OrganizationRoleAddComponent,canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: '/work-flow/organization-role-list' } },
        { path: 'organization-role-add/:id', component: OrganizationRoleAddComponent,canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: '/work-flow/organization-role-list' } },
        { path: 'organization-role-edit/:id', component: OrganizationRoleEditComponent,canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: '/work-flow/organization-role-list' } },
        { path: 'organization-role-link-with-user', component: OrgRoleLinkWithUserComponent }
    ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkFlowRoutingModule { }
