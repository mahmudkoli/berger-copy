import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BaseLayoutComponent } from './LayoutComponent/base-layout/base-layout.component';
import { AuthGuard } from '../Shared/Guards/auth.guard';
import { PermissionGuard } from '../Shared/Guards/permission.guard';

const routes: Routes = [
    {
        path: '',
        component: BaseLayoutComponent,
        canActivate: [AuthGuard],
        children: [

            { path: '', redirectTo: 'menu' },
            // { path: '', component: MenuListComponent,data: {extraParameter: 'dashboardsMenu'} },

            { path: 'menu', loadChildren: () => import('./menu-details/menu-details.module').then(m => m.MenuDetailsModule) },
            // tslint:disable-next-line:max-line-length
            { path: 'product', loadChildren: () => import('./product-details/product-details.module').then(m => m.ProductDetailsModule) },
            // tslint:disable-next-line:max-line-length
            { path: 'role', loadChildren: () => import('./role-details/role-details.module').then(m => m.RoleDetailsModule) },
            { path: 'demo', loadChildren: () => import('./DemoPages/demo.module').then(m => m.DemoModule) },
            { path: 'users-info', loadChildren: () => import('./user-info/user-info.module').then(m => m.UserInfoModule) },
            { path: 'notification', loadChildren: () => import('./notification/notification.module').then(m => m.NotificationModule) },
            { path: 'setup', loadChildren: () => import('./setup/setup.module').then(m => m.SetupModule) },
            { path: 'collection', loadChildren: () => import('./Collection-Entry/collectionEntry.module').then(m => m.CollectionEntryModule) },
            { path: 'journey-plan', loadChildren: () => import('./JourneyPlan/journeyPlan.module').then(m => m.JourneyPlanModule) },
            { path: 'dealer', loadChildren: () => import('./FocusDealer/focusDealer.module').then(m => m.FocusDealerModule) },
            { path: 'painter', loadChildren: () => import('./PainterRegis/painter-regis.module').then(m => m.PainterRegisModule) },
            { path: 'scheme', loadChildren: () => import('./Scheme/scheme.module').then(m => m.SchemeModule) },
            { path: 'tinting', loadChildren: () => import('./Tinting/tintingmachine.module').then(m => m.TintingmachineModule) },
            { path: 'eLearning', loadChildren: () => import('./eLearning/eLearning.module').then(m => m.ELearningModule) },
            { path: 'brand', loadChildren: () => import('./brand/brand.module').then(m => m.BrandModule) },
            { path: 'dealer-sales-call', loadChildren: () => import('./dealer-sales-call/dealer-sales-call.module').then(m => m.DealerSalesCallModule) },
            { path: 'lead', loadChildren: () => import('./demand-generation/lead.module').then(m => m.LeadModule) },
            { path: 'access-denied', loadChildren: () => import('./access-denied/access-denied.module').then(m => m.AccessDeniedModule) },

        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule { }
