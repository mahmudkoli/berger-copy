import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';

import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
import { SetupRoutingModule } from './setup-routing.module';
import { DropdownListComponent } from './dropdown-list/dropdown-list.component';
import { DropdownAddComponent } from './dropdown-add/dropdown-add.component';
import { EmailConfigForDealerOppeningComponent } from './email-config-for-dealer-oppening/email-config-for-dealer-oppening.component';

//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [DropdownListComponent, DropdownAddComponent, EmailConfigForDealerOppeningComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        AngularFontAwesomeModule,
        SetupRoutingModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
    ]
})
export class SetupModule { }
