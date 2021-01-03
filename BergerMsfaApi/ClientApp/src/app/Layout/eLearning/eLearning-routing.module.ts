import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { ELearningFormEditComponent } from './eLearning-form-edit/eLearning-form-edit.component';
import { ELearningFormNewComponent } from './eLearning-form-new/eLearning-form-new.component';
import { ELearningListComponent } from './eLearning-list/eLearning-list.component';
import { ELearningComponent } from './eLearning.component';
import { QuestionFormComponent } from './question-form/question-form.component';
import { QuestionListComponent } from './question-list/question-list.component';
import { QuestionSetFormComponent } from './question-set-form/question-set-form.component';
import { QuestionSetListComponent } from './question-set-list/question-set-list.component';


const routes: Routes = [
  {
    path: '',
    component: ELearningComponent,
    // canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'list',
        pathMatch: 'full',
      },
      {
        path: 'list',
        component: ELearningListComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'ELearning', },
      },
      {
        path: 'new',
        component: ELearningFormNewComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'New ELearning', },
      },
      {
        path: 'edit/:id',
        component: ELearningFormEditComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Edit ELearning', },
      },
      {
        path: 'question/list',
        component: QuestionListComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Question', },
      },
      {
        path: 'question/new',
        component: QuestionFormComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'New Question', },
      },
      {
        path: 'question/edit/:id',
        component: QuestionFormComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Edit Question', },
      },
      {
        path: 'questionSet/list',
        component: QuestionSetListComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Question Set', },
      },
      {
        path: 'questionSet/new',
        component: QuestionSetFormComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'New Question Set', },
      },
      {
        path: 'questionSet/edit/:id',
        component: QuestionSetFormComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Edit Question Set', },
      },
    ],

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ELearningRoutingModule { }
