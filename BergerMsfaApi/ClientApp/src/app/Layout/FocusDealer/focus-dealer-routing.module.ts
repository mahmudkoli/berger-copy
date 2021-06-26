import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FocusDealerListComponent } from './focus-dealer-list/focus-dealer-list.component';
import { FocusDealerAddComponent } from './focus-dealer-add/focus-dealer-add.component';
import { DealerOpeningListComponent } from './dealer-opening-list/dealer-opening-list.component';
import { DealerOpeningDetailComponent } from './dealer-opening-detail/dealer-opening-detail.component';
import { DealerListComponent } from './dealer-list/dealer-list.component';
import { DealerInfoLogDetailsComponent } from './dealer-info-log-details/dealer-info-log-details.component';
import { EmailConfigAddComponent } from './email-config-add/email-config-add.component';
import { EmailConfigListComponent } from './email-config-list/email-config-list.component';

const routes: Routes = [
    {
        path: '',
        children: [
            { 
                path: '', 
                redirectTo: 'focus-dealer-list' 
            },
            { 
                path: 'focus-dealer-list', 
                component: FocusDealerListComponent, 
                data: { title: 'Focus Dealer' } 
            },
            { 
                path: 'new-focus-dealer', 
                component: FocusDealerAddComponent, 
                data: { title: 'New Focus Dealer' } 
            },
            { 
                path: "edit-focus-dealer/:id", 
                component: FocusDealerAddComponent, 
                data: { title: 'Edit Focus Dealer' } 
            },
            { 
                path: "openingList/:id", 
                component: DealerOpeningDetailComponent 
            },
            { 
                path: "dealerList/:id", 
                component: DealerInfoLogDetailsComponent, 
                data: { title: 'Dealer Info log details' } 
            },
            
            { 
                path: "openingList", 
                component: DealerOpeningListComponent
            },
            { 
                path: "dealerList", 
                component: DealerListComponent
            },
            { 
                path: "addEmail", 
                component: EmailConfigAddComponent
            },
            { 
                path: "email", 
                component: EmailConfigListComponent
            },
            { 
                path: "addEmail/:id", 
                component: EmailConfigAddComponent
            },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FocusDealerRoutingModule { }