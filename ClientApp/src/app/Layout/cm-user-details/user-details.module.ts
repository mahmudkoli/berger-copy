import { UserListComponent } from './user-list/user-list.component';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedMasterModule } from '../../Shared/Modules/shared-master/shared-master.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { UserDetailsRoutingModule } from './user-details-routing.module';;
import { CreateCmUserComponent } from './create-cm-user/create-user.component'
import { NgSelectModule } from '@ng-select/ng-select';

import { MustMatchDirective } from '../../Shared/Directive/mustmatch.directive';
import { EditCmUserComponent } from './edit-cm-user/edit-user.component';;
import { DelegationAddComponent } from './delegation-add/delegation-add.component';
import { DelegationListComponent } from './delegation-list/delegation-list.component'
;
import { ModalExcelImportCmUserComponent } from './modal-excel-import-cm-user/modal-excel-import-cm-user.component'
import { UserInfoComponent } from './user-info/user-info.component';

@NgModule({
    declarations: [
        UserListComponent,
        CreateCmUserComponent,
        EditCmUserComponent,
        MustMatchDirective,
        DelegationAddComponent,
        DelegationListComponent,
        ModalExcelImportCmUserComponent,
        UserInfoComponent
    ],
    imports: [
        CommonModule,
        SharedMasterModule,
        ReactiveFormsModule,
        FormsModule,
        UserDetailsRoutingModule,
        // Angular Bootstrap Components
        AngularFontAwesomeModule,
        NgbModule,
        NgSelectModule
    ],
    entryComponents: [
        ModalExcelImportCmUserComponent
    ]
})

export class UserDetailsModule {

}