import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { Brand, BrandQuery, BrandStatus } from 'src/app/Shared/Entity/Brand/brand';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from './../../../Shared/Modules/search-option/search-option';

@Component({
	selector: 'app-brand-list',
	templateUrl: './brand-list.component.html',
	styleUrls: ['./brand-list.component.css']
})
export class BrandListComponent implements OnInit, OnDestroy {
	searchOptionQuery: SearchOptionQuery;
	query: BrandQuery;
	PAGE_SIZE: number;
	brands: Brand[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination
	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef: [
		  new SearchOptionDef({
			searchOption: EnumSearchOption.MaterialCode,
			isRequired: false,
		  }),
		  new SearchOptionDef({
			searchOption: EnumSearchOption.Brand,
			isRequired: false,
		  }),
		],
	  });
	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private brandService: BrandService,
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

		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();

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

						obj.isEnamelText = obj.isEnamel ? 'Enamel' : 'Non Enamel';
						obj.isEnamelBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isEnamel ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isEnamelBtnIcon = 'fa fa-' + (obj.isEnamel ? 'check' : 'ban');

						obj.isLiquidText = obj.isLiquid ? 'Liquid' : 'Non Liquid';
						obj.isLiquidBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isLiquid ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isLiquidBtnIcon = 'fa fa-' + (obj.isLiquid ? 'check' : 'ban');

						obj.isPowderText = obj.isPowder ? 'Powder' : 'Non Powder';
						obj.isPowderBtnClass = 'btn-transition btn btn-sm btn-outline-' + (obj.isPowder ? 'primary' : 'warning') + ' d-flex align-items-center';
						obj.isPowderBtnIcon = 'fa fa-' + (obj.isPowder ? 'check' : 'ban');

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
			globalSearchValue: '',
			brands:[],
			matrialCodes:[]
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
			{ headerName: 'Material Code', width: '10%', internalName: 'materialCode', sort: true, type: "" },
			{ headerName: 'Material Description', width: '20%', internalName: 'materialDescription', sort: true, type: "" },
			{ headerName: 'Material Group/Brand', width: '10%', internalName: 'materialGroupOrBrand', sort: true, type: "" },
			{ headerName: 'Pack Size', width: '8%', internalName: 'packSize', sort: false, type: "" },
			{ headerName: 'Division', width: '7%', internalName: 'division', sort: false, type: "" },
			{ headerName: 'Is CB', width: '10%', internalName: 'isCBInstalledText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isCBInstalledBtnClass', innerBtnIcon: 'isCBInstalledBtnIcon' },
			{ headerName: 'Is MTS', width: '10%', internalName: 'isMTSText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isMTSBtnClass', innerBtnIcon: 'isMTSBtnIcon' },
			{ headerName: 'Is Premium', width: '10%', internalName: 'isPremiumText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isPremiumBtnClass', innerBtnIcon: 'isPremiumBtnIcon' },
			{ headerName: 'Is Enamel', width: '10%', internalName: 'isEnamelText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isEnamelBtnClass', innerBtnIcon: 'isEnamelBtnIcon' },
			{ headerName: 'Is Liquid', width: '10%', internalName: 'isLiquidText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isLiquidBtnClass', innerBtnIcon: 'isLiquidBtnIcon' },
			{ headerName: 'Is Powder', width: '10%', internalName: 'isPowderText', sort: false, type: "dynamic-button", onClick: 'true', className: 'isPowderBtnClass', innerBtnIcon: 'isPowderBtnIcon' },
			{ headerName: 'Details', width: '5%', internalName: 'viewDetailsText', sort: false, type: "dynamic-button", onClick: 'true', className: 'viewDetailsBtnclass', innerBtnIcon: '' }
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
		// this.query = new BrandQuery({
		// 	page: queryObj.pageNo,
		// 	pageSize: queryObj.pageSize,
		// 	sortBy: queryObj.orderBy,
		// 	isSortAscending: queryObj.isOrderAsc,
		// 	globalSearchValue: queryObj.searchVal
		// });

		this.query.page= queryObj.pageNo;
		this.query.pageSize= queryObj.pageSize;
		this.query.sortBy= queryObj.orderBy;
		this.query.isSortAscending= queryObj.isOrderAsc;
		this.query.globalSearchValue= queryObj.searchVal;


		this.loadBrandsPage();
	}

	public cellClickCallbackFn(event) {

		if (event.cellName == "isCBInstalledText" || event.cellName == "isMTSText" || event.cellName == "isPremiumText"
		|| event.cellName =="isEnamelText"
		|| event.cellName =="isPowderText"
		|| event.cellName =="isLiquidText"
		) {
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
			else if (event.cellName == "isEnamelText") {
				brandStatus.propertyName = 'IsEnamel';
				brandStatus.materialOrBrandCode = event.record.materialGroupOrBrand;
			}
			else if (event.cellName == "isPowderText") {
				brandStatus.propertyName = 'IsPowder';
				brandStatus.materialOrBrandCode = event.record.materialGroupOrBrand;
			}
			else if (event.cellName == "isLiquidText") {
				brandStatus.propertyName = 'IsLiquid';
				brandStatus.materialOrBrandCode = event.record.materialGroupOrBrand;
			}

			this.updateBrandStatus(brandStatus);
		}
		if (event.cellName == 'viewDetailsText') {
			let id = event.record.id;
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
					this.loadBrandsPage();
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(brandsSubscription);
	}


	searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
		this.query.brands = queryObj.brands;
		this.query.matrialCodes = queryObj.materialCodes;

		this.loadBrandsPage();
	  }
}
