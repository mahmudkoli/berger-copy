import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../Shared/Guards/auth.guard';
import { BaseLayoutComponent } from './LayoutComponent/base-layout/base-layout.component';

const routes: Routes = [
  {
    path: '',
    component: BaseLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'journey-plan' },
      // { path: '', component: MenuListComponent,data: {extraParameter: 'dashboardsMenu'} },

      {
        path: 'dashboard',
        loadChildren: () =>
          import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },
      {
        path: 'menu',
        loadChildren: () =>
          import('./menu-details/menu-details.module').then(
            (m) => m.MenuDetailsModule
          ),
      },
      // tslint:disable-next-line:max-line-length
      // { path: 'product', loadChildren: () => import('./product-details/product-details.module').then(m => m.ProductDetailsModule) },
      // tslint:disable-next-line:max-line-length
      {
        path: 'role',
        loadChildren: () =>
          import('./role-details/role-details.module').then(
            (m) => m.RoleDetailsModule
          ),
      },
      {
        path: 'demo',
        loadChildren: () =>
          import('./DemoPages/demo.module').then((m) => m.DemoModule),
      },
      {
        path: 'users-info',
        loadChildren: () =>
          import('./user-info/user-info.module').then((m) => m.UserInfoModule),
      },
      {
        path: 'notification',
        loadChildren: () =>
          import('./notification/notification.module').then(
            (m) => m.NotificationModule
          ),
      },
      {
        path: 'setup',
        loadChildren: () =>
          import('./setup/setup.module').then((m) => m.SetupModule),
      },
      {
        path: 'collection',
        loadChildren: () =>
          import('./Collection-Entry/collectionEntry.module').then(
            (m) => m.CollectionEntryModule
          ),
      },
      {
        path: 'journey-plan',
        loadChildren: () =>
          import('./JourneyPlan/journeyPlan.module').then(
            (m) => m.JourneyPlanModule
          ),
      },
      {
        path: 'dealer',
        loadChildren: () =>
          import('./FocusDealer/focus-dealer.module').then(
            (m) => m.FocusDealerModule
          ),
      },
      {
        path: 'painter',
        loadChildren: () =>
          import('./PainterRegis/painter-regis.module').then(
            (m) => m.PainterRegisModule
          ),
      },
      {
        path: 'scheme',
        loadChildren: () =>
          import('./Scheme/scheme.module').then((m) => m.SchemeModule),
      },
      {
        path: 'tinting',
        loadChildren: () =>
          import('./Tinting/tintingmachine.module').then(
            (m) => m.TintingmachineModule
          ),
      },
      {
        path: 'eLearning',
        loadChildren: () =>
          import('./eLearning/eLearning.module').then((m) => m.ELearningModule),
      },
      {
        path: 'brand',
        loadChildren: () =>
          import('./brand/brand.module').then((m) => m.BrandModule),
      },
      {
        path: 'dealer-sales-call',
        loadChildren: () =>
          import('./dealer-sales-call/dealer-sales-call.module').then(
            (m) => m.DealerSalesCallModule
          ),
      },
      {
        path: 'lead',
        loadChildren: () =>
          import('./demand-generation/lead.module').then((m) => m.LeadModule),
      },
      {
        path: 'report',
        loadChildren: () =>
          import('./report/report.module').then((m) => m.ReportModule),
      },
      {
        path: 'access-denied',
        loadChildren: () =>
          import('./access-denied/access-denied.module').then(
            (m) => m.AccessDeniedModule
          ),
      },
      {
        path: 'collection-plan',
        loadChildren: () =>
          import('./CollectionPlan/collection-plan.module').then(
            (m) => m.CollectionPlanModule
          ),
      },
      {
        path: 'universe-reach-analysis',
        loadChildren: () =>
          import('./UniverseReachAnalysis/universe-reach-analysis.module').then(
            (m) => m.UniverseReachAnalysisModule
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class LayoutRoutingModule {}
