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

@NgModule({
  declarations: [
    LeadListComponent,
    LeadComponent,
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
  ]
})
export class LeadModule { }
