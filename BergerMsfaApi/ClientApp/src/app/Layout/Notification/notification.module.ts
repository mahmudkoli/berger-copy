import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NotificationRoutingModule } from './notification-routing.module';
import { NotificationDetailsComponent } from './notification-details/notification-details.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';


@NgModule({
  declarations: [NotificationDetailsComponent],
  imports: [
    CommonModule,
    NotificationRoutingModule,
    SharedMasterModule
  ]
})
export class NotificationModule { }
