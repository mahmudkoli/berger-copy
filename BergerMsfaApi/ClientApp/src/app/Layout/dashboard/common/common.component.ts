import { Component, OnInit } from '@angular/core';
import { Color } from 'ng2-charts';
import { DashboardService } from 'src/app/Shared/Services/Dashboard/dashboard.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { Dashboard } from 'src/app/Shared/Entity/Common/dashboard';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';

@Component({
  selector: 'app-common',
  templateUrl: './common.component.html',
  styleUrls: ['./common.component.css']
})
export class CommonComponent implements OnInit {

  heading = 'Analytics Dashboard';
  subheading = 'This is an example dashboard created using build-in elements and components.';
  icon = 'pe-7s-plane icon-gradient bg-tempting-azure';

  public tosterMsgError: string = "Something went wrong!";

  dashboardModel : Dashboard;
  test : string = "70%";

  // slideConfig6 = {
  //   className: 'center',
  //   infinite: true,
  //   slidesToShow: 1,
  //   speed: 500,
  //   adaptiveHeight: true,
  //   dots: true,
  // };

  // public datasets = [
  //   {
  //     label: 'Installation product',
  //     data: [65, 59, 80, 81, 46, 55, 38, 59, 80],
  //     datalabels: {
  //       display: false,
  //     },

  //   },
  //   {
  //     label: 'Repair product',
  //     data: [65, 59, 80, 81, 46, 55, 38, 59, 80],
  //     datalabels: {
  //       display: false,
  //     },

  //   },
  //   {
  //     label: 'Removal product',
  //     data: [65, 59, 80, 81, 46, 55, 38, 59, 80],
  //     datalabels: {
  //       display: false,
  //     },

  //   }
  // ];

  // public datasets2 = [
  //   {
  //     label: 'My First dataset',
  //     data: [46, 55, 59, 80, 81, 38, 65, 59, 80],
  //     datalabels: {
  //       display: false,
  //     },

  //   }
  // ];

  // public datasets3 = [
  //   {
  //     label: 'My First dataset',
  //     data: [65, 59, 80, 81, 55, 38, 59, 80, 46],
  //     datalabels: {
  //       display: false,
  //     },

  //   }
  // ];

  // public lineChartColors: Color[] = [
  //   { // dark grey
  //     backgroundColor: 'rgba(48, 177, 255, 0.2)',
  //     borderColor: '#30b1ff',
  //     borderCapStyle: 'round',
  //     borderDash: [],
  //     borderWidth: 4,
  //     borderDashOffset: 0.0,
  //     borderJoinStyle: 'round',
  //     pointBorderColor: '#30b1ff',
  //     pointBackgroundColor: '#ffffff',
  //     pointHoverBorderWidth: 4,
  //     pointRadius: 6,
  //     pointBorderWidth: 5,
  //     pointHoverRadius: 8,
  //     pointHitRadius: 10,
  //     pointHoverBackgroundColor: '#ffffff',
  //     pointHoverBorderColor: '#30b1ff',
  //   },

  //   { // dark grey
  //     backgroundColor: 'rgba(247, 185, 36, 0.2)',
  //     borderColor: '#f7b924',
  //     borderCapStyle: 'round',
  //     borderDash: [],
  //     borderWidth: 4,
  //     borderDashOffset: 0.0,
  //     borderJoinStyle: 'round',
  //     pointBorderColor: '#f7b924',
  //     pointBackgroundColor: '#fff',
  //     pointHoverBorderWidth: 4,
  //     pointRadius: 6,
  //     pointBorderWidth: 5,
  //     pointHoverRadius: 8,
  //     pointHitRadius: 10,
  //     pointHoverBackgroundColor: '#fff',
  //     pointHoverBorderColor: '#f7b924',
  //   },

