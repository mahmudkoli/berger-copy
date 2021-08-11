import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UniverseReachAnalysisListComponent } from './universe-reach-analysis-list/universe-reach-analysis-list.component';
import { UniverseReachAnalysisAddComponent } from './universe-reach-analysis-add/universe-reach-analysis-add.component';
import { UniverseReachAnalysisRoutingModule } from './universe-reach-analysis-routing.module';

@NgModule({
  declarations: [ 
    UniverseReachAnalysisListComponent, 
    UniverseReachAnalysisAddComponent
  ],
  imports: [
    CommonModule,
    UniverseReachAnalysisRoutingModule,
    CommonModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
  ]
})
export class UniverseReachAnalysisModule { }
