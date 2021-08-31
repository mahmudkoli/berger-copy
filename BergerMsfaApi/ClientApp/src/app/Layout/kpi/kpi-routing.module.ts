import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ColorBankTargetSetupComponent } from './color-bank-target-setup/color-bank-target-setup.component';
import { NewDealerDevelopmentComponent } from './new-dealer-development/new-dealer-development.component';

const routes: Routes = [
  {
    path: 'dealer-opening-status/add-edit',
    component: NewDealerDevelopmentComponent,
  },
  { path: 'color-bank-target-setup', component: ColorBankTargetSetupComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class KpiRoutingModule {}
