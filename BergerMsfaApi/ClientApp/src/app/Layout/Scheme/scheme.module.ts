import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SchemeRoutingModule } from './scheme-routing.module';
import { SchememasterListComponent } from './schememaster-list/schememaster-list.component';
import { SchememasterAddComponent } from './schememaster-add/schememaster-add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SchemedetailListComponent } from './schemedetail-list/schemedetail-list.component';
import { SchemedetailAddComponent } from './schemedetail-add/schemedetail-add.component';


@NgModule({
  declarations: [ SchememasterListComponent, SchememasterAddComponent, SchemedetailListComponent, SchemedetailAddComponent],
  imports: [
    CommonModule,
      SchemeRoutingModule,
      CommonModule,
      SharedMasterModule,
      AngularFontAwesomeModule,
      NgbModule,
      FormsModule,
      ReactiveFormsModule,
      NgSelectModule,
  ]
})
export class SchemeModule { }
