import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { UniverseReachAnalysis, UniverseReachAnalysisQuery } from 'src/app/Shared/Entity/KPI/UniverseReachAnalysis';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { UniverseReachAnalysisService } from 'src/app/Shared/Services/KPI/UniverseReachAnalysisService';
import { AuthService } from 'src/app/Shared/Services/Users';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';

@Component({
    selector: 'app-universe-reach-analysis-list',
    templateUrl: './universe-reach-analysis-list.component.html',
    styleUrls: ['./universe-reach-analysis-list.component.css']
})
export class UniverseReachAnalysisListComponent implements OnInit, OnDestroy {

	query: UniverseReachAnalysisQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	universeReachAnalysiss: UniverseReachAnalysis[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private universeReachAnalysisService: UniverseReachAnalysisService,
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
		// 	this.loadUniverseReachAnalysissPage();
		// });
		
		this.ptableSettings.enabledEditBtn = this.authService.isAdmin;
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadUniverseReachAnalysissPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const universeReachAnalysissSubscription = this.universeReachAnalysisService.getUniverseReachAnalysiss(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.universeReachAnalysiss = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					// this.universeReachAnalysiss.forEach(obj => {
					// 	obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					// });
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(universeReachAnalysissSubscription);
	}

	searchConfiguration() {
		this.query = new UniverseReachAnalysisQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'fiscalYear',
			isSortAscending: false,
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
		this.loadUniverseReachAnalysissPage();
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.universeReachAnalysisService.activeInactive(id).subscribe(res => {
	// 		this.loadUniverseReachAnalysissPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editUniverseReachAnalysis(id) {
		// this.router.navigate(['/universe-reach-analysis/universe-reach-analysis-edit', id]);
		const url = this.router.serializeUrl(
			this.router.createUrlTree(['/universe-reach-analysis/universe-reach-analysis-edit', id])
		);
		window.open(url, '_blank');
	}

	newUniverseReachAnalysis() {
		// this.router.navigate(['/universe-reach-analysis/universe-reach-analysis-add']);
		const url = this.router.serializeUrl(
			this.router.createUrlTree(['/universe-reach-analysis/universe-reach-analysis-add'])
		);
		window.open(url, '_blank');
	}

	deleteUniverseReachAnalysis(id) {
		this.alertService.confirm("Are you sure want to delete this Universe Reach Plan?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.universeReachAnalysisService.deleteUniverseReachAnalysis(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Universe Reach Plan has been deleted successfully.");
						this.loadUniverseReachAnalysissPage();
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
		tableID: "universe-reach-analysis-table",
		tableClass: "table table-border ",
		tableName: 'Universe Reach Plan List',
		tableRowIDInternalName: "id",
		tableColDef: [
			// { headerName: 'User Full Name', width: '30%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Business Area', width: '20%', internalName: 'businessArea', sort: true, type: "" },
			{ headerName: 'Territory', width: '10%', internalName: 'territory', sort: true, type: "" },
			{ headerName: 'Fiscal Year', width: '10%', internalName: 'fiscalYear', sort: true, type: "" },
			{ headerName: 'Outlet Number', width: '10%', internalName: 'outletNumber', sort: false, type: "" },
			{ headerName: 'Direct Covered', width: '10%', internalName: 'directCovered', sort: false, type: "" },
			{ headerName: 'Indirect Covered', width: '10%', internalName: 'indirectCovered', sort: false, type: "" },
			{ headerName: 'Direct Target', width: '10%', internalName: 'directTarget', sort: false, type: "" },
			{ headerName: 'Indirect Target', width: '10%', internalName: 'indirectTarget', sort: false, type: "" },
			{ headerName: 'Indirect Manual', width: '10%', internalName: 'indirectManual', sort: false, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: true,
		enabledDataLength: true,
		newRecordButtonText: 'New Universe Reach Plan'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newUniverseReachAnalysis();
		}
		else if (event.action == "edit-item") {
			this.editUniverseReachAnalysis(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteUniverseReachAnalysis(event.record.id);
		}
	}
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new UniverseReachAnalysisQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadUniverseReachAnalysissPage();
	}
}

