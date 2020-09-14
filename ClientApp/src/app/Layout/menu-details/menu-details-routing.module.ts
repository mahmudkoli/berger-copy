import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MenuListComponent } from './menu-list/menu-list.component';
import { MenuPermissionsComponent } from './menu-permissions/menu-permissions.component';
import { MenuActivityListComponent } from './menu-activity-list/menu-activity-list.component';
import { MenuActivityPermissionsComponent } from './menu-activity-permissions/menu-activity-permissions.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', redirectTo: 'menu-list' },
      { path: 'menu-list', component: MenuListComponent,canActivate: [PermissionGuard],data: {extraParameter: 'dashboardsMenu', permissionType: 'view', permissionGroup: '/menu/menu-list'} },
        { path: 'menu-permissions', component: MenuPermissionsComponent,canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: '/menu/menu-permissions' } },
        { path: 'menu-activity', component: MenuActivityListComponent,canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: '/menu/menu-activity' } },
        { path: 'menu-activity-permission', component: MenuActivityPermissionsComponent,canActivate: [PermissionGuard], data: { permissionType: 'view', permissionGroup: '/menu/menu-activity-permission' } }
    
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MenuDetailsRoutingModule {

}
