import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DropdownAddComponent } from './dropdown-add/dropdown-add.component';
import { DropdownListComponent } from './dropdown-list/dropdown-list.component';
import { SyncSetupEditComponent } from './sync-setup-edit/sync-setup-edit.component';
import { SyncSetupComponent } from './sync-setup/sync-setup.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', redirectTo: 'dropdown-list' },
      {
        path: 'dropdown-list',
        component: DropdownListComponent,
        data: {
          extraParameter: 'product',
          permissionType: 'view',
          permissionGroup: 'product/product-list',
        },
      },
      {
        path: 'dropdown-add',
        component: DropdownAddComponent,
        data: {
          permissionType: 'create',
          permissionGroup: 'product/product-list',
        },
      },
      {
        path: 'dropdown-add/:id',
        component: DropdownAddComponent,
        data: {
          permissionType: 'update',
          permissionGroup: 'product/product-list',
        },
      },
      { path: 'sync', component: SyncSetupComponent },
      { path: 'sync-setup-edit/:id', component: SyncSetupEditComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SetupRoutingModule {}
