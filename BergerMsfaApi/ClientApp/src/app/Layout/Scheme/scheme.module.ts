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
import { SchememasterListPngComponent } from './schememaster-list-png/schememaster-list.component';
import { SchemedetailListPngComponent } from './schemedetail-list-png/schemedetail-list.component';


@NgModule({
  declarations: [ 
    SchememasterListComponent, 
    SchememasterListPngComponent, 
    SchememasterAddComponent, 
    SchemedetailListComponent, 
    SchemedetailListPngComponent, 
    SchemedetailAddComponent
  ],
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
