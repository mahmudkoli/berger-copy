import { Component, OnInit } from '@angular/core';
import { TintingService } from '../../../Shared/Services/Tinting/TintingService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';

@Component({
  selector: 'app-tintingmachine-list',
  templateUrl: './tintingmachine-list.component.html',
  styleUrls: ['./tintingmachine-list.component.css']
})
export class TintingmachineListComponent implements OnInit {

    tintingMachineList: any[] = [];
    constructor(
        private alertService: AlertService,
        private tintingMachineSvc: TintingService) { }

    ngOnInit() {
        this.getTintingMachineList();
    }
    first = 0;

    rows = 5;
    getTintingMachineList() {
        this.alertService.fnLoading(true);
        this.tintingMachineSvc.getTintingMachineList("T001")
            .subscribe(
                (res) => { this.tintingMachineList = res.data||[] },
                (error) => { this.displayError(error) }
        ).add(() => this.alertService.fnLoading(false));
    }

    private displayError(errorDetails: any) {
        this.alertService.fnLoading(false);
        console.log("error", errorDetails);
        let errList = errorDetails.error.errors;
        if (errList.length) {
            console.log("error", errList, errList[0].errorList[0]);
            this.alertService.tosterDanger(errList[0].errorList[0]);
        } else {
            this.alertService.tosterDanger(errorDetails.error.msg);
        }
    }
}
