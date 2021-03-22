import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FocusdealerListComponent } from './focusdealer-list/focusdealer-list.component';
import { FocusdealerAddComponent } from './focusdealer-add/focusdealer-add.component';
import { DealerOpeningListComponent } from './dealer-opening-list/dealer-opening-list.component';
import { DealerOpeningDetailComponent } from './dealer-opening-detail/dealer-opening-detail.component';
import { DealerListComponent } from './dealer-list/dealer-list.component';
import { EmailConfigAddComponent } from './email-config-add/email-config-add.component';
import { EmailConfigListComponent } from './email-config-list/email-config-list.component';




const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', redirectTo: 'focusdealer-list' },
            { path: 'focusdealer-list', component: FocusdealerListComponent, data: { extraParameter: 'product', permissionType: 'view', permissionGroup: 'product/product-list' } },
            { path: 'add-focusdealer', component: FocusdealerAddComponent, data: { permissionType: 'create', permissionGroup: 'product/product-list' } },
            { path: "add-focusdealer/:id", component: FocusdealerAddComponent, data: { permissionType: 'update', permissionGroup: 'product/product-list' } },
            { path: "openingList/:id", component: DealerOpeningDetailComponent },
            { path: "openingList", component: DealerOpeningListComponent},
            { path: "dealerList", component: DealerListComponent},
            { path: "addEmail", component: EmailConfigAddComponent},
            { path: "email", component: EmailConfigListComponent},
            { path: "addEmail/:id", component: EmailConfigAddComponent},



        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FocusDealerRoutingModule { }