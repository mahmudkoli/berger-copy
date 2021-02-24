import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SchememasterListComponent } from './schememaster-list/schememaster-list.component';
import { SchememasterAddComponent } from './schememaster-add/schememaster-add.component';
import { SchemedetailAddComponent } from './schemedetail-add/schemedetail-add.component';
import { SchemedetailListComponent } from './schemedetail-list/schemedetail-list.component';


const routes: Routes = [

    { path: 'detail-list', component: SchemedetailListComponent },
    { path: 'detail-add', component: SchemedetailAddComponent },
    { path: 'detail-edit/:id', component: SchemedetailAddComponent },
    { path: 'master-list', component: SchememasterListComponent },
    { path: 'master-add', component: SchememasterAddComponent },
    { path: 'master-edit/:id', component: SchememasterAddComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SchemeRoutingModule { }
