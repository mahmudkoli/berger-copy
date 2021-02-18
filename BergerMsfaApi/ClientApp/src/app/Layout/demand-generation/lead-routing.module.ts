import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { LeadListComponent } from './lead-list/lead-list.component';
import { LeadComponent } from './lead.component';

const routes: Routes = [
  {
    path: '',
    component: LeadComponent,
    // canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'list',
        pathMatch: 'full',
      },
      {
        path: 'list',
        component: LeadListComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Lead', },
      },
    ],

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LeadRoutingModule { }