  //   { // dark grey
  //     backgroundColor: 'rgba(86, 196, 121, 0.2)',
  //     borderColor: '#FF0000',
  //     borderCapStyle: 'round',
  //     borderDash: [],
  //     borderWidth: 4,
  //     borderDashOffset: 0.0,
  //     borderJoinStyle: 'round',
  //     pointBorderColor: '#FF0000',
  //     pointBackgroundColor: '#fff',
  //     pointHoverBorderWidth: 4,
  //     pointRadius: 6,
  //     pointBorderWidth: 5,
  //     pointHoverRadius: 8,
  //     pointHitRadius: 10,
  //     pointHoverBackgroundColor: '#fff',
  //     pointHoverBorderColor: '#FF0000',
  //   },

  // ];

  // public lineChartColors2: Color[] = [
  //   { // dark grey
  //     backgroundColor: 'rgba(48, 177, 255, 0.2)',
  //     borderColor: '#30b1ff',
  //     borderCapStyle: 'round',
  //     borderDash: [],
  //     borderWidth: 4,
  //     borderDashOffset: 0.0,
  //     borderJoinStyle: 'round',
  //     pointBorderColor: '#30b1ff',
  //     pointBackgroundColor: '#ffffff',
  //     pointHoverBorderWidth: 4,
  //     pointRadius: 6,
  //     pointBorderWidth: 5,
  //     pointHoverRadius: 8,
  //     pointHitRadius: 10,
  //     pointHoverBackgroundColor: '#ffffff',
  //     pointHoverBorderColor: '#30b1ff',
  //   },

    
  // ];

  // public lineChartColors3: Color[] = [
  //   { // dark grey
  //     backgroundColor: 'rgba(86, 196, 121, 0.2)',
  //     borderColor: '#56c479',
  //     borderCapStyle: 'round',
  //     borderDash: [],
  //     borderWidth: 4,
  //     borderDashOffset: 0.0,
  //     borderJoinStyle: 'round',
  //     pointBorderColor: '#56c479',
  //     pointBackgroundColor: '#fff',
  //     pointHoverBorderWidth: 4,
  //     pointRadius: 6,
  //     pointBorderWidth: 5,
  //     pointHoverRadius: 8,
  //     pointHitRadius: 10,
  //     pointHoverBackgroundColor: '#fff',
  //     pointHoverBorderColor: '#56c479',
  //   },
  // ];

  // public labels = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August'];

  // public labels : any;

  // public options = {
  //   layout: {
  //     padding: {
  //       left: 0,
  //       right: 8,
  //       top: 0,
  //       bottom: 0
  //     }
  //   },
  //   scales: {
  //     yAxes: [{
  //       ticks: {
  //         display: false,
  //         beginAtZero: true
  //       },
  //       gridLines: {
  //         display: false
  //       }
  //     }],
  //     xAxes: [{
  //       ticks: {
  //         display: false
  //       },
  //       gridLines: {
  //         display: false
  //       }
  //     }]
  //   },
  //   legend: {
  //     display: false
  //   },
  //   responsive: true,
  //   maintainAspectRatio: false
  // };

  constructor(private dashboardService: DashboardService,
    private alertService: AlertService,
    private router: Router) { }

  ngOnInit() {
    this.getDashboardData();
  }

  mapObject : MapObject;
  enumStatusTypes : MapObject[] = StatusTypes.statusType;

  getDashboardData(){
    this.dashboardModel = new Dashboard()
    this.alertService.fnLoading(true);
  //   this.dashboardService.getDashboardData().subscribe(
  //     (res: any) => {
  //       this.dashboardModel = res.data;
  //       console.log(res.data);
  //       this.labels = this.dashboardModel.cmUserNameList;
  //       this.datasets[0].data = this.dashboardModel.posmInstallationProductCountListForCM;
  //       this.datasets[1].data = this.dashboardModel.posmRepairProductCountListForCM;
  //       this.datasets[2].data = this.dashboardModel.posmRemovalProductCountListForCM;
  //     },
  //     (error) => {
  //       console.log(error);
  //       this.alertService.fnLoading(false);
  //       this.alertService.tosterDanger(this.tosterMsgError);
  //     },
  //     () => this.alertService.fnLoading(false));
  }

  // viewCompleteReport(){
  //   this.router.navigate(['/task/complete-report']);
  // }
}
