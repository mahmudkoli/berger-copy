import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { BrandListComponent } from './brand-list/brand-list.component';
import { BrandComponent } from './brand.component';
import { BrandInfoLogDetailsComponent } from './brand-info-log-details/brand-info-log-details.component';
const routes: Routes = [
  {
    path: '',
    component: BrandComponent,
    // canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'list',
        pathMatch: 'full',
      },
      {
        path: 'list',
        component: BrandListComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Brand', },
      },
    {
        path: 'log-details/:id',
        component: BrandInfoLogDetailsComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Brand Info log details', },
    },
    ],

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BrandRoutingModule { }
