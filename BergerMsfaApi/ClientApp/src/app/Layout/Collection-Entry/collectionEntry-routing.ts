import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CollectionEntryEditComponent } from './collection-entry-edit/collection-entry-edit.component';
import { CollectionEntryListComponent } from './collection-entry-list/collection-entry-list.component';




const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'payment-list' },
            {
                path: 'payment-list',
                component: CollectionEntryListComponent,
                //data: { extraParameter: 'collection', permissionType: 'view', permissionGroup: 'collection/collection-list' 
            },
            {
                path: 'payment-details/:id',
                component: CollectionEntryEditComponent,
                //data: { extraParameter: 'collection', permissionType: 'view', permissionGroup: 'collection/collection-list' 
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CollectionEntryRoutingModule { }