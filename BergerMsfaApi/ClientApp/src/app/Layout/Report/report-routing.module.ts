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
import { PainterCallReportComponent } from './painter-call/painter-call-report.component';
import { DealerVisitReportComponent } from './dealer-visit/dealer-visit-report.component';
import { SubDealerIssueReportComponent } from './sub-dealer-issue/sub-dealer-issue-report.component';
import { DealerIssueReportComponent } from './dealer-issue/dealer-issue-report.component';
import { SubDealerSalescallReportComponent } from './sub-dealer-salescall/sub-dealer-salescall-report.component';
import { DealerSalescallReportComponent } from './dealer-salescall/dealer-salescall-report.component';
import { TintingMachineReportComponent } from './tinting-machine/tinting-machine-report.component';
import { ActiveSummeryReportComponent } from './active-summery/active-summery-report.component';
import { OSOver90DaysTrendReportComponent } from './os-over-90-days-trend/os-over-90-days-trend-report.component';
import { MtsValueTargetAchivementReportComponent } from './mts-value-target-achivement/mts-value-target-achivement-report.component';
import { BillingDealerQuarterlyGrowthReportComponent } from './billing-dealer-quarterly-growth/billing-dealer-quarterly-growth-report.component';
import { EnamelPaintsQuarterlyGrowthReportComponent } from './enamel-paints-quarterly-growth/enamel-paints-quarterly-growth-report.component';
import { PremiumBrandsContributionReportComponent } from './premium-brands-contribution/premium-brands-contribution-report.component';
import { PremiumBrandsGrowthReportComponent } from './premium-brands-growth/premium-brands-growth-report.component';
import { MerchendizingSnapshotReportComponent } from './merchendizing-snapshot/merchendizing-snapshot-report.component';
import { LoginLogReportComponent } from './login-log/login-log-report.component';
import { TerritoryWiseKpiTargetAchivementReportComponent } from './territory-wise-kpi-target-achivement/territory-wise-kpi-target-achivement-report.component';
import { DealerWiseKpiTargetAchivementReportComponent } from './dealer-wise-kpi-target-achivement/dealer-wise-kpi-target-achivement-report.component';
import { ProductWiseKpiTargetAchivementReportComponent } from './product-wise-kpi-target-achivement/product-wise-kpi-target-achivement-report.component';

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
      {
        path: 'painter-call',
        component: PainterCallReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Painter Call', },
      },
      {
        path: 'dealer-visit',
        component: DealerVisitReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Visit', },
      },
      {
        path: 'sub-dealer-issue',
        component: SubDealerIssueReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Sub Dealer Visit', },
      },
      {
        path: 'dealer-issue',
        component: DealerIssueReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Issue', },
      },
      {
        path: 'sub-dealer-sales-call',
        component: SubDealerSalescallReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Sub Dealer Sales Call', },
      },
      {
        path: 'dealer-sales-call',
        component: DealerSalescallReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Sales Call', },
      },
      {
        path: 'tinting-machine',
        component: TintingMachineReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Tinting Machine Report', },
      },
      {
        path: 'active-summery',
        component: ActiveSummeryReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Active Summery Report', },
      },
      {
        path: 'os-over-90-days-trend',
        component: OSOver90DaysTrendReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'OS Over 90 Days Trend Report', },
      },



      {
        path: 'mts-value-target-achivement',
        component: MtsValueTargetAchivementReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'MTS Value Target achivement', },
      },

      {
        path: 'billing-dealer-quarterly-growth-report',
        component: BillingDealerQuarterlyGrowthReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Billing Dealer Quarterly Growth', },
      },

      {
        path: 'enamel-paints-quarterly-growth-report',
        component: EnamelPaintsQuarterlyGrowthReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Enamel Paints Quarterly Growth', },
      },


      {
        path: 'premium-brands-growth-report',
        component: PremiumBrandsGrowthReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Premium Brands Growth', },
      },


      {
        path: 'premium-brands-contribution-report',
        component: PremiumBrandsContributionReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Premium Brands Contribution', },
      },

      {
        path: 'merchendizing-snapshot-report',
        component: MerchendizingSnapshotReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Merchendizing Snapshot Report', },
      },

      {
        path: 'login-log-report',
        component: LoginLogReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Login Log Report', },
      },

      {
        path: 'territory-target-achievemt',
        component: TerritoryWiseKpiTargetAchivementReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Territory Target Achievement Report', },
      },

      {
        path: 'dealer-target-achievemt',
        component: DealerWiseKpiTargetAchivementReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Target Achievement Report', },
      },

      {
        path: 'product-target-achievemt',
        component: ProductWiseKpiTargetAchivementReportComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Dealer Target Achievement Report', },
      },

    ],
  }
];

// ProductWiseKpiTargetAchivementReportComponent

@NgModule({

  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
