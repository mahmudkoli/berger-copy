import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { APIModel } from 'src/app/Shared/Entity';
import { CollectionPlan, CollectionPlanQuery } from 'src/app/Shared/Entity/KPI/CollectionPlan';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { CollectionPlanService } from 'src/app/Shared/Services/KPI/CollectionPlanService';
import { AuthService } from 'src/app/Shared/Services/Users';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';

@Component({
    selector: 'app-collection-plan-list',
    templateUrl: './collection-plan-list.component.html',
    styleUrls: ['./collection-plan-list.component.css']
})
export class CollectionPlanListComponent implements OnInit, OnDestroy {

	query: CollectionPlanQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	collectionPlans: CollectionPlan[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private collectionPlanService: CollectionPlanService,
		private modalService: NgbModal,
		private commonService: CommonService,
		private authService: AuthService) {
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
		// of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
		// 	this.loadCollectionPlansPage();
		// });
		
		this.ptableSettings.enabledEditBtn = this.authService.isAdmin;
		this.ptableSettings.enabledDeleteBtn = this.authService.isAdmin;
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadCollectionPlansPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const collectionPlansSubscription = this.collectionPlanService.getCollectionPlans(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.collectionPlans = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					// this.collectionPlans.forEach(obj => {
					// 	obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					// });
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(collectionPlansSubscription);
	}

	searchConfiguration() {
		this.query = new CollectionPlanQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'userFullName',
			isSortAscending: true,
			globalSearchValue: '',
			businessArea: '',
			territories: []
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole:true}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.businessArea = queryObj.depot;
		this.query.territories = queryObj.territories;
		this.loadCollectionPlansPage();
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.collectionPlanService.activeInactive(id).subscribe(res => {
	// 		this.loadCollectionPlansPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editCollectionPlan(obj:CollectionPlan) {
		// var dateNow = new Date();
		// var orgDate = new Date(obj.changeableMaxDate);
		// if (dateNow.getDay() > orgDate.getDay()){
		// 	this.alertService.tosterWarning(`Sorry! You can't edit after ${obj.changeableMaxDateText}.`);
		// 	return;
		// }
		// this.router.navigate(['/collection-plan/collection-plan-edit', obj.id]);
		const url = this.router.serializeUrl(
			this.router.createUrlTree(['/collection-plan/collection-plan-edit', obj.id])
		);
		window.open(url, '_blank');
	}

	newCollectionPlan() {
		// this.router.navigate(['/collection-plan/collection-plan-add']);
		const url = this.router.serializeUrl(
			this.router.createUrlTree(['/collection-plan/collection-plan-add'])
		);
		window.open(url, '_blank');
	}

	deleteCollectionPlan(id) {
		this.alertService.confirm("Are you sure want to delete this Collection Plan?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.collectionPlanService.deleteCollectionPlan(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Collection Plan has been deleted successfully.");
						this.loadCollectionPlansPage();
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
		tableID: "collection-plans-table",
		tableClass: "table table-border ",
		tableName: 'Collection Plan List',
		tableRowIDInternalName: "id",
		tableColDef: [
			// { headerName: 'User Full Name', width: '30%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Business Area', width: '20%', internalName: 'businessArea', sort: true, type: "" },
			{ headerName: 'Territory', width: '20%', internalName: 'territory', sort: true, type: "" },
			{ headerName: 'Year Month', width: '10%', internalName: 'yearMonthText', sort: true, type: "" },
		  	{ headerName: 'Slippage Amount', width: '25%', internalName: 'slippageAmount', sort: false, type: "text", displayType: 'number-format-color-fraction', showTotal: true },
			{ headerName: 'Collection Target Amount', width: '25%', internalName: 'collectionTargetAmount', sort: false, type: "text",displayType: 'number-format-color-fraction', showTotal: true  },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		enabledTotal:true,
		// pageSize: 10,
		enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: true,
		enabledDataLength: true,
		newRecordButtonText: 'New Collection Plan'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newCollectionPlan();
		}
		else if (event.action == "edit-item") {
			this.editCollectionPlan(event.record);
		}
		else if (event.action == "delete-item") {
			this.deleteCollectionPlan(event.record.id);
		}
	}
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new CollectionPlanQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadCollectionPlansPage();
	}
}

