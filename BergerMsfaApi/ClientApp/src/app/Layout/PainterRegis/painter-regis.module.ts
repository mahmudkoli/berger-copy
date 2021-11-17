import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PainterRegisListComponent } from './painter-regis-list/painter-regis-list.component';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PainterRegisRoutingModule } from './painterRegis-routing.module';
import { PainterRegisDetailComponent } from './painter-regis-detail/painter-regis-detail.component';
import { ModalPainterCallDetailsComponent } from './modal-painter-call-details/modal-painter-call-details.component';
import { PainterUpdateStatusComponent } from './painter-update-status/painter-update-status.component';
import { PainterEditFormComponent } from './painter-edit-form/painter-edit-form.component';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [
    PainterRegisListComponent, 
    PainterRegisDetailComponent, 
    ModalPainterCallDetailsComponent, 
    PainterUpdateStatusComponent,
    PainterEditFormComponent
  ],
  imports: [
      CommonModule,
      SharedMasterModule,
      PainterRegisRoutingModule,
      AngularFontAwesomeModule,
      NgbModule,
      FormsModule,
      ReactiveFormsModule,
      NgSelectModule 
    ],
    entryComponents: [
        ModalPainterCallDetailsComponent
    ]
})
export class PainterRegisModule { }
