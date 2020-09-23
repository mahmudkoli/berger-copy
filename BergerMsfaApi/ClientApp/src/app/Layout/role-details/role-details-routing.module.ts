import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleAddComponent } from './role-add/role-add.component';
import { RoleLinkWithUserComponent } from './role-link-with-user/role-link-with-user.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'role-list' },
            { path: 'role-list', component: RoleListComponent, canActivate: [PermissionGuard], data: { extraParameter: 'role', permissionType: 'view', permissionGroup: 'role/role-list' } },
            { path: 'role-add', component: RoleAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'role/role-list' } },
            { path: "role-add/:id", component: RoleAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'role/role-list' } },
            { path: 'role-link-with-user', component: RoleLinkWithUserComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'role/role-list' } }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RoleDetailsRoutingModule { }
