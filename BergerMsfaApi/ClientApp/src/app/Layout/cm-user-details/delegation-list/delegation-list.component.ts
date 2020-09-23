import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { DelegationService } from 'src/app/Shared/Services/Users/delegation.service';
import { Delegation } from 'src/app/Shared/Entity/Users/delegation';
import { UserService } from 'src/app/Shared/Services/Users';
import { UserInfo } from 'src/app/Shared/Entity/Users';
import { NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
  selector: 'app-delegation-list',
  templateUrl: './delegation-list.component.html',
  styleUrls: ['./delegation-list.component.css']
})
export class DelegationListComponent implements OnInit {

  heading = 'Create A New Delegatin';
  subheading = '';
  icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';
   
   tosterMsgDltSuccess: string = "Record has been deleted successfully.";
   tosterMsgError: string = "Something went wrong!";

   permissionGroup: PermissionGroup = new PermissionGroup();

  constructor(
    private router: Router,
    private delegationService: DelegationService,
    private userService: UserService,
    private alertService: AlertService,
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,
    private calendar: NgbCalendar,
  ) {
    this.initPermissionGroup();
  }


 

  //delegationList: Delegation[] = [];
  delegationNewList: Delegation[] = [];
  delegationPastList: Delegation[] = [];
  userInfoList: UserInfo[] = [];
  toDay = this.calendar.getToday();

  ngOnInit() {
      this.getNewDelegations();
      this.getPastDelegations();
  }


  private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
    this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
}

  createDelegation() {
    this.router.navigate(['/users/delegation-add']);
  } 

  getUserList() {
    this.userService.getAllUserInfo().subscribe((res: any) => {
      console.log("user info list", res.data);
      this.userInfoList = res.data || []; 
    });
    }

  getNewDelegations() {
      this.alertService.fnLoading(true);
      this.delegationService.getDelegationNewList().subscribe(
          (res: any) => {
              let delegationNewData = res.data || [];
              delegationNewData.forEach(obj => {
                  obj.fromDateStr = obj.fromDate.substring(0, 10);
                  obj.toDateStr = obj.toDate.substring(0, 10);
              });
              this.delegationNewList = delegationNewData;
              
          },
          (error) => {
              console.log(error);
              this.alertService.fnLoading(false);
              this.alertService.tosterDanger(this.tosterMsgError);
          },
          () => this.alertService.fnLoading(false));
  }

  getPastDelegations() {
    this.alertService.fnLoading(true);
      this.delegationService.getDelegationPastList().subscribe(
          (res: any) => {
              let delegationPastData = res.data || [];
              delegationPastData.forEach(obj => {
                  obj.fromDateStr = obj.fromDate.substring(0, 10);
                  obj.toDateStr = obj.toDate.substring(0, 10);
              });
              this.delegationPastList = delegationPastData;
              
        },
      (error) => {
        
        this.alertService.fnLoading(false);
        this.alertService.tosterDanger(this.tosterMsgError);
      },
      () => this.alertService.fnLoading(false));
    }



  edit(id: number) {
    this.router.navigate([`/users/delegation-add/${id}`]);
  }

  delete(id: number) {
    this.alertService.confirm("Are you sure you want to delete this item?",
      () => {
        this.alertService.fnLoading(true);
        this.delegationService.deleteDelegation(id).subscribe(
          (succ: any) => {
            this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
            this.getNewDelegations();
            this.getPastDelegations();
          },
          (error) => {
            this.alertService.fnLoading(false);
            this.alertService.tosterDanger(this.tosterMsgError);
          },
          () => this.alertService.fnLoading(false));
      }, () => { });
  }

  public ptableSettings = {
    tableID: "delegation-table",
    tableClass: "table-responsive",
    tableName: 'Delegation List',
    tableRowIDInternalName: "Id",
    tableColDef: [
      { headerName: 'Delegated From User', width: '25%', internalName: 'deligatedFromUserName', sort: true, type: "" },
      { headerName: 'Delegated To User', width: '25%', internalName: 'deligatedToUserName', sort: true, type: "" },
      { headerName: 'From Date', width: '15%', internalName: 'fromDateStr', sort: true, type: "" },
      { headerName: 'To Date', width: '15%', internalName: 'toDateStr', sort: true, type: "" },
      { headerName: 'Name', width: '20%', internalName: 'name', sort: true, type: "" }
    ],
    enabledSearch: true,
    enabledSerialNo: true,
    pageSize: 10,
    enabledPagination: true,
    enabledEditBtn: true,
    enabledDeleteBtn: true,
    enabledColumnFilter: true,
    enabledRecordCreateBtn: true,
  };

  public fnCustomTrigger(event) {
    console.log("custom  click: ", event);

    if (event.action == "new-record") {
      this.createDelegation();
    }
    else if (event.action == "edit-item") {
      this.edit(event.record.id);
    }
    else if (event.action == "delete-item") {
      this.delete(event.record.id);
    }
  }
}
