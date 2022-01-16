import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { FocusDealer, FocusDealerQuery } from 'src/app/Shared/Entity/FocusDealer/FocusDealer';
import { EnumEmployeeRole } from 'src/app/Shared/Enums/employee-role';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { FocusDealerService } from '../../../Shared/Services/FocusDealer/focus-dealer.service';

@Component({
  selector: 'app-focus-dealer-list',
  templateUrl: './focus-dealer-list.component.html',
  styleUrls: ['./focus-dealer-list.component.css'],
})
export class FocusDealerListComponent implements OnInit {

	query: FocusDealerQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	isPermitted=false;
	focusDealers: FocusDealer[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private focusDealerService: FocusDealerService,
		private modalService: NgbModal,
		private commonService: CommonService) {
			// this.PAGE_SIZE = 5000;
			// this.ptableSettings.pageSize = 10;
			// this.ptableSettings.enabledServerSitePaggination = false;
			// server side paggination
			this.PAGE_SIZE = commonService.PAGE_SIZE;
			this.ptableSettings.pageSize = this.PAGE_SIZE;
			this.ptableSettings.enabledServerSitePaggination = true;
		// this.isAuthorize();

	}

	ngOnInit() {
		this.searchConfiguration();
		// this.isAuthorize();
		// of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
		// 	this.loadFocusDealersPage();
		// });
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	isAuthorize() {
		var res=this.commonService.getUserInfoFromLocalStorage()
		this.isPermitted=EnumEmployeeRole.TM_TO==res.employeeRole||EnumEmployeeRole.ZO==res.employeeRole?false:true;
		// console.log(this.isPermitted)
		return this.isPermitted;
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Zone, isRequiredBasedOnEmployeeRole:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		this.query.depot = queryObj.depot;
		this.query.territories = queryObj.territories;
		this.query.zones = queryObj.zones;
		this.loadFocusDealersPage();
	}

	loadFocusDealersPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const focusDealersSubscription = this.focusDealerService.getFocusDealerList(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					this.focusDealers = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					// this.focusDealers.forEach(obj => {
					// 	obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					// });
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(focusDealersSubscription);
	}

	searchConfiguration() {
		this.query = new FocusDealerQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: ''
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.focusDealerService.activeInactive(id).subscribe(res => {
	// 		this.loadFocusDealersPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editFocusDealer(id) {
		this.router.navigate(['/dealer/edit-focus-dealer', id]);
	}

	newFocusDealer() {
		this.router.navigate(['/dealer/new-focus-dealer']);
	}

	deleteFocusDealer(id) {
		this.alertService.confirm("Are you sure want to delete this Focus Dealer?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.focusDealerService.deleteFocusDealer(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						this.alertService.tosterSuccess("Focus Dealer has been deleted successfully.");
						this.loadFocusDealersPage();
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
		tableID: "focusDealers-table",
		tableClass: "table table-border ",
		tableName: 'Focus Dealer List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Territory', width: '10%', internalName: 'territory', sort: false, type: "" },
			{ headerName: 'Zone', width: '10%', internalName: 'zone', sort: false, type: "" },
			{ headerName: 'Dealer', width: '25%', internalName: 'dealerName', sort: true, type: "" },
			{ headerName: 'Employee', width: '25%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Valid From', width: '15%', internalName: 'validFromText', sort: true, type: "" },
			{ headerName: 'Valid To', width: '15%', internalName: 'validToText', sort: true, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		enabledDeleteBtn: true,
		enabledEditBtn: true,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: this.isAuthorize(),
		enabledDataLength: true,
		newRecordButtonText: 'New Focus Dealer'
	};

	public fnCustomTrigger(event) {

		if (event.action == "new-record") {
			this.newFocusDealer();
		}
		else if (event.action == "edit-item") {
			this.editFocusDealer(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteFocusDealer(event.record.id);
		}
	}

	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		this.query.page = queryObj.pageNo;
		this.query.pageSize = queryObj.pageSize;
		this.query.sortBy = queryObj.orderBy || this.query.sortBy;
		this.query.isSortAscending = queryObj.isOrderAsc != undefined && queryObj.isOrderAsc != null ? queryObj.isOrderAsc : this.query.isSortAscending;
		this.query.globalSearchValue = queryObj.searchVal;
		this.loadFocusDealersPage();
	}
}
