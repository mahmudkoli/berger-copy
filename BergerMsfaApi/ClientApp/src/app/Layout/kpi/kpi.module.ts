import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { KpiRoutingModule } from './kpi-routing.module';
import { NewDealerDevelopmentComponent } from './new-dealer-development/new-dealer-development.component';
import { NewDealerDevelopmentListComponent } from './new-dealer-development-list/new-dealer-development-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';


@NgModule({
  declarations: [NewDealerDevelopmentComponent, NewDealerDevelopmentListComponent],
  imports: [
    CommonModule,
    KpiRoutingModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
  ]
})
export class KpiModule { }
