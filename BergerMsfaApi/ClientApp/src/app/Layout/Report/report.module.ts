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
import { TintingMachineReportComponent } from './tinting-machine/tinting-machine-report.component';

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
      TintingMachineReportComponent
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
