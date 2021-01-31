import { Component, OnDestroy, OnInit } from '@angular/core';
import { UserInfo, UserInfoQuery } from '../../../Shared/Entity/Users/userInfo';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/Shared/Services/Users';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { of, Subscription } from 'rxjs';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';

@Component({
    selector: 'app-user-info-list',
    templateUrl: './user-info-list.component.html',
    styleUrls: ['./user-info-list.component.css']
})
export class UserInfoListComponent implements OnInit, OnDestroy {

	query: UserInfoQuery;
	PAGE_SIZE: number;
	userInfos: UserInfo[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(
        private userService: UserService,
        private router: Router,
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private alertService: AlertService,
        private commonService: CommonService) { 
			// this.PAGE_SIZE = 5000;
			// this.ptableSettings.pageSize = 10;
			// this.ptableSettings.enabledServerSitePaggination = false;
			// server side paggination
			this.PAGE_SIZE = commonService.PAGE_SIZE;
			this.ptableSettings.pageSize = this.PAGE_SIZE;
			this.ptableSettings.enabledServerSitePaggination = true;
            this.initPermissionGroup();
    }

	ngOnInit() {
		this.searchConfiguration();
		of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
			this.loadUserInfosPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}
  
    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;

        this.ptableSettings.enabledRecordCreateBtn = true;
        this.ptableSettings.enabledEditBtn = true;
        this.ptableSettings.enabledDeleteBtn = true;
    }

	loadUserInfosPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const userInfosSubscription = this.userService.getAllUserInfo(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.userInfos = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					this.userInfos.forEach(obj => {
						obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(userInfosSubscription);
	}

	searchConfiguration() {
		this.query = new UserInfoQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'fullName',
			isSortAscending: true,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.eLearningDocumentService.activeInactive(id).subscribe(res => {
	// 		this.loadELearningDocumentsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
    // }

	editUserInfo(id) {
		this.router.navigate(['/users-info/edituser-info', id]);
	}

	newUserInfo() {
		this.router.navigate(['/users-info/newuser-info']);
	}

	deleteUserInfo(id) {
		this.alertService.confirm("Are you sure want to delete this User?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.userService.deleteUserInfo(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("User has been deleted successfully.");
						this.loadUserInfosPage();
					},
						(error) => {
							console.log(error);
						});
				this.subscriptions.push(deleteSubscription);
			},
			() => {
			});
	}

	public ptableSettings: IPTableSetting = {
		tableID: "users-table",
		tableClass: "table table-border ",
		tableName: 'User List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Full Name', width: '40%', internalName: 'fullName', sort: true, type: "" },
			{ headerName: 'Department', width: '25%', internalName: 'department', sort: true, type: "" },
			{ headerName: 'Designation', width: '25%', internalName: 'designation', sort: true, type: "" },
			{ headerName: 'Status', width: '10%', internalName: 'statusText', sort: false, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		enabledDeleteBtn: true,
		enabledEditBtn: true,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: true,
		enabledDataLength: true,
		newRecordButtonText: 'New User'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newUserInfo();
		}
		else if (event.action == "edit-item") {
			this.editUserInfo(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteUserInfo(event.record.id);
		}
	}
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new UserInfoQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadUserInfosPage();
	}
}
