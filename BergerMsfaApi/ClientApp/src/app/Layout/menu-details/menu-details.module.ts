import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MenuDetailsRoutingModule } from './menu-details-routing.module';
import { MenuListComponent } from './menu-list/menu-list.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalMenuComponent } from './modal-menu/modal-menu.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { MenuPermissionsComponent } from './menu-permissions/menu-permissions.component';
import { MenuActivityPermissionsComponent } from './menu-activity-permissions/menu-activity-permissions.component';
import { MenuActivityListComponent } from './menu-activity-list/menu-activity-list.component';
import { ModalMenuActivityComponent } from './modal-menu-activity/modal-menu-activity.component';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';

@NgModule({
  declarations: [
    MenuListComponent,
    ModalMenuComponent,
    MenuPermissionsComponent,
    MenuActivityPermissionsComponent,
    MenuActivityListComponent,
        ModalMenuActivityComponent
       
  ],
  imports: [
    CommonModule,
    MenuDetailsRoutingModule,
    NgbModule,
    FormsModule,
    NgSelectModule,
      SharedMasterModule,
      ReactiveFormsModule,
      NgSelectModule
  ],
  entryComponents: [
      ModalMenuComponent,
      ModalMenuActivityComponent
  ]
})
export class MenuDetailsModule { }
