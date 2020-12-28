import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ELearningListComponent } from './eLearning-list/eLearning-list.component';
import { ELearningFormNewComponent } from './eLearning-form-new/eLearning-form-new.component';
import { ELearningComponent } from './eLearning.component';
import { ELearningRoutingModule } from './eLearning-routing.module';
import { FileUploadModule } from 'primeng/fileupload';
import { NgSelectModule } from '@ng-select/ng-select';
import { ELearningFormEditComponent } from './eLearning-form-edit/eLearning-form-edit.component';
import { QuestionListComponent } from './question-list/question-list.component';
import { QuestionFormComponent } from './question-form/question-form.component';
import { ModalQuestionOptionFormComponent } from './modal-question-option-form/modal-question-option-form.component';

@NgModule({
  declarations: [
    ELearningListComponent,
    ELearningFormNewComponent,
    ELearningFormEditComponent,
    QuestionListComponent,
    QuestionFormComponent,
    ELearningComponent,
    ModalQuestionOptionFormComponent
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
    ModalQuestionOptionFormComponent
  ]
})
export class ELearningModule { }
