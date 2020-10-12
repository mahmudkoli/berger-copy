import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PainterRegisListComponent } from './painter-regis-list/painter-regis-list.component';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'painter-list' },
            { path: 'painter-list', component: PainterRegisListComponent, /*canActivate: [PermissionGuard], data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' }*/ },

        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PainterRegisRoutingModule { }