import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { CommonComponent } from './common/common.component';
import { StandardComponent } from '../DemoPages/Elements/Buttons/standard/standard.component';
import { DropdownsComponent } from '../DemoPages/Elements/dropdowns/dropdowns.component';
import { CardsComponent } from '../DemoPages/Elements/cards/cards.component';
import { ListGroupsComponent } from '../DemoPages/Elements/list-groups/list-groups.component';
import { TimelineComponent } from '../DemoPages/Elements/timeline/timeline.component';
import { IconsComponent } from '../DemoPages/Elements/icons/icons.component';
import { AccordionsComponent } from '../DemoPages/Components/accordions/accordions.component';
import { TabsComponent } from '../DemoPages/Components/tabs/tabs.component';
import { CarouselComponent } from '../DemoPages/Components/carousel/carousel.component';
import { ModalsComponent } from '../DemoPages/Components/modals/modals.component';
import { ProgressBarComponent } from '../DemoPages/Components/progress-bar/progress-bar.component';
import { PaginationComponent } from '../DemoPages/Components/pagination/pagination.component';
import { TooltipsPopoversComponent } from '../DemoPages/Components/tooltips-popovers/tooltips-popovers.component';
import { RegularComponent } from '../DemoPages/Tables/regular/regular.component';
import { TablesMainComponent } from '../DemoPages/Tables/tables-main/tables-main.component';
import { ChartBoxes3Component } from '../DemoPages/Widgets/chart-boxes3/chart-boxes3.component';
import { ControlsComponent } from '../DemoPages/Forms/Elements/controls/controls.component';
import { LayoutComponent } from '../DemoPages/Forms/Elements/layout/layout.component';
import { ChartjsComponent } from '../DemoPages/Charts/chartjs/chartjs.component';
import { LineChartComponent } from '../DemoPages/Charts/chartjs/examples/line-chart/line-chart.component';
import { BarChartComponent } from '../DemoPages/Charts/chartjs/examples/bar-chart/bar-chart.component';
import { ScatterChartComponent } from '../DemoPages/Charts/chartjs/examples/scatter-chart/scatter-chart.component';
import { RadarChartComponent } from '../DemoPages/Charts/chartjs/examples/radar-chart/radar-chart.component';
import { PolarAreaChartComponent } from '../DemoPages/Charts/chartjs/examples/polar-area-chart/polar-area-chart.component';
import { BubbleChartComponent } from '../DemoPages/Charts/chartjs/examples/bubble-chart/bubble-chart.component';
import { DynamicChartComponent } from '../DemoPages/Charts/chartjs/examples/dynamic-chart/dynamic-chart.component';
import { DoughnutChartComponent } from '../DemoPages/Charts/chartjs/examples/doughnut-chart/doughnut-chart.component';
import { PieChartComponent } from '../DemoPages/Charts/chartjs/examples/pie-chart/pie-chart.component';
import { FetchDataComponent } from '../DemoPages/fetch-data/fetch-data.component';
import { ExampleFormValidationComponent } from '../DemoPages/example-form-validation/example-form-validation.component';
import { AlertDemoComponent } from '../DemoPages/alert-demo/alert-demo.component';
import { DataTableComponent } from '../DemoPages/data-table/data-table.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { DemoRoutingModule } from '../DemoPages/demo-routing.module';
import { SharedMasterModule } from 'src/app/Shared/Modules/shared-master/shared-master.module';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { ChartsModule } from 'ng2-charts';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AnalyticsComponent } from '../DemoPages/Dashboards/analytics/analytics.component';
import { DemoModule } from '../DemoPages/demo.module';


@NgModule({
  declarations: [
    CommonComponent   
  
  
  
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule,
DemoModule,
    NgSelectModule,
    
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
export class DashboardModule { }
