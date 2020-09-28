import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CollectionEntryListComponent } from './collection-entry-list/collection-entry-list.component';




const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'collection-list' },
            {
                path: 'collection-list',
                component: CollectionEntryListComponent,
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