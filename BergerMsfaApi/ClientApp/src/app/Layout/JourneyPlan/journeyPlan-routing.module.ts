import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { JourneyPlanListComponent } from './journey-plan-list/journey-plan-list.component';
import { JourneyPlanAddComponent } from './journey-plan-add/journey-plan-add.component';



const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'list' },
            { path: 'list', component: JourneyPlanListComponent, data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' } },
            { path: 'add', component: JourneyPlanAddComponent, data: { permissionType: 'create', permissionGroup: 'product/product-list' } },
            { path: "add/:id", component: JourneyPlanAddComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class JourneyPlanRoutingModule { }