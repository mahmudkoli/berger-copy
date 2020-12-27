import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { JourneyPlanListComponent } from './journey-plan-list/journey-plan-list.component';
import { JourneyPlanAddComponent } from './journey-plan-add/journey-plan-add.component';
import { JourneyPlanDetailComponent } from './journey-plan-detail/journey-plan-detail.component';
import { JourneyPlanListLineManagerComponent } from './journey-plan-list-line-manager/journey-plan-list-line-manager.component';
import { JouneryPlanLinemanagerDetailComponent } from './jounery-plan-linemanager-detail/jounery-plan-linemanager-detail.component';



const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'list' },
            { path: 'list', component: JourneyPlanListComponent, data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' } },
            { path: 'add', component: JourneyPlanAddComponent, data: { permissionType: 'create', permissionGroup: 'product/product-list' } },
            { path: "add/:date", component: JourneyPlanAddComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } },
            { path: "detail/:id", component: JourneyPlanDetailComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } },
            { path: "line-manager", component: JourneyPlanListLineManagerComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } },
            { path: "line-manager-detail/:id", component: JouneryPlanLinemanagerDetailComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } }
        ]
    }
];
 
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class JourneyPlanRoutingModule { }