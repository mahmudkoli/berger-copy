import { Component, OnInit, OnDestroy } from '@angular/core';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { finalize, take, delay, distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { Subscription, of } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Brand, BrandQuery, BrandStatus } from 'src/app/Shared/Entity/Brand/brand';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';

@Component({
	selector: 'app-brand-list',
	templateUrl: './brand-list.component.html',
	styleUrls: ['./brand-list.component.css']
})
export class BrandListComponent implements OnInit, OnDestroy {

	query: BrandQuery;
	PAGE_SIZE: number;
	brands: Brand[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private brandService: BrandService,
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
			this.loadBrandsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadBrandsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const brandsSubscription = this.brandService.getBrands(this.query)
			.pipe(
				finalize(() => { this.alertService.fnLoading(false); }),
				// debounceTime(1000),
				// distinctUntilChanged()
			)
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.brands = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					this.brands.forEach(obj => {
						obj.isCBInstalledText = obj.isCBInstalled ? 'CB' : 'Non CB';
						obj.isCBInstalledBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isCBInstalled ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isCBInstalledBtnIcon = 'fa fa-' + (obj.isCBInstalled ? 'check' : 'ban');

						obj.isMTSText = obj.isMTS ? 'MTS' : 'Non MTS';
						obj.isMTSBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isMTS ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isMTSBtnIcon = 'fa fa-' + (obj.isMTS ? 'check' : 'ban');

						obj.isPremiumText = obj.isPremium ? 'Premium' : 'Non Premium';
						obj.isPremiumBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isPremium ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isPremiumBtnIcon = 'fa fa-' + (obj.isPremium ? 'check' : 'ban');

						obj.viewDetailsText = 'View Log Details';
						obj.viewDetailsBtnclass = 'btn-transition btn btn-sm btn-outline-primary d-flex align-items-center';
						
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(brandsSubscription);
	}

	searchConfiguration() {
		this.query = new BrandQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'matrialCode',
			isSortAscending: true,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.brandService.activeInactive(id).subscribe(res => {
	// 		this.loadBrandsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	public ptableSettings: IPTableSetting = {
		tableID: "brands-table",
		tableClass: "table table-border ",
		tableName: 'Brand List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Material Code', width: '15%', internalName: 'materialCode', sort: true, type: "" },
			{ headerName: 'Material Description', width: '25%', internalName: 'materialDescription', sort: true, type: "" },
			{ headerName: 'Material Group/Brand', width: '10%', internalName: 'materialGroupOrBrand', sort: true, type: "" },
			{ headerName: 'Pack Size', width: '8%', internalName: 'packSize', sort: false, type: "" },
			{ headerName: 'Division', width: '7%', internalName: 'division', sort: false, type: "" },
			{ headerName: 'Is CB', width: '10%', internalName: 'isCBInstalledText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isCBInstalledBtnClass', innerBtnIcon: 'isCBInstalledBtnIcon' },
			{ headerName: 'Is MTS', width: '10%', internalName: 'isMTSText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isMTSBtnClass', innerBtnIcon: 'isMTSBtnIcon' },
			{ headerName: 'Is Premium', width: '10%', internalName: 'isPremiumText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isPremiumBtnClass', innerBtnIcon: 'isPremiumBtnIcon' },
			{ headerName: 'Details', width: '5%', internalName: 'viewDetailsText', sort: false, type: "dynamic-button", onClick: 'true', className: 'viewDetailsBtnclass', innerBtnIcon: '' },
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
		this.query = new BrandQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadBrandsPage();
	}

	public cellClickCallbackFn(event) {
		console.log("cell click: ", event);

		if (event.cellName == "isCBInstalledText" || event.cellName == "isMTSText" || event.cellName == "isPremiumText") {
			let brandStatus = new BrandStatus();
			brandStatus.clear();
	
			if (event.cellName == "isCBInstalledText") {
				brandStatus.propertyName = 'IsCBInstalled';
				brandStatus.materialOrBrandCode = event.record.materialCode;
			}
			else if (event.cellName == "isMTSText") {
				brandStatus.propertyName = 'IsMTS';
				brandStatus.materialOrBrandCode = event.record.materialGroupOrBrand;
			}
			else if (event.cellName == "isPremiumText") {
				brandStatus.propertyName = 'IsPremium';
				brandStatus.materialOrBrandCode = event.record.materialGroupOrBrand;
			}

			this.updateBrandStatus(brandStatus);
		}
		if (event.cellName == 'viewDetailsText') {
			let id = event.record.id;
			console.log("id: "+id);
			this.detailsBrandInfoStatusLogCall(id);
		}
	}
	detailsBrandInfoStatusLogCall(id: any) {
		this.router.navigate([`/brand/log-details/${id}`]);
	}

	updateBrandStatus(brandStatus) {
		this.alertService.fnLoading(true);
		const brandsSubscription = this.brandService.updateBrandStatus(brandStatus)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.loadBrandsPage();
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(brandsSubscription);
	}
}
