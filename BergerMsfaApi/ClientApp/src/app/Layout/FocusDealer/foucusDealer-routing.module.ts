import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FocusdealerListComponent } from './focusdealer-list/focusdealer-list.component';
import { FocusdealerAddComponent } from './focusdealer-add/focusdealer-add.component';




const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'list' },
            { path: 'list', component: FocusdealerListComponent, data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' } },
            { path: 'add', component: FocusdealerAddComponent, data: { permissionType: 'create', permissionGroup: 'product/product-list' } },
            { path: "add/:id", component: FocusdealerAddComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FocusDealerRoutingModule { }