import { Route } from '@angular/compiler/src/core';
import { Routes, RouterModule } from '@angular/router';
import { UserListComponent } from './user-list/user-list.component';
import { NgModule } from '@angular/core';
import { CreateCmUserComponent } from './create-cm-user/create-user.component';
import { EditCmUserComponent } from './edit-cm-user/edit-user.component';
import { DelegationAddComponent } from './delegation-add/delegation-add.component';
import { DelegationListComponent } from './delegation-list/delegation-list.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'users-list' },
            { path: 'create-user', component: CreateCmUserComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'users/users-list' } },
            { path: 'users-list', component: UserListComponent, canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: 'users/users-list' } },
            { path: 'edit-cmuser/:id', component: EditCmUserComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'users/users-list' } },
            { path: 'delegation-add', component: DelegationAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'users/delegation-list' } },
            { path: 'delegation-list', component: DelegationListComponent, canActivate: [PermissionGuard] , data: { permissionType: 'view', permissionGroup: 'users/delegation-list' }},
            { path: 'delegation-add/:id', component: DelegationAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'users/delegation-list' } }
        ]
    }
];


@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class UserDetailsRoutingModule {

}