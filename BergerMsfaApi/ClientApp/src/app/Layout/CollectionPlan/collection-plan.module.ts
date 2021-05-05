import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CollectionConfigListComponent } from './collection-config-list/collection-config-list.component';
import { CollectionConfigAddComponent } from './collection-config-add/collection-config-add.component';
import { CollectionPlanListComponent } from './collection-plan-list/collection-plan-list.component';
import { CollectionPlanAddComponent } from './collection-plan-add/collection-plan-add.component';
import { CollectionPlanRoutingModule } from './collection-plan-routing.module';


@NgModule({
  declarations: [ 
    CollectionConfigListComponent, 
    CollectionConfigAddComponent, 
    CollectionPlanListComponent, 
    CollectionPlanAddComponent
  ],
  imports: [
    CommonModule,
      CollectionPlanRoutingModule,
      CommonModule,
      SharedMasterModule,
      AngularFontAwesomeModule,
      NgbModule,
      FormsModule,
      ReactiveFormsModule,
      NgSelectModule,
  ]
})
export class CollectionPlanModule { }
