import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { DealerSalesCallRoutingModule } from './dealer-sales-call-routing.module';
import { DealerSalesCallComponent } from './dealer-sales-call.component';
import { DealerSalesCallListComponent } from './dealer-sales-call-list/dealer-sales-call-list.component';
import { DealerSalesCallDetailsComponent } from './dealer-sales-call-details/dealer-sales-call-details.component';
import { DealerSalesCallEmailConfigListComponent } from './dealer-sales-call-email-config-list/dealer-sales-call-email-config-list.component';
import { DealerSalesCallEmailConfigAddComponent } from './dealer-sales-call-email-config-add/dealer-sales-call-email-config-add.component';

@NgModule({
  declarations: [
    DealerSalesCallListComponent,
    DealerSalesCallDetailsComponent,
    DealerSalesCallComponent,
    DealerSalesCallEmailConfigListComponent,
    DealerSalesCallEmailConfigAddComponent
  ],
  imports: [
    CommonModule,
    DealerSalesCallRoutingModule,
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
export class DealerSalesCallModule { }
