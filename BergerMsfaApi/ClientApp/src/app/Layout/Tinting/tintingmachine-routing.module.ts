import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TintingmachineListComponent } from './tintingmachine-list/tintingmachine-list.component';


const routes: Routes = [
    { path: 'tinting-machine-list', component: TintingmachineListComponent },
    //{ path: '/', redirectTo: "tinting-machine-list" },
    //{ path: 'tinting-machine-list', component: TintingmachineListComponent },
];

@NgModule({

  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TintingmachineRoutingModule { }
