import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { DropdownAddComponent } from './dropdown-add/dropdown-add.component';
import { DropdownListComponent } from './dropdown-list/dropdown-list.component';
import { SetupRoutingModule } from './setup-routing.module';
import { SyncSetupComponent } from './sync-setup/sync-setup.component';
import { SyncSetupEditComponent } from './sync-setup-edit/sync-setup-edit.component';

//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';

@NgModule({
  declarations: [
    DropdownListComponent,
    DropdownAddComponent,
    SyncSetupComponent,
    SyncSetupEditComponent,
  ],
  imports: [
    CommonModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    SetupRoutingModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
  ],
})
export class SetupModule {}
