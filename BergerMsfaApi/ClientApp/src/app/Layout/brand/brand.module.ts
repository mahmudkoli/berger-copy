import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { BrandListComponent } from './brand-list/brand-list.component';
import { BrandComponent } from './brand.component';
import { BrandRoutingModule } from './brand-routing.module';

@NgModule({
  declarations: [
    BrandListComponent,
    BrandComponent,
  ],
  imports: [
    CommonModule,
    BrandRoutingModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule
  ],
  entryComponents: [
  ]
})
export class BrandModule { }
