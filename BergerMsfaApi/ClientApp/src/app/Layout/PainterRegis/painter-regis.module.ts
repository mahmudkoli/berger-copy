import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PainterRegisListComponent } from './painter-regis-list/painter-regis-list.component';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PainterRegisRoutingModule } from './painterRegis-routing.module';



@NgModule({
  declarations: [PainterRegisListComponent],
  imports: [
      CommonModule,
      SharedMasterModule,
      PainterRegisRoutingModule,
      AngularFontAwesomeModule,
      NgbModule,
      FormsModule,
      ReactiveFormsModule,
  ]
})
export class PainterRegisModule { }
