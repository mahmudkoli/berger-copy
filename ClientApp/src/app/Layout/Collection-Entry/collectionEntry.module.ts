import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { CollectionEntryListComponent } from './collection-entry-list/collection-entry-list.component';
import { CollectionEntryRoutingModule } from './collectionEntry-routing';


//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [CollectionEntryListComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        AngularFontAwesomeModule,
        CollectionEntryRoutingModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
    ]
})

export class CollectionEntryModule { }
