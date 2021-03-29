import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { DealerSalesCallDetailsComponent } from './dealer-sales-call-details/dealer-sales-call-details.component';
import { DealerSalesCallEmailConfigAddComponent } from './dealer-sales-call-email-config-add/dealer-sales-call-email-config-add.component';
import { DealerSalesCallEmailConfigListComponent } from './dealer-sales-call-email-config-list/dealer-sales-call-email-config-list.component';
import { DealerSalesCallListComponent } from './dealer-sales-call-list/dealer-sales-call-list.component';
import { DealerSalesCallComponent } from './dealer-sales-call.component';

const routes: Routes = [
  {
    path: '',
    component: DealerSalesCallComponent,
    // canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'list',
        pathMatch: 'full',
      },
      {
        path: 'list',
        component: DealerSalesCallListComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Sales Call', },
      },
      {
        path: 'details/:id',
        component: DealerSalesCallDetailsComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Sales Call Details', },
      },

      {
        path: 'emailList',
        component: DealerSalesCallEmailConfigListComponent
        // canActivate: [AuthGuard, PermissionGuard],
      },

      {
        path: 'addemailconfig',
        component: DealerSalesCallEmailConfigAddComponent
        // canActivate: [AuthGuard, PermissionGuard],
      },

      {
        path: 'addemailconfig/:id',
        component: DealerSalesCallEmailConfigAddComponent,
        // canActivate: [AuthGuard, PermissionGuard],
      },
    ],

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DealerSalesCallRoutingModule { }
