import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { DealerInfo, DealerInfoQuery, DealerInfoStatus } from 'src/app/Shared/Entity/DealerInfo/DealerInfo';
import { EnumClubSupremeLabel } from 'src/app/Shared/Enums/dealer-info';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { FocusDealerService } from 'src/app/Shared/Services/FocusDealer/focus-dealer.service';
import { ModalExcelImportDealerStatusComponent } from '../modal-excel-import-dealer-status/modal-excel-import-dealer-status.component';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from './../../../Shared/Modules/search-option/search-option';

@Component({
	selector: 'app-dealer-list',
	templateUrl: './dealer-list.component.html',
	styleUrls: ['./dealer-list.component.css']
})
export class DealerListComponent implements OnInit, OnDestroy {

	query: DealerInfoQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	dealers: DealerInfo[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination
	enumClubSupremeLabels: MapObject[] = EnumClubSupremeLabel.enumClubSupremeLabel;

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private modalService: NgbModal,
		private dealerService: FocusDealerService,
		private commonService: CommonService) {
			// this.PAGE_SIZE = 5000;
			// this.ptableSettings.pageSize = 10;
			// this.ptableSettings.enabledServerSitePaggination = false;
			// server side paggination
			this.PAGE_SIZE = this.commonService.PAGE_SIZE;
			this.ptableSettings.pageSize = this.PAGE_SIZE;
			this.ptableSettings.enabledServerSitePaggination = true;
	}

