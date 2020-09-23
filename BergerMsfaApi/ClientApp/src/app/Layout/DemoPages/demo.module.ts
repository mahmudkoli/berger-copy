import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DemoRoutingModule } from './demo-routing.module';
import { AnalyticsComponent } from './Dashboards/analytics/analytics.component';
import { StandardComponent } from './Elements/Buttons/standard/standard.component';
import { DropdownsComponent } from './Elements/dropdowns/dropdowns.component';
import { CardsComponent } from './Elements/cards/cards.component';
import { ListGroupsComponent } from './Elements/list-groups/list-groups.component';
import { TimelineComponent } from './Elements/timeline/timeline.component';
import { IconsComponent } from './Elements/icons/icons.component';
import { AccordionsComponent } from './Components/accordions/accordions.component';
import { TabsComponent } from './Components/tabs/tabs.component';
import { CarouselComponent } from './Components/carousel/carousel.component';
import { ModalsComponent } from './Components/modals/modals.component';
import { ProgressBarComponent } from './Components/progress-bar/progress-bar.component';
import { PaginationComponent } from './Components/pagination/pagination.component';
import { TooltipsPopoversComponent } from './Components/tooltips-popovers/tooltips-popovers.component';
import { RegularComponent } from './Tables/regular/regular.component';
import { TablesMainComponent } from './Tables/tables-main/tables-main.component';
import { ChartBoxes3Component } from './Widgets/chart-boxes3/chart-boxes3.component';
import { ControlsComponent } from './Forms/Elements/controls/controls.component';
import { LayoutComponent } from './Forms/Elements/layout/layout.component';
import { ChartjsComponent } from './Charts/chartjs/chartjs.component';
import { LineChartComponent } from './Charts/chartjs/examples/line-chart/line-chart.component';
import { BarChartComponent } from './Charts/chartjs/examples/bar-chart/bar-chart.component';
import { ScatterChartComponent } from './Charts/chartjs/examples/scatter-chart/scatter-chart.component';
import { RadarChartComponent } from './Charts/chartjs/examples/radar-chart/radar-chart.component';
import { PolarAreaChartComponent } from './Charts/chartjs/examples/polar-area-chart/polar-area-chart.component';
import { BubbleChartComponent } from './Charts/chartjs/examples/bubble-chart/bubble-chart.component';
import { DynamicChartComponent } from './Charts/chartjs/examples/dynamic-chart/dynamic-chart.component';
import { DoughnutChartComponent } from './Charts/chartjs/examples/doughnut-chart/doughnut-chart.component';
import { PieChartComponent } from './Charts/chartjs/examples/pie-chart/pie-chart.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { ChartsModule } from 'ng2-charts';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { ExampleFormValidationComponent } from './example-form-validation/example-form-validation.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { AlertDemoComponent } from './alert-demo/alert-demo.component';
import { DataTableComponent } from './data-table/data-table.component';
import { TestCompComponent } from './test-comp/test-comp.component';
@NgModule({
  declarations: [
    AnalyticsComponent,
    StandardComponent,
    DropdownsComponent,
    CardsComponent,
    ListGroupsComponent,
    TimelineComponent,
    IconsComponent,
    //Components
    AccordionsComponent,
    TabsComponent,
    CarouselComponent,
    ModalsComponent,
    ProgressBarComponent,
    PaginationComponent,
    TooltipsPopoversComponent,
    //Tables
    RegularComponent,
    TablesMainComponent,
    // Widgets
    ChartBoxes3Component,
    // Forms Elements
    ControlsComponent,
    LayoutComponent,

    // // Charts
    ChartjsComponent,
    LineChartComponent,
    BarChartComponent,
    ScatterChartComponent,
    RadarChartComponent,
    PolarAreaChartComponent,
    BubbleChartComponent,
    DynamicChartComponent,
    DoughnutChartComponent,
    PieChartComponent,
    FetchDataComponent,
    ExampleFormValidationComponent,
    AlertDemoComponent,
    DataTableComponent,
    TestCompComponent

  ],
  imports: [
    NgSelectModule,
    CommonModule,
    DemoRoutingModule,
    SharedMasterModule,
    PerfectScrollbarModule,
     //Charts
    ChartsModule,
    AngularFontAwesomeModule,
    NgbModule,
    FormsModule,
    ReactiveFormsModule

  ]
})
export class DemoModule { }
