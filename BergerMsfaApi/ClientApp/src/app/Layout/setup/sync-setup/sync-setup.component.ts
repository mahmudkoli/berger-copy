import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SyncSetup } from 'src/app/Shared/Entity/Setup/sync-setup';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { SyncSetupService } from 'src/app/Shared/Services/Setup/sync-setup.service';

@Component({
  selector: 'app-sync-setup',
  templateUrl: './sync-setup.component.html',
  styleUrls: ['./sync-setup.component.css'],
})
export class SyncSetupComponent implements OnInit {
  permissionGroup: PermissionGroup = new PermissionGroup();

  public list: SyncSetup[] = [];

  constructor(
    private syncSetupService: SyncSetupService,
    private alertService: AlertService,
    private router: Router
  ) {}

  ngOnInit() {
    this.fnSyncSetupList();
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

  private fnSyncSetupList() {
    this.alertService.fnLoading(true);
    this.syncSetupService.getSyncSeetup().subscribe(
      (res) => {
        this.list = res.data || [];
        this.list.forEach((element) => {
          element.lastSyncTime = this.formatDate(element.lastSyncTime);
        });
      },
      (error) => {
        console.log(error);
      },
      () => this.alertService.fnLoading(false)
    );
  }

  public fnPtableCellClick(event) {
    console.log('cell click: ');
  }

  public fnCustomTrigger(event) {
    if (event.action == 'edit-item') {
      this.edit(event.record.id);
    }
  }

  public ptableSettings = {
    tableID: 'Setup-table',
    tableClass: 'table table-border ',
    tableName: 'Sync Setup',
    tableRowIDInternalName: 'Id',
    tableColDef: [
      {
        headerName: 'Last Sync Time',
        internalName: 'lastSyncTime',
        sort: true,
        type: '',
      },
      {
        headerName: 'Hourly Interval',
        internalName: 'syncHourlyInterval',
        sort: true,
        type: '',
      },
    ],
    enabledSearch: false,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledDeleteBtn: false,
    enabledEditBtn: true,
    enabledColumnFilter: false,
    enabledRecordCreateBtn: false,
  };

  private edit(id: number) {
    console.log('edit product', id);
    this.router.navigate(['/setup/sync-setup-edit/' + id]);
  }
}
