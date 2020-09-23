import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { WorkFlowRoutingModule } from "./work-flow-routing.module";
import { WorkFlowAddComponent } from "./work-flow-add/work-flow-add.component";
import { WorkFlowListComponent } from "./work-flow-list/work-flow-list.component";
import { SharedMasterModule } from "src/app/Shared/Modules/shared-master/shared-master.module";
import { AngularFontAwesomeModule } from "angular-font-awesome";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { WorkFlowTreeviewComponent } from "./work-flow-treeview/work-flow-treeview.component";
import { NgSelectModule } from "@ng-select/ng-select";
import { WorkflowConfigurationListComponent } from "./workflow-configuration-list/workflow-configuration-list.component";
import { WorkflowConfigurationAddComponent } from "./workflow-configuration-add/workflow-configuration-add.component";
import { OrganizationRoleListComponent } from './organization-role-list/organization-role-list.component';
import { OrganizationRoleAddComponent } from './organization-role-add/organization-role-add.component';
import { OrgRoleLinkWithUserComponent } from './org-role-link-with-user/org-role-link-with-user.component';
import { OrganizationRoleEditComponent } from './organization-role-edit/organization-role-edit.component';

@NgModule({
  declarations: [
    WorkFlowAddComponent,
    WorkFlowListComponent,
    WorkFlowTreeviewComponent,
    WorkflowConfigurationListComponent,
    WorkflowConfigurationAddComponent,
    OrganizationRoleListComponent,
    OrganizationRoleAddComponent,
    OrganizationRoleEditComponent,
    OrgRoleLinkWithUserComponent
  ],
  imports: [
    CommonModule,
    WorkFlowRoutingModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
  ],
})
export class WorkFlowModule {}
