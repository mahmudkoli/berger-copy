import { Component, OnInit, ViewChild } from '@angular/core';
//import { IPTableSetting } from '../../../Shared/Modules/p-table';
import { MenuActivity } from '../../../Shared/Entity/Menu/menu-activity';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { MenuActivityService } from '../../../Shared/Services/Menu-Details/menu-activity.service';
import { Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Menu } from '../../../Shared/Entity/Menu/menu.model';
import { MenuService } from '../../../Shared/Services/Menu-Details/menu.service';
import { Status } from 'src/app/Shared/Enums/status';
@Component({
    selector: 'app-menu-activity-list',
    templateUrl: './menu-activity-list.component.html',
    styleUrls: ['./menu-activity-list.component.css']
})
export class MenuActivityListComponent implements OnInit {
    @ViewChild('menuActivityForm', {static: false}) menuActivityForm: any;

    menuActivity: MenuActivity[] = [];
    menuActivityModel: MenuActivity;
    menus: Menu[] = [];
    showForm: boolean = false;
    toasterSuccess: string = `Successfully `;
    public enumStatus = Status;
    public statusValues = [];

    constructor(private menuActivityService: MenuActivityService,
        private alertService: AlertService,
        private menuService: MenuService) { }

    ngOnInit() {
        this.menuActivityModel = new MenuActivity();
        this.statusValues = Object.keys(this.enumStatus).filter(e => !isNaN(Number(e)));
        this.getAllMenuActivity();
        this.getAllChildMenu();
    }

    getAllMenuActivity() {
        this.alertService.fnLoading(true);
        this.menuActivityService.getAllActivity().subscribe(res => {
            this.menuActivity = res.data;
            this.menuActivity.forEach(m => {
                m.statusText = this.enumStatus[m.status];
                m.menuName = m.menu.name;
            });
            console.log("Activities:", this.menuActivity);
            this.alertService.fnLoading(false);
        }, (err) => {
            console.log(err);
            this.alertService.fnLoading(false);
        },
            () => this.alertService.fnLoading(false));
    }

    submit() {
        if (this.menuActivityModel.id > 0) {
            this.update();
        }
        else {
            this.create();
        }
    }

    create() {
        this.menuActivityService.createActivity(this.menuActivityModel).subscribe(
            (res) => {
                console.log(res);
                this.createMenuActivityForm();
                this.alertService.tosterSuccess(this.toasterSuccess + 'Created');
                this.getAllMenuActivity();
            },
            (err) => {
                console.log(err);
                this.alertService.fnLoading(false);
            },
            () => this.alertService.fnLoading(false));
    }

    update() {
        this.menuActivityService.updateActivity(this.menuActivityModel).subscribe(
            (res) => {
                console.log(res);
                this.createMenuActivityForm();
                this.alertService.tosterSuccess(this.toasterSuccess + 'Updated');
                this.getAllMenuActivity();
            },
            (err) => {
                console.log(err);
                this.alertService.fnLoading(false);
            },
            () => this.alertService.fnLoading(false));
    }

    editMenuActivityForm(id: number) {
        this.menuActivityService.getActivityById(id).subscribe((res: any) => {
            this.menuActivityModel = res.data || new MenuActivity();
            console.log(this.menuActivityModel);
            // this.menuActivity.forEach(m => {
            //     m.statusText = this.enumStatus[m.status];
            //     m.menuName = m.menu.name;
            // });
        });
    }

    createMenuActivityForm() {
        // debugger;
        this.menuActivityForm.reset();
        this.menuActivityModel = new MenuActivity();
        console.log(this.menuActivityModel);
    }

    deleteMenuActivity(id: number) {
        this.alertService.confirm("Are you sure you want to delete this item?",
            () => {
                this.menuActivityService.deleteActivity(id).subscribe(
                    (succ: any) => {
                        console.log(succ.data);
                        this.alertService.tosterSuccess(this.toasterSuccess + 'Deleted');
                        this.getAllMenuActivity();
                    });
            },
            () => {
            });
    }

    getAllChildMenu() {
        this.menuService.getAllChild().subscribe(
            (res: any) => {
                console.log(res.data);
                this.menus = res.data;
            },
            (err) => {
                console.log(err);
            });
    }

    public ptableSettings: IPTableSetting = {
        tableID: "Menu-Activity-table",
        tableClass: "table-responsive",
        tableName: 'Activity Menu',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Menu Name', width: '25%', internalName: 'menuName', sort: true, type: "" },
            { headerName: 'Activity Name', width: '25%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Activity Code', width: '25%', internalName: 'activityCode', sort: true, type: "" },
            { headerName: 'Status', width: '25%', internalName: 'statusText', sort: true, type: "" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        enabledEditDeleteBtn: true,
        enabledColumnFilter: true,
        enabledRecordCreateBtn: false,
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.createMenuActivityForm();

        }
        else if (event.action == "edit-item") {
            console.log(event.record);
            this.editMenuActivityForm(event.record.id)

        }
        else if (event.action == "delete-item") {
            console.log(event);
            this.deleteMenuActivity(event.record.id);
        }
    }

}
