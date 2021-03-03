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

@NgModule({
    declarations: [
      ReportComponent,
      LeadSummaryReportComponent,
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
