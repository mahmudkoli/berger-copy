import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { UserInfoRoutingModule } from './user-info-routing.module';
import { UserInfoFormComponent } from './user-info-form/user-info-form.component';
import { UserInfoListComponent } from './user-info-list/user-info-list.component';
import { NgSelectModule } from '@ng-select/ng-select';



@NgModule({
    declarations:
        [
            UserInfoFormComponent,
            UserInfoListComponent,
        ],
    imports: [
        CommonModule,
        SharedMasterModule,
        FormsModule,
        ReactiveFormsModule,
        UserInfoRoutingModule,
        AngularFontAwesomeModule,
        NgbModule,
        NgSelectModule 
    ]
})

export class UserInfoModule {

}