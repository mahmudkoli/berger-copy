import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CollectionConfigAddComponent } from './collection-config-add/collection-config-add.component';
import { CollectionConfigListComponent } from './collection-config-list/collection-config-list.component';
import { CollectionPlanAddComponent } from './collection-plan-add/collection-plan-add.component';
import { CollectionPlanListComponent } from './collection-plan-list/collection-plan-list.component';

const routes: Routes = [
    { path: 'collection-plan-list', component: CollectionPlanListComponent },
    { path: 'collection-plan-add', component: CollectionPlanAddComponent },
    { path: 'collection-plan-edit/:id', component: CollectionPlanAddComponent },
    { path: 'collection-config-list', component: CollectionConfigListComponent },
    // { path: 'collection-config-add', component: CollectionConfigAddComponent },
    { path: 'collection-config-edit/:id', component: CollectionConfigAddComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CollectionPlanRoutingModule { }
