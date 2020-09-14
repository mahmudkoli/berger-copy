import { Component, OnInit } from '@angular/core';

import { User } from '../../../Shared/Entity/Users/user';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { NgbModalOptions, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalExcelImportCmUserComponent } from '../modal-excel-import-cm-user/modal-excel-import-cm-user.component';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.css'],
    providers: [UserService]
})
export class UserListComponent implements OnInit {
    users: User[] = [];
    closeResult: string;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    public tosterMsgDltSuccess: string = "Record Has been deleted successfully.";
    public tosterMsgError: string = "Something went wrong!";
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private userService: UserService,
         private router: Router,
         private modalService: NgbModal,
         private activityPermissionService: ActivityPermissionService,
         private activatedRoute: ActivatedRoute,
          private alertService: AlertService) {
            this.initPermissionGroup();
           }

    ngOnInit() {
        this.getAllUser();
    }

    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    getAllUser() {
        this.alertService.fnLoading(true);
        this.userService.getUserList().subscribe(
            (res: any) => {
                // console.log(res);
                // this.users = res.data;
                let usersData = res.data || [];
                usersData.forEach(obj => {
                    obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                });
                console.log("usersData", usersData);
                this.users = usersData;
            },
            (error) => {
                console.log(error);
            }, () => this.alertService.fnLoading(false));
        
    }
    addNewUser() {
        this.router.navigate(['/users/create-user']);
    }
    deleteUser(id) {
        let userId = parseInt(id);
        //this.userService.deleteUser(userId).subscribe(res => {
        //    console.log(res);
        //});

        this.alertService.confirm("Are you sure?",
            () => {
                this.alertService.fnLoading(true);
                this.userService.deleteUser(userId).subscribe(
                    (succ: any) => {
                        console.log(succ.data);
                        this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
                        this.getAllUser();
                    },
                    (error) => {
                        console.log(error);
                        this.alertService.fnLoading(false);
                        this.alertService.tosterDanger(this.tosterMsgError);
                    },
                    () => this.alertService.fnLoading(false));
            }, () => { });
    }
    editUser(id: number) {
        console.log(id);

        this.router.navigate([`/users/edit-cmuser/${id}`]);

        
    }


    openExcelImportModal() {
        let ngbModalOptions: NgbModalOptions = {
          backdrop: 'static',
          keyboard: false
        };
        const modalRef = this.modalService.open(ModalExcelImportCmUserComponent, ngbModalOptions);
    
        modalRef.result.then((result) => {
          console.log(result);
          this.closeResult = `Closed with: ${result}`;
          this.router.navigate(['/users/users-list/']);
        },
        (reason) => {
            console.log(reason);
        });
    }


    public ptableSettings = {
        tableID: "CMUser-table",
        tableClass: "table-responsive",
        tableName: '',
        tableRowIDInternalName: "Id",
        tableColDef: [

            { headerName: 'Name', width: '15%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Code', width: '10%', internalName: 'code', sort: true, type: "" },
            { headerName: 'Phone Number', width: '15%', internalName: 'phoneNumber', sort: true, type: "" },
            { headerName: 'Email', width: '15%', internalName: 'email', sort: true, type: "" },
            { headerName: 'Family Contact No', width: '15%', internalName: 'familyContactNo', sort: true, type: "" },
            { headerName: 'Address', width: '20%', internalName: 'address', sort: true, type: "" },
            { headerName: 'Status', width: '10%', internalName: 'statusText', sort: true, type: "" },
            

        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        enabledEditBtn: true,
        enabledDeleteBtn: true,
        enabledColumnFilter: true,
        
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.addNewUser();
        }
        else if (event.action == "edit-item") {
            console.log(event.record);
            this.editUser(event.record.id);
        }
        else if (event.action == "delete-item") {
            console.log(event);
            this.deleteUser(event.record.id);
        }
    }

}
