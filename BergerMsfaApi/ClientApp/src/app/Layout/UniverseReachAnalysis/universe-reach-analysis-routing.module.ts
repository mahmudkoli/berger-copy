import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UniverseReachAnalysisAddComponent } from './universe-reach-analysis-add/universe-reach-analysis-add.component';
import { UniverseReachAnalysisListComponent } from './universe-reach-analysis-list/universe-reach-analysis-list.component';

const routes: Routes = [
    { path: 'universe-reach-analysis-list', component: UniverseReachAnalysisListComponent },
    { path: 'universe-reach-analysis-add', component: UniverseReachAnalysisAddComponent },
    { path: 'universe-reach-analysis-edit/:id', component: UniverseReachAnalysisAddComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UniverseReachAnalysisRoutingModule { }
