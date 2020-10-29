import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PainterRegisListComponent } from './painter-regis-list/painter-regis-list.component';
import { PainterRegisDetailComponent } from './painter-regis-detail/painter-regis-detail.component';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'register-list' },
            { path: 'register-list', component: PainterRegisListComponent, /*canActivate: [PermissionGuard], data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' }*/ },
            { path: "detail/:id", component: PainterRegisDetailComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PainterRegisRoutingModule { }