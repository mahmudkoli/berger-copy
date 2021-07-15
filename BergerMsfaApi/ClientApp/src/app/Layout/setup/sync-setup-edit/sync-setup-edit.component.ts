import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { SyncSetupService } from 'src/app/Shared/Services/Setup/sync-setup.service';
import { SyncSetup } from './../../../Shared/Entity/Setup/sync-setup';

@Component({
  selector: 'app-sync-setup-edit',
  templateUrl: './sync-setup-edit.component.html',
  styleUrls: ['./sync-setup-edit.component.css'],
})
export class SyncSetupEditComponent implements OnInit {
  model: SyncSetup = new SyncSetup();
  constructor(
    private syncSetupService: SyncSetupService,
    private router: Router,
    private alertService: AlertService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    if (
      Object.keys(this.route.snapshot.params).length !== 0 &&
      this.route.snapshot.params.id !== 'undefined'
    ) {
      let id = this.route.snapshot.params.id;
      this.getSyncSetup(id);
    }
  }

  private getSyncSetup(id) {
    this.syncSetupService.getById(id).subscribe(
      (result: any) => {
        this.editForm(result.data);
      },
      (err: any) => console.log(err)
    );
  }

  public fnRouteList() {
    this.router.navigate(['/setup/sync']);
  }

  private formatDate(date) {
    var d = new Date(date),
      month = '' + (d.getMonth() + 1),
      day = '' + d.getDate(),
      year = d.getFullYear(),
      hour = d.getHours(),
      minute = d.getMinutes();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-') + ' ' + hour + ':' + minute;
  }

  private editForm(model: SyncSetup) {
    this.model = model;
    if (this.model) {
      this.model.lastSyncTime = this.formatDate(this.model.lastSyncTime);
    }
  }

  public fnSave() {
    this.update(this.model);
  }

  private update(model: SyncSetup) {
    this.syncSetupService.update(model).subscribe(
      (res) => {
        this.router.navigate(['/setup/sync']).then(() => {
          this.alertService.tosterSuccess(
            'SyncSetup has been edited successfully.'
          );
        });
      },
      (error) => {
        this.displayError(error);
      },
      () => this.alertService.fnLoading(false)
    );
  }
  private displayError(errorDetails: any) {
    let errList = errorDetails.error.errors;
    if (errList.length) {
      this.alertService.tosterDanger(errList[0].errorList[0]);
    } else {
      this.alertService.tosterDanger(errorDetails.error.msg);
    }
  }
}
