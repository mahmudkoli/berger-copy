import { Component, OnInit } from '@angular/core';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-alert-demo',
  templateUrl: './alert-demo.component.html',
  styleUrls: ['./alert-demo.component.sass']
})
export class AlertDemoComponent implements OnInit {

  constructor(private alertService: AlertService) { }

  ngOnInit() {
  }

  showAlert() {
    this.alertService.alert("This is simple alert.");
  }

  confirmAlert() {
    this.alertService.confirm("This is simple alert.", () => {
      console.log("confimr");
    }, () => {

    });
  }

  confirmTerminated(){
    this.alertService.alertAutoTerminated("Testing message");
  }
  loading(status) {
    this.alertService.fnLoading(status);
  }

  fnGenerateTosterSuccess(){
    this.alertService.tosterSuccess("Success toster.......");
  }

  fnGenerateTosterDanger(){
    this.alertService.tosterDanger("Danger toster.......");
  }
  fnGenerateTosterInfo(){
    this.alertService.tosterInfo("Info toster.......");
  }
  fnGenerateTosterWarning(){
    this.alertService.tosterWarning("Warning toster.......");
  }

  fnGenerateTitleTosterSuccess(){
    this.alertService.titleTosterSuccess("Shwoing successfull title toster to the title it will be disable after 5 min.");
  }
  fnGenerateTitleTosterWarning(){
    this.alertService.titleTosterWarning("Shwoing successfull title toster to the title it will be disable after 5 min.");
  }
}
