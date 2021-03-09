import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { DealerOpeningReportComponent } from './dealer-opening/dealer-opening-report.component';
import { LeadFollowUpDetailsReportComponent } from './lead-followup-details/lead-followup-details-report.component';
import { LeadGenerationDetailsReportComponent } from './lead-generation-details/lead-generation-details-report.component';
import { LeadSummaryReportComponent } from './lead-summary/lead-summary-report.component';
import { PainterRegistrationReportComponent } from './painter-registration/painter-registration-report.component';
import { ReportComponent } from './report.component';

const routes: Routes = [
  {
    path: '',
    component: ReportComponent,
    // canActivate: [AuthGuard],
    children: [
      // {
      //   path: '',
      //   redirectTo: 'list',
      //   pathMatch: 'full',
      // },
      {
        path: 'lead-summary',
        component: LeadSummaryReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Lead Summary', },
      },
      {
        path: 'lead-generation-details',
        component: LeadGenerationDetailsReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Lead Generation Details', },
      },
      {
        path: 'lead-followup-details',
        component: LeadFollowUpDetailsReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Lead FollowUp Details', },
      },
      {
        path: 'painter-registration',
        component: PainterRegistrationReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Painter Registration', },
      },
      {
        path: 'dealer-opening',
        component: DealerOpeningReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Opening', },
      },
    ],
  }
];

@NgModule({

  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
