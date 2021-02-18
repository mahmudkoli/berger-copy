import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
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
    ],

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DealerSalesCallRoutingModule { }
