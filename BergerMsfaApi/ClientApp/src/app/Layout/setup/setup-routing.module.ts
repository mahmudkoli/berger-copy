import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DropdownListComponent } from './dropdown-list/dropdown-list.component';
import { DropdownAddComponent } from './dropdown-add/dropdown-add.component';



const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'dropdown-list' },
            { path: 'dropdown-list', component: DropdownListComponent, data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' } },
            { path: 'dropdown-add', component: DropdownAddComponent, data: { permissionType: 'create', permissionGroup: 'product/product-list' } },
            { path: "dropdown-add/:id", component: DropdownAddComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SetupRoutingModule { }