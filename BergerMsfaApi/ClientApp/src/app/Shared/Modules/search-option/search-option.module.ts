import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchOptionComponent } from './search-option.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
    imports: [
        CommonModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
    ],
    declarations: [
        SearchOptionComponent
    ],
    exports: [
        SearchOptionComponent
    ],
    providers:[]
})
export class SearchOptionModule {}
