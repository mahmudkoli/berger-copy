import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewDealerDevelopmentListComponent } from './new-dealer-development-list/new-dealer-development-list.component';
import { NewDealerDevelopmentComponent } from './new-dealer-development/new-dealer-development.component';


const routes: Routes = [
  { path: 'list', component: NewDealerDevelopmentListComponent },
  { path: 'add-edit', component: NewDealerDevelopmentComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KpiRoutingModule { }
