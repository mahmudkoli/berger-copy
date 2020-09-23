import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PagesLayoutComponent } from './pages-layout.component';
import { ForgotPasswordBoxedComponent } from './forgot-password-boxed/forgot-password-boxed.component';
import { LoginBoxedComponent } from './login-boxed/login-boxed.component';
import { RegisterBoxedComponent } from './register-boxed/register-boxed.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { LoginComponent } from './login/login.component';



const routes: Routes = [{
  path: '',
  component: PagesLayoutComponent,
  children: [
   {path:'', redirectTo:"loginboxed"},
   { path: 'forgot-pass', component: ForgotPasswordBoxedComponent,data: {extraParameter: 'dashboardsMenu'} },
   { path: 'loginboxed', component: LoginBoxedComponent,data: {extraParameter: 'dashboardsMenu'} },
   { path: 'login', component: LoginComponent,data: {extraParameter: 'dashboardsMenu'} },
   { path: 'register', component: RegisterBoxedComponent,data: {extraParameter: 'dashboardsMenu'} },
   { path: 'unauthorized', component: UnauthorizedComponent,data: {extraParameter: 'dashboardsMenu'} },
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OutsideLayoutRoutingModule { }
