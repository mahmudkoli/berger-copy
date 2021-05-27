import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { ReportRoutingModule } from './report-routing.module';
import { ReportComponent } from './report.component';
import { LeadSummaryReportComponent } from './lead-summary/lead-summary-report.component';
import { LeadGenerationDetailsReportComponent } from './lead-generation-details/lead-generation-details-report.component';
import { LeadFollowUpDetailsReportComponent } from './lead-followup-details/lead-followup-details-report.component';
import { PainterRegistrationReportComponent } from './painter-registration/painter-registration-report.component';
import { DealerOpeningReportComponent } from './dealer-opening/dealer-opening-report.component';
import { DealerCollectionReportComponent } from './dealer-collection/dealer-collection-report.component';
import { SubDealerCollectionReportComponent } from './sub-dealer-collection/sub-dealer-collection-report.component';
import { CustomerCollectionReportComponent } from './customer-collection/customer-collection-report.component';
import { ProjectCollectionReportComponent } from './project-collection/project-collection-report.component';
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
import { PremiumBrandsGrowthReportComponent } from './premium-brands-growth/premium-brands-growth-report.component';
import { PremiumBrandsContributionReportComponent } from './premium-brands-contribution/premium-brands-contribution-report.component';
import { MerchendizingSnapshotReportComponent } from './merchendizing-snapshot/merchendizing-snapshot-report.component';
import { LoginLogReportComponent } from './login-log/login-log-report.component';
import { TerritoryWiseKpiTargetAchivementReportComponent } from './territory-wise-kpi-target-achivement/territory-wise-kpi-target-achivement-report.component';
import { DealerWiseKpiTargetAchivementReportComponent } from './dealer-wise-kpi-target-achivement/dealer-wise-kpi-target-achivement-report.component';
import { ProductWiseKpiTargetAchivementReportComponent } from './product-wise-kpi-target-achivement/product-wise-kpi-target-achivement-report.component';
import { BusinessCallAnalysisReportComponent } from './business-call-analysis/business-call-analysis-report.component';
import { StrikeRateKpiReportComponent } from './strike-rate-kpi/strike-rate-kpi-report.component';
import { AddhocSubDealerSalescallReportComponent } from './addhoc-sub-dealer-salescall/addhoc-sub-dealer-salescall-report.component';
import { AddhocDealerSalescallReportComponent } from './addhoc-dealer-salescall/addhoc-dealer-salescall-report.component';

@NgModule({
    declarations: [
      ReportComponent,
      LeadSummaryReportComponent,
      LeadGenerationDetailsReportComponent,
      LeadFollowUpDetailsReportComponent,
      PainterRegistrationReportComponent,
      DealerOpeningReportComponent,
      DealerCollectionReportComponent,
      SubDealerCollectionReportComponent,
      CustomerCollectionReportComponent,
      ProjectCollectionReportComponent,
      PainterCallReportComponent,
      DealerVisitReportComponent,
      SubDealerIssueReportComponent,
      DealerIssueReportComponent,
      SubDealerSalescallReportComponent,
      DealerSalescallReportComponent,
      DealerVisitReportComponent,
      TintingMachineReportComponent,
      ActiveSummeryReportComponent,
      OSOver90DaysTrendReportComponent,
      MtsValueTargetAchivementReportComponent,
      BillingDealerQuarterlyGrowthReportComponent,
      EnamelPaintsQuarterlyGrowthReportComponent,
      PremiumBrandsGrowthReportComponent,
      PremiumBrandsContributionReportComponent,
      MerchendizingSnapshotReportComponent,
      LoginLogReportComponent,
      TerritoryWiseKpiTargetAchivementReportComponent,
      DealerWiseKpiTargetAchivementReportComponent,
      ProductWiseKpiTargetAchivementReportComponent,
      BusinessCallAnalysisReportComponent,
      StrikeRateKpiReportComponent,
      AddhocSubDealerSalescallReportComponent,
      AddhocDealerSalescallReportComponent,
    ],
  imports: [
      CommonModule,
      CommonModule,
      SharedMasterModule,
      ReportRoutingModule,
      AngularFontAwesomeModule,
      NgbModule,
      FormsModule,
      ReactiveFormsModule,
      NgSelectModule,
  ]
})
export class ReportModule { }
