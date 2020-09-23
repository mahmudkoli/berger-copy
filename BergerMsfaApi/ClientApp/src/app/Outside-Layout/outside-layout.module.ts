import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {LoadingBarRouterModule} from '@ngx-loading-bar/router';
import { OutsideLayoutRoutingModule } from './outside-layout-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// // Pages

import { ForgotPasswordBoxedComponent } from './forgot-password-boxed/forgot-password-boxed.component';
import { LoginBoxedComponent } from './login-boxed/login-boxed.component';
import { RegisterBoxedComponent } from './register-boxed/register-boxed.component';
import { PagesLayoutComponent } from './pages-layout.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './login/login.component';

@NgModule({
  declarations: [
    PagesLayoutComponent,
    ForgotPasswordBoxedComponent,
    LoginBoxedComponent,
    RegisterBoxedComponent,
    UnauthorizedComponent,
    LoginComponent,
  ],
  imports: [
    CommonModule,
    OutsideLayoutRoutingModule,
    LoadingBarRouterModule,
    FormsModule,
    NgbModule
  ]
})
export class OutsideLayoutModule { }
