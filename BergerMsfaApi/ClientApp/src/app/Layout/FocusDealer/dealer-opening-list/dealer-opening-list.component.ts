import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { DealerOpeningQuery, FocusDealer, FocusDealerQuery } from 'src/app/Shared/Entity/FocusDealer/FocusDealer';
import { EnumEmployeeRole } from 'src/app/Shared/Enums/employee-role';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { DealeropeningService } from '../../../Shared/Services/FocusDealer/dealeropening.service';


@Component({
    selector: 'app-dealer-opening-list',
    templateUrl: './dealer-opening-list.component.html',
    styleUrls: ['./dealer-opening-list.component.css']
})
export class DealerOpeningListComponent implements OnInit {
    query: DealerOpeningQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	isPermitted=false;
	dealerOpening: [];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private dealerOpeningService: DealeropeningService,
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


  
	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.territories = queryObj.territories;
		this.loadDealersOpeningPage();
	}

	loadDealersOpeningPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const focusDealersSubscription = this.dealerOpeningService.GetDealerOpeningList(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.dealerOpening = res.data.items;
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

    viewDealerOpening(id) {
		// this.router.navigate(['/dealer/openingList/', id]);
		const url = this.router.serializeUrl(
			this.router.createUrlTree(['/dealer/openingList/', id])
		);
		window.open(url, '_blank');
	}


	public ptableSettings: IPTableSetting = {
		tableID: "dealerOpening-table",
		tableClass: "table table-border ",
		tableName: 'New Dealer Approval List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Code', width: '10%', internalName: 'code', sort: false, type: "" },
			{ headerName: 'Business Area', width: '10%', internalName: 'businessAreaName', sort: false, type: "" },
			{ headerName: 'Sale Office', width: '25%', internalName: 'saleOfficeName', sort: true, type: "" },
			{ headerName: 'Sale Group', width: '25%', internalName: 'saleGroupName', sort: true, type: "" },
			{ headerName: 'Territory', width: '15%', internalName: 'territoryName', sort: true, type: "" },
			{ headerName: 'Employee Id', width: '15%', internalName: 'employeeId', sort: true, type: "" },
			{ headerName: 'Zone', width: '15%', internalName: 'zoneName', sort: true, type: "" },
			{ headerName: 'Status', width: '15%', internalName: 'dealerOpeningStatusText', sort: true, type: "" },


		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		enabledDeleteBtn: false,
		enabledEditBtn: false,
        enabledDetailsBtn:true,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: false,
		enabledDataLength: true,
		newRecordButtonText: 'New Focus Dealer'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		// if (event.action == "new-record") {
		// 	this.newFocusDealer();
		// }
		// else if (event.action == "edit-item") {
		// 	this.editFocusDealer(event.record.id);
		// }
		// else if (event.action == "delete-item") {
		// 	this.deleteFocusDealer(event.record.id);
		// }
         if (event.action == "details-item") {
			this.viewDealerOpening(event.record.id);
		}
	}

	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query.page = queryObj.pageNo;
		this.query.pageSize = queryObj.pageSize;
		this.query.sortBy = queryObj.orderBy || this.query.sortBy;
		this.query.isSortAscending = queryObj.isOrderAsc != undefined && queryObj.isOrderAsc != null ? queryObj.isOrderAsc : this.query.isSortAscending;
		this.query.globalSearchValue = queryObj.searchVal;
		this.loadDealersOpeningPage();
	}

}
