import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductAddComponent } from './product-add/product-add.component';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'product-list' },
            { path: 'product-list', component: ProductListComponent, canActivate: [PermissionGuard], data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' } },
            { path: 'product-add', component: ProductAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'create', permissionGroup: 'product/product-list' } },
            { path: "product-add/:id", component: ProductAddComponent, canActivate: [PermissionGuard], data: { permissionType: 'update', permissionGroup: 'product/product-list' } }
        ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductDetailsRoutingModule { }