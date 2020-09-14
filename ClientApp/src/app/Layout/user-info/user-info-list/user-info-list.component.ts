import { Component, OnInit } from '@angular/core';
import { UserInfo } from '../../../Shared/Entity/Users/userInfo';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
  selector: 'app-user-info-list',
  templateUrl: './user-info-list.component.html',
    styleUrls: ['./user-info-list.component.css'],
    providers: [UserService]
})
export class UserInfoListComponent implements OnInit {
    usersInfo: UserInfo[] = [];
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    mapObject : MapObject;
    public tosterMsgDltSuccess: string = "Record has been deleted successfully.";
    public tosterMsgError: string = "Something went wrong!";
    permissionGroup: PermissionGroup = new PermissionGroup();


    constructor(private userService: UserService,
         private router: Router,
         private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private alertService: AlertService) { 
            this.initPermissionGroup();
        }

    ngOnInit() {
        this.getAllUserInfo();
  }

  
private initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
}
    getAllUserInfo() {
        this.userService.getAllUserInfo().subscribe(
            (res: any) => {
                this.usersInfo = res.data;

                this.usersInfo.forEach(element => {

                    this.mapObject = this.enumStatusTypes.filter(k => k.id == element.status)[0];

                    if(this.mapObject != null)
                    {
                        element.statusText = this.mapObject.label;
                        
                    }

                });
            });
    }
    private addNewUserInfo() {
        this.router.navigate(['/users-info/insertuser-info']);
    }


    // deleteUserInfo(id: number) {
    //     this.alertService.confirm("Are you sure you want to delete this item?",
    //       () => {
    //         this.alertService.fnLoading(true);
    //         this.userService.deleteUserInfo(id).subscribe(
    //           (succ: any) => {
    //             console.log(succ.data);
    //             this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
    //             this.getAllUserInfo();
    //           },
    //           (error) => {
    //             console.log(error);
    //             this.alertService.fnLoading(false);
    //             this.alertService.tosterDanger(this.tosterMsgError);
    //           },
    //           () => this.alertService.fnLoading(false));
    //       }, () => { });
    //   }






    editUserInfo(id: number) {
        console.log(id);

        this.router.navigate([`/users-info/edituser-info/${id}`]);

    }



    public ptableSettings:IPTableSetting = {
        tableID: "UserInfo-table",
        tableClass: "table-responsive",
        tableName: 'Application User',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Name', width: '20%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Designation', width: '10%', internalName: 'designation', sort: true, type: "" },
            { headerName: 'Phone Number', width: '15%', internalName: 'phoneNumber', sort: true, type: "" },
            { headerName: 'Email', width: '30%', internalName: 'email', sort: true, type: "" },
            { headerName: 'Status', width: '15%', internalName: 'statusText', sort: true, type: "" },
            
           
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        enabledEditBtn: true,
        enabledColumnFilter: true,
        enabledRecordCreateBtn: true,
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.addNewUserInfo();
        }
        else if (event.action == "edit-item") {
            console.log(event.record);
            this.editUserInfo(event.record.id);
        }
    }
}
