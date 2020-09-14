import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProductDetailsRoutingModule } from './product-details-routing.module';
import { ProductListComponent } from './product-list/product-list.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { ProductAddComponent } from './product-add/product-add.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [ProductListComponent, ProductAddComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        ProductDetailsRoutingModule,
        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
        //NgOptionHighlightModule,
    ]
})
export class ProductDetailsModule { }
