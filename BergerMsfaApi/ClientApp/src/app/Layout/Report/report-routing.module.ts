import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { CustomerCollectionReportComponent } from './customer-collection/customer-collection-report.component';
import { DealerCollectionReportComponent } from './dealer-collection/dealer-collection-report.component';
import { DealerOpeningReportComponent } from './dealer-opening/dealer-opening-report.component';
import { ProjectCollectionReportComponent } from './project-collection/project-collection-report.component';
import { LeadFollowUpDetailsReportComponent } from './lead-followup-details/lead-followup-details-report.component';
import { LeadGenerationDetailsReportComponent } from './lead-generation-details/lead-generation-details-report.component';
import { LeadSummaryReportComponent } from './lead-summary/lead-summary-report.component';
import { PainterRegistrationReportComponent } from './painter-registration/painter-registration-report.component';
import { ReportComponent } from './report.component';
import { SubDealerCollectionReportComponent } from './sub-dealer-collection/sub-dealer-collection-report.component';

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
      {
        path: 'dealer-collection',
        component: DealerCollectionReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Collection', },
      },
      {
        path: 'sub-dealer-collection',
        component: SubDealerCollectionReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Sub Dealer Collection', },
      },
      {
        path: 'customer-collection',
        component: CustomerCollectionReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Customer Collection', },
      },
      {
        path: 'project-collection',
        component: ProjectCollectionReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Direct Project Collection', },
      },
    ],
  }
];

@NgModule({

  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
