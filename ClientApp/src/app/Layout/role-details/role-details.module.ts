import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RoleDetailsRoutingModule } from './role-details-routing.module';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleAddComponent } from './role-add/role-add.component';
import { RoleLinkWithUserComponent } from './role-link-with-user/role-link-with-user.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgSelectModule } from '@ng-select/ng-select';
//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';


@NgModule({
    declarations: [RoleListComponent, RoleAddComponent, RoleLinkWithUserComponent],
    imports: [
        CommonModule,
        SharedMasterModule,
        RoleDetailsRoutingModule,
        AngularFontAwesomeModule,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule
        //NgOptionHighlightModule,
    ]
})
export class RoleDetailsModule { }