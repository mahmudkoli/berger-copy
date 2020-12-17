import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ELearningListComponent } from './eLearning-list/eLearning-list.component';
import { ELearningFormComponent } from './eLearning-form-new/eLearning-form-new.component';
import { ELearningComponent } from './eLearning.component';
import { ELearningRoutingModule } from './eLearning-routing.module';
import { FileUploadModule } from 'primeng/fileupload';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [
    ELearningListComponent,
    ELearningFormComponent,
    ELearningComponent
  ],
  imports: [
    CommonModule,
    ELearningRoutingModule,
    SharedMasterModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    FileUploadModule
  ],
  entryComponents: [
  ]
})
export class ELearningModule { }
