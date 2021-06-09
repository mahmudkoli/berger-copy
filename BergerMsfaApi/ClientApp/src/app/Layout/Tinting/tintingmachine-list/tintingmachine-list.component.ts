import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { TintingService } from '../../../Shared/Services/Tinting/TintingService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { of, Subscription } from 'rxjs';
import { TintingMachine, TintingMachineQuery } from 'src/app/Shared/Entity/Tinting/TintingMachine';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';

@Component({
    selector: 'app-tintingmachine-list',
    templateUrl: './tintingmachine-list.component.html',
    styleUrls: ['./tintingmachine-list.component.css']
})
export class TintingmachineListComponent implements OnInit, OnDestroy {

	query: TintingMachineQuery;
	PAGE_SIZE: number;
	tintingMachines: TintingMachine[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private tintingMachineService: TintingService,
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
			this.loadTintingMachinesPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadTintingMachinesPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const tintingMachinesSubscription = this.tintingMachineService.getTintingMachines(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.tintingMachines = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					// this.tintingMachines.forEach(obj => {
					// 	obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					// });
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(tintingMachinesSubscription);
	}

	searchConfiguration() {
		this.query = new TintingMachineQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'title',
			isSortAscending: true,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.tintingMachineService.activeInactive(id).subscribe(res => {
	// 		this.loadTintingMachinesPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	public ptableSettings: IPTableSetting = {
		tableID: "tintingMachines-table",
		tableClass: "table table-border ",
		tableName: 'Tinting Machine List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Employee Name', width: '20%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Depot', width: '20%', internalName: 'depot', sort: true, type: "" },
			{ headerName: 'Territory', width: '10%', internalName: 'territory', sort: true, type: "" },
			{ headerName: 'Company Name', width: '10%', internalName: 'companyName', sort: true, type: "" },
			{ headerName: 'No Of Active Machine', width: '10%', internalName: 'noOfActiveMachine', sort: true, type: "" },
			{ headerName: 'No Of Inactive Machine', width: '10%', internalName: 'noOfInactiveMachine', sort: true, type: "" },
			{ headerName: 'No', width: '10%', internalName: 'no', sort: true, type: "" },
			{ headerName: 'Contribution', width: '10%', internalName: 'contribution', sort: true, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		enabledColumnFilter: false,
		// enabledRecordCreateBtn: true,
		enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
	};
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new TintingMachineQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadTintingMachinesPage();
	}
}
