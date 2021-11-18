import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { LeadRoutingModule } from './lead-routing.module';
import { LeadComponent } from './lead.component';
import { LeadListComponent } from './lead-list/lead-list.component';
import { LeadDetailsComponent } from './lead-details/lead-details.component';
import { ModalLeadFollowUpDetailsComponent } from './modal-lead-followup-details/modal-lead-followup-details.component';
import { LeadEditComponent } from './lead-edit/lead-edit.component';

@NgModule({
  declarations: [
    LeadListComponent,
    LeadDetailsComponent,
    LeadComponent,
    LeadEditComponent,
    ModalLeadFollowUpDetailsComponent
  ],
  imports: [
    CommonModule,
    LeadRoutingModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule
  ],
  entryComponents: [
    ModalLeadFollowUpDetailsComponent
  ]
})
export class LeadModule { }
