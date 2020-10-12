import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';

import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';

import { FocusdealerAddComponent } from './focusdealer-add/focusdealer-add.component';
import { FocusdealerListComponent } from './focusdealer-list/focusdealer-list.component';
import { FocusDealerRoutingModule } from './foucusDealer-routing.module';


//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';



@NgModule({
    declarations: [FocusdealerAddComponent, FocusdealerListComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        AngularFontAwesomeModule,
        NgbModule,
        FocusDealerRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
    ]
})
export class FocusDealerModule { }
