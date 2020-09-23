import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import{NotificationDetailsComponent} from './notification-details/notification-details.component'


const routes: Routes = [
  {
    path: '',
    children:
    [ 
            { path: '', redirectTo: 'users-list' },
            { path: 'notification-details', component: NotificationDetailsComponent }
          
    ]
}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NotificationRoutingModule { }
