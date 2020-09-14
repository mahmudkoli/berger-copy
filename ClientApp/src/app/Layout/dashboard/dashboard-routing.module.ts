import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CommonComponent } from './common/common.component';


const routes: Routes = [
  {
    path: '',
    children: [
        { path: '', redirectTo: 'common' },
        { path: 'common', component: CommonComponent, data: { extraParameter: 'dashboard' } },
       
    ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
