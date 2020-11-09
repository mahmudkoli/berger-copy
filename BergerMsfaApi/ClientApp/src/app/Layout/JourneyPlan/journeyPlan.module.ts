import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
import { JourneyPlanListComponent } from './journey-plan-list/journey-plan-list.component';
import { JourneyPlanAddComponent } from './journey-plan-add/journey-plan-add.component';
import { JourneyPlanRoutingModule } from './journeyPlan-routing.module';
import { JourneyPlanDetailComponent } from './journey-plan-detail/journey-plan-detail.component';
import { StatusPipe } from '../../Shared/Pipes/status-filter.pipe';
import { JourneyPlanListLineManagerComponent } from './journey-plan-list-line-manager/journey-plan-list-line-manager.component';




@NgModule({
    declarations: [JourneyPlanListComponent, JourneyPlanAddComponent, StatusPipe,JourneyPlanDetailComponent, JourneyPlanListLineManagerComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        AngularFontAwesomeModule,
        NgbModule,
        JourneyPlanRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule,
        
   
    ],
    entryComponents: []
})
export class JourneyPlanModule { }
