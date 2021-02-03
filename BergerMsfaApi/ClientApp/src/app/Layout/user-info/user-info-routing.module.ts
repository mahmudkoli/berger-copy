import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { UserInfoListComponent } from './user-info-list/user-info-list.component';
import { UserInfoFormComponent } from './user-info-form/user-info-form.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'users-infolist' },
            { path: 'newuser-info', component: UserInfoFormComponent, 
            
            //data: { permissionType: 'create', permissionGroup: 'users-info/users-infolist' }
         },
            { path: 'users-infolist', component: UserInfoListComponent,
           //  canActivate: [PermissionGuard], 
             //data: { permissionType: 'view', permissionGroup: 'users-info/users-infolist' } 
            },
            { path: 'edituser-info/:id', component: UserInfoFormComponent, 
           // canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'users-info/users-infolist' }
         }
        ]
    }
];



@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class UserInfoRoutingModule {

}