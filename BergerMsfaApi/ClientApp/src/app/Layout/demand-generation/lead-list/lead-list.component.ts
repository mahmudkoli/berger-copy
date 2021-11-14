import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { LeadGeneration, LeadQuery } from 'src/app/Shared/Entity/DemandGeneration/lead';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { LeadService } from 'src/app/Shared/Services/DemandGeneration/lead.service';

@Component({
	selector: 'app-lead-list',
	templateUrl: './lead-list.component.html',
	styleUrls: ['./lead-list.component.css']
})
export class LeadListComponent implements OnInit, OnDestroy {

	query: LeadQuery;
	PAGE_SIZE: number;
	leads: LeadGeneration[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private leadService: LeadService,
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
			this.loadLeadsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadLeadsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const leadsSubscription = this.leadService.getLeads(this.query)
			.pipe(
				finalize(() => { this.alertService.fnLoading(false); }),
				// debounceTime(1000),
				// distinctUntilChanged()
			)
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.leads = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					this.leads.forEach((x) => {
						x.detailsBtnText = "View Lead";
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(leadsSubscription);
	}

	searchConfiguration() {
		this.query = new LeadQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.leadService.activeInactive(id).subscribe(res => {
	// 		this.loadLeadsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	public ptableSettings: IPTableSetting = {
		tableID: "leads-table",
		tableClass: "table table-border ",
		tableName: 'Lead List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Employee Name', width: '20%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'Depot', width: '10%', internalName: 'depot', sort: false, type: "" },
			{ headerName: 'Territory', width: '10%', internalName: 'territory', sort: false, type: "" },
			{ headerName: 'Zone', width: '10%', internalName: 'zone', sort: false, type: "" },
			{ headerName: 'Code', width: '10%', internalName: 'code', sort: false, type: "" },
			{ headerName: 'Project Name', width: '10%', internalName: 'projectName', sort: false, type: "" },
			{ headerName: 'Project Address', width: '10%', internalName: 'projectAddress', sort: false, type: "" },
			{ headerName: 'Details', width: '10%', internalName: 'detailsBtnText', sort: false, type: "button", onClick: 'true', innerBtnIcon: "" }
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
		this.query = new LeadQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadLeadsPage();
	}

	public cellClickCallbackFn(event: any) {
		console.log(event);
		let id = event.record.id;
		let cellName = event.cellName;

		if (cellName == "detailsBtnText") {
			this.detailsLead(id);
		}
	}

	public detailsLead(id) {
		this.router.navigate([`/lead/details/${id}`]);
	}
}