	ngOnInit() {
		this.searchConfiguration();
		// of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
		// 	this.loadDealersPage();
		// });
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef: [
        new SearchOptionDef({ searchOption: EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole: true }),
        new SearchOptionDef({ searchOption: EnumSearchOption.SalesGroup, isRequiredBasedOnEmployeeRole: true }),
        new SearchOptionDef({ searchOption: EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole: true }),
        new SearchOptionDef({ searchOption: EnumSearchOption.Zone, isRequiredBasedOnEmployeeRole: true }),
		]});


  searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
    this.query.depot = queryObj.depot;
    this.query.salesGroups = queryObj.salesGroups;
    this.query.territories = queryObj.territories;
    this.query.zones = queryObj.zones;
    this.loadDealersPage();
  }

	loadDealersPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const dealersSubscription = this.dealerService.getDealerList(this.query)
			.pipe(
				finalize(() => { this.alertService.fnLoading(false); }),
				// debounceTime(1000),
				// distinctUntilChanged()
			)
			.subscribe(
				(res) => {
					this.dealers = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					this.dealers.forEach(obj => {
						obj.isExclusiveText = obj.isExclusive ? 'Exclusive' : 'Not Exclusive';
						obj.isExclusiveBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isExclusive ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isExclusiveBtnIcon = 'fa fa-' + (obj.isExclusive ? 'check' : 'ban');

						obj.isLastYearAppointedText = obj.isLastYearAppointed ? 'Last Year Appointed' : 'Not Appointed';
						obj.isLastYearAppointedBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isLastYearAppointed ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isLastYearAppointedBtnIcon = 'fa fa-' + (obj.isLastYearAppointed ? 'check' : 'ban');

						obj.isAPText = obj.isAP ? 'AP' : 'Not AP';
						obj.isAPBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isAP ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isAPBtnIcon = 'fa fa-' + (obj.isAP ? 'check' : 'ban');

						obj.clubSupremeTypeDropdown = {value: +obj.clubSupremeType||0, data: this.enumClubSupremeLabels}
						obj.clubSupremeTypeDropdownClass = 'ng-select-' + ((+obj.clubSupremeType||0) > 0 ? 'primary' : 'warning');

						// obj.viewDetailsText = 'View Log Details';
						// obj.viewDetailsBtnClass = 'btn-transition btn btn-sm btn-outline-primary d-flex align-items-center';

					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(dealersSubscription);
	}

	searchConfiguration() {
		this.query = new DealerInfoQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'customerName',
			isSortAscending: true,
			globalSearchValue: ''
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.dealerService.activeInactive(id).subscribe(res => {
	// 		this.loadDealersPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	public ptableSettings: IPTableSetting = {
		tableID: "dealers-table",
		tableClass: "table table-border ",
		tableName: 'Dealer List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Territory', width: '7%', internalName: 'territory', sort: false, type: "" },
			{ headerName: 'Zone', width: '6%', internalName: 'zone', sort: false, type: "" },
			{ headerName: 'Customer No', width: '7%', internalName: 'customerNo', sort: true, type: "" },
			{ headerName: 'Customer Name', width: '15%', internalName: 'customerName', sort: true, type: "" },
			{ headerName: 'Contact No', width: '10%', internalName: 'contactNo', sort: false, type: "" },
			{ headerName: 'Address', width: '15%', internalName: 'address', sort: false, type: "" },
			{ headerName: 'Exclusive', width: '10%', internalName: 'isExclusiveText', sort: true, type: "dynamic-button", onClick: 'true', className: 'isExclusiveBtnClass', innerBtnIcon: 'isExclusiveBtnIcon' },
			{ headerName: 'Last Year Appointed', width: '10%', internalName: 'isLastYearAppointedText', sort: true, type: "dynamic-button", onClick: 'true', className: 'isLastYearAppointedBtnClass', innerBtnIcon: 'isLastYearAppointedBtnIcon' },
			{ headerName: 'AP', width: '10%', internalName: 'isAPText', sort: true, type: "dynamic-button", onClick: 'true', className: 'isAPBtnClass', innerBtnIcon: 'isAPBtnIcon' },
			{ headerName: 'Club Supreme', width: '10%', internalName: 'clubSupremeTypeDropdown', sort: true, type: "dynamic-dropdown", onClick: 'true', className: 'clubSupremeTypeDropdownClass', innerBtnIcon: '' },
			// { headerName: 'Details', width: '10%', internalName: 'viewDetailsText', sort: false, type: "dynamic-button", onClick: 'true', className: 'viewDetailsBtnClass', innerBtnIcon: '' }
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		enabledDetailsBtn: true,
		enabledCellClick: true,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: true,
		enabledDataLength: true,
		newRecordButtonText: 'Dealer Status Update',
		newRecordButtonIcon: 'fa fa-file-excel-o'
	};

	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query.page = queryObj.pageNo;
		this.query.pageSize = queryObj.pageSize;
		this.query.sortBy = queryObj.orderBy || this.query.sortBy;
		this.query.isSortAscending = queryObj.isOrderAsc != undefined && queryObj.isOrderAsc != null ? queryObj.isOrderAsc : this.query.isSortAscending;
		this.query.globalSearchValue = queryObj.searchVal;
		this.loadDealersPage();
	}

	public cellClickCallbackFn(event) {

		if (event.cellName == "isExclusiveText" || event.cellName == "isLastYearAppointedText" || event.cellName == "isAPText" || event.cellName == "clubSupremeTypeDropdown") {
			let dealerStatus = new DealerInfoStatus();
			dealerStatus.clear();

			if (event.cellName == "isExclusiveText") {
				dealerStatus.propertyName = 'IsExclusive';
				dealerStatus.dealerId = event.record.id;
			}
			else if (event.cellName == "isLastYearAppointedText") {
				dealerStatus.propertyName = 'IsLastYearAppointed';
				dealerStatus.dealerId = event.record.id;
			}
			else if (event.cellName == "isAPText") {
				dealerStatus.propertyName = 'IsAP';
				dealerStatus.dealerId = event.record.id;
			}
			else if (event.cellName == "clubSupremeTypeDropdown") {
				dealerStatus.propertyName = 'ClubSupremeType';
				dealerStatus.propertyValue = event.record.clubSupremeTypeDropdown.value;
				dealerStatus.dealerId = event.record.id;
			}

			this.updateDealerStatus(dealerStatus);
		}

		// if (event.cellName == 'viewDetailsText') {
		// 	let id = event.record.id;
		// 	this.detailsDealerInfoStatusLog(id);
		// }
	}

	detailsDealerInfoStatusLog(id: any) {
		this.router.navigate([`/dealer/dealer-log-details/${id}`]);
	}

	updateDealerStatus(dealerStatus: DealerInfoStatus) {
		this.alertService.fnLoading(true);
		const dealersSubscription = this.dealerService.updateDealerStatus(dealerStatus)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					this.loadDealersPage();
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(dealersSubscription);
	}

	public fnCustomTrigger(event) {

		if (event.action == "new-record") {
			this.openExcelImportModal();
		} else if (event.action == "details-item") {
			let id = event.record.id;
			this.detailsDealerInfoStatusLog(id);
		}
	}

	openExcelImportModal() {
		let ngbModalOptions: NgbModalOptions = {
			backdrop: 'static',
			keyboard: false
		};
		const modalRef = this.modalService.open(ModalExcelImportDealerStatusComponent, ngbModalOptions);
	
		modalRef.result.then((result) => {
			console.log(result);
		  // this.router.navigate(['/dealer/dealer-list']);
			if (this.query.depot)
				this.loadDealersPage();
		},
		(reason) => {
			console.log(reason);
		});
	}
}
