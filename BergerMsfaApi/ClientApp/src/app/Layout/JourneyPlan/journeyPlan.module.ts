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

//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [JourneyPlanListComponent, JourneyPlanAddComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        AngularFontAwesomeModule,
        NgbModule,
        JourneyPlanRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
    ]
})
export class JourneyPlanModule { }
