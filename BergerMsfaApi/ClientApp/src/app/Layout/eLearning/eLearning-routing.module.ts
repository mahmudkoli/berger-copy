import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/Shared/Guards/auth.guard';
import { PermissionGuard } from 'src/app/Shared/Guards/permission.guard';
import { ELearningFormComponent } from './eLearning-form-new/eLearning-form-new.component';
import { ELearningListComponent } from './eLearning-list/eLearning-list.component';
import { ELearningComponent } from './eLearning.component';


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
        component: ELearningFormComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'New ELearning', },
      },
      {
        path: 'edit/:id',
        component: ELearningFormComponent,
        // canActivate: [AuthGuard, PermissionGuard],
        data: { title: 'Edit ELearning', },
      },
    ],

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ELearningRoutingModule { }
