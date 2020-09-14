import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { UserInfoListComponent } from './user-info-list/user-info-list.component';
import { UserInfoInsertComponent } from './user-info-insert/user-info-insert.component';
import { UserInfoEditComponent } from './user-info-edit/user-info-edit.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'users-infolist' },
            { path: 'insertuser-info', component: UserInfoInsertComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'users-info/users-infolist' } },
            { path: 'users-infolist', component: UserInfoListComponent, canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: 'users-info/users-infolist' } },
            { path: 'edituser-info/:id', component: UserInfoEditComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'users-info/users-infolist' } }
        ]
    }
];



@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class UserInfoRoutingModule {

}