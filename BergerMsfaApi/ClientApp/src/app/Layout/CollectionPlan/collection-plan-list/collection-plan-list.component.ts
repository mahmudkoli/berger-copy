import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { APIModel } from 'src/app/Shared/Entity';
import { CollectionPlan, CollectionPlanQuery } from 'src/app/Shared/Entity/KPI/CollectionPlan';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { CollectionPlanService } from 'src/app/Shared/Services/KPI/CollectionPlanService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';

@Component({
    selector: 'app-collection-plan-list',
    templateUrl: './collection-plan-list.component.html',
    styleUrls: ['./collection-plan-list.component.css']
})
export class CollectionPlanListComponent implements OnInit, OnDestroy {

	query: CollectionPlanQuery;
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
			this.loadCollectionPlansPage();
		});
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
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.collectionPlanService.activeInactive(id).subscribe(res => {
	// 		this.loadCollectionPlansPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editCollectionPlan(obj:CollectionPlan) {
		var dateNow = new Date();
		var orgDate = new Date(obj.changeableMaxDate);
		if (dateNow.getDay() > orgDate.getDay()){
			this.alertService.tosterWarning(`Sorry! You can't edit after ${obj.changeableMaxDateText}.`);
			return;
		}
		this.router.navigate(['/collection-plan/collection-plan-edit', obj.id]);
	}

	newCollectionPlan() {
		this.router.navigate(['/collection-plan/collection-plan-add']);
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
			{ headerName: 'User Full Name', width: '30%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Business Area', width: '15%', internalName: 'businessArea', sort: false, type: "" },
			{ headerName: 'Territory', width: '15%', internalName: 'territory', sort: false, type: "" },
			{ headerName: 'Year Month', width: '10%', internalName: 'yearMonthText', sort: true, type: "" },
			{ headerName: 'Slippage Amount', width: '15%', internalName: 'slippageAmount', sort: true, type: "" },
			{ headerName: 'Collection Target Amount', width: '15%', internalName: 'collectionTargetAmount', sort: true, type: "" },
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

