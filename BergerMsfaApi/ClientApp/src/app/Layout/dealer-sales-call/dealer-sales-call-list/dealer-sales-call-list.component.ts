import { Component, OnInit, OnDestroy } from '@angular/core';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { finalize, take, delay, distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { Subscription, of } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { DealerSalesCall, DealerSalesCallQuery } from 'src/app/Shared/Entity/DealerSalesCall/dealer-sales-call';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';

@Component({
	selector: 'app-dealer-sales-call-list',
	templateUrl: './dealer-sales-call-list.component.html',
	styleUrls: ['./dealer-sales-call-list.component.css']
})
export class DealerSalesCallListComponent implements OnInit, OnDestroy {

	query: DealerSalesCallQuery;
	PAGE_SIZE: number;
	dealerSalesCalls: DealerSalesCall[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private dealerSalesCallService: DealerSalesCallService,
		private modalService: NgbModal,
		private commonService: CommonService) {
			// this.PAGE_SIZE = 5000;
			// this.ptableSettings.pageSize = 10;
			// this.ptableSettings.enabledServerSitePaggination = false;
			// server side paggination
			this.PAGE_SIZE = commonService.PAGE_SIZE;
			this.ptableSettings.pageSize = this.PAGE_SIZE;
			this.ptableSettings.enabledServerSitePaggination = true;
	}

	ngOnInit() {
		this.searchConfiguration();
		of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
			this.loadDealerSalesCallsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadDealerSalesCallsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const dealerSalesCallsSubscription = this.dealerSalesCallService.getDealerSalesCalls(this.query)
			.pipe(
				finalize(() => { this.alertService.fnLoading(false); }),
				// debounceTime(1000),
				// distinctUntilChanged()
			)
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.dealerSalesCalls = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(dealerSalesCallsSubscription);
	}

	searchConfiguration() {
		this.query = new DealerSalesCallQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'userFullName',
			isSortAscending: true,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.dealerSalesCallService.activeInactive(id).subscribe(res => {
	// 		this.loadDealerSalesCallsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	public ptableSettings: IPTableSetting = {
		tableID: "dealerSalesCalls-table",
		tableClass: "table table-border ",
		tableName: 'Dealer Sales Call List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'User Full Name', width: '15%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Dealer Name', width: '15%', internalName: 'dealerName', sort: true, type: "" },
			{ headerName: 'Is Target Promotion Communicated', width: '10%', internalName: 'isTargetPromotionCommunicated', sort: false, type: "" },
			{ headerName: 'Is Target Communicated', width: '10%', internalName: 'isTargetCommunicated', sort: false, type: "" },
			{ headerName: 'Is OS Communicated', width: '10%', internalName: 'isOSCommunicated', sort: false, type: "" },
			{ headerName: 'Is Slippage Communicated', width: '10%', internalName: 'isSlippageCommunicated', sort: false, type: "" },
			{ headerName: 'Is PremiumProduct Communicated', width: '10%', internalName: 'isPremiumProductCommunicated', sort: false, type: "" },
			{ headerName: 'Is CB Installed', width: '10%', internalName: 'isCBInstalled', sort: false, type: "" },
			{ headerName: 'Is CB Productivity Communicated', width: '10%', internalName: 'isCBProductivityCommunicated', sort: false, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		enabledCellClick: true,
		enabledColumnFilter: false,
		// enabledRecordCreateBtn: true,
		enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
	};
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new DealerSalesCallQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadDealerSalesCallsPage();
	}
}
