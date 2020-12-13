import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TintingmachineRoutingModule } from './tintingmachine-routing.module';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TintingmachineListComponent } from './tintingmachine-list/tintingmachine-list.component';


@NgModule({
    declarations: [TintingmachineListComponent],
  imports: [
      CommonModule,
      CommonModule,
      SharedMasterModule,
      TintingmachineRoutingModule,
      AngularFontAwesomeModule,
      NgbModule,
      FormsModule,
      ReactiveFormsModule,
      NgSelectModule,
    
  ]
})
export class TintingmachineModule { }
