import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
 import { AnalyticsComponent } from './Dashboards/analytics/analytics.component';
import { DropdownsComponent } from './Elements/dropdowns/dropdowns.component';
import { StandardComponent } from './Elements/Buttons/standard/standard.component';
import { CardsComponent } from './Elements/cards/cards.component';
import { ListGroupsComponent } from './Elements/list-groups/list-groups.component';
import { TimelineComponent } from './Elements/timeline/timeline.component';
import { IconsComponent } from './Elements/icons/icons.component';
import { TabsComponent } from './Components/tabs/tabs.component';
import { AccordionsComponent } from './Components/accordions/accordions.component';
import { ModalsComponent } from './Components/modals/modals.component';
import { ProgressBarComponent } from './Components/progress-bar/progress-bar.component';
import { TooltipsPopoversComponent } from './Components/tooltips-popovers/tooltips-popovers.component';
import { CarouselComponent } from './Components/carousel/carousel.component';
import { PaginationComponent } from './Components/pagination/pagination.component';
import { ControlsComponent } from './Forms/Elements/controls/controls.component';
import { LayoutComponent } from './Forms/Elements/layout/layout.component';
import { ChartBoxes3Component } from './Widgets/chart-boxes3/chart-boxes3.component';
import { TablesMainComponent } from './Tables/tables-main/tables-main.component';
import { ChartjsComponent } from './Charts/chartjs/chartjs.component';
import { ExampleFormValidationComponent } from './example-form-validation/example-form-validation.component';
import { AlertDemoComponent } from './alert-demo/alert-demo.component';
import { DataTableComponent } from './data-table/data-table.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {path:'', redirectTo:'analytics'},
      { path: 'dropdowns', component: DropdownsComponent,data: {extraParameter: 'dashboardsMenu'} },
      { path: 'analytics', component: AnalyticsComponent,data: {extraParameter: 'analytics'} },
      { path: 'buttons-standard', component: StandardComponent,data: {extraParameter: 'buttons-standard'} },
      { path: 'cards', component: CardsComponent,data: {extraParameter: 'cards'} },
      { path: 'list-group', component: ListGroupsComponent,data: {extraParameter: 'list-group'} },
      { path: 'timeline', component: TimelineComponent,data: {extraParameter: 'timeline'} },
      { path: 'icons', component: IconsComponent,data: {extraParameter: 'icons'} },

      { path: 'accordions', component: AccordionsComponent,data: {extraParameter: 'accordions'} },
      { path: 'tabs', component: TabsComponent,data: {extraParameter: 'tabs'} },
      { path: 'carousel', component: CarouselComponent,data: {extraParameter: 'carousel'} },
      { path: 'modals', component: ModalsComponent,data: {extraParameter: 'modals'} },
      { path: 'progress-bar', component: ProgressBarComponent,data: {extraParameter: 'progress-bar'} },
      { path: 'pagination', component: PaginationComponent,data: {extraParameter: 'pagination'} },
      { path: 'tooltips-popovers', component: TooltipsPopoversComponent,data: {extraParameter: 'tooltips-popovers'} },

      //{ path: 'bootstrap-table', component: RegularComponent,data: {extraParameter: 'bootstrap-table'} },
      { path: 'bootstrap-table', component: TablesMainComponent,data: {extraParameter: 'bootstrap-table'} },

      { path: 'chart-boxes-3', component: ChartBoxes3Component,data: {extraParameter: 'chart-boxes-3'} },

      { path: 'controls', component: ControlsComponent,data: {extraParameter: 'controls'} },
      { path: 'layouts', component: LayoutComponent,data: {extraParameter: 'layouts'} },


      { path: 'chartjs', component: ChartjsComponent,data: {extraParameter: 'chartjs'} },
      { path: 'form-validation', component: ExampleFormValidationComponent },
      { path: 'alert', component: AlertDemoComponent },
      { path: 'data-table', component: DataTableComponent },
    ]
  }
];



@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DemoRoutingModule { }
