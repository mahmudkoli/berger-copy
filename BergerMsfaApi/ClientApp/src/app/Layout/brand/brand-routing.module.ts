import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { BrandListComponent } from './brand-list/brand-list.component';
import { BrandComponent } from './brand.component';

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
    ],

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BrandRoutingModule { }
