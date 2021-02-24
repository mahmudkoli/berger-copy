import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { SchemeService } from '../../../Shared/Services/Scheme/SchemeService';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { SchemeMaster, SchemeMasterQuery } from 'src/app/Shared/Entity/Scheme/SchemeMaster';
import { of, Subscription } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';

@Component({
    selector: 'app-schememaster-list',
    templateUrl: './schememaster-list.component.html',
    styleUrls: ['./schememaster-list.component.css']
})
export class SchememasterListComponent implements OnInit, OnDestroy {

	query: SchemeMasterQuery;
	PAGE_SIZE: number;
	schemeMasters: SchemeMaster[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private schemeMasterService: SchemeService,
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
			this.loadSchemeMastersPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadSchemeMastersPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const schemeMastersSubscription = this.schemeMasterService.getSchemeMasters(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.schemeMasters = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					// this.schemeMasters.forEach(obj => {
					// 	obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					// });
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(schemeMastersSubscription);
	}

	searchConfiguration() {
		this.query = new SchemeMasterQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'title',
			isSortAscending: true,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.schemeMasterService.activeInactive(id).subscribe(res => {
	// 		this.loadSchemeMastersPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editSchemeMaster(id) {
		this.router.navigate(['/scheme/master-add', id]);
	}

	newSchemeMaster() {
		this.router.navigate(['/scheme/master-add']);
	}

	deleteSchemeMaster(id) {
		this.alertService.confirm("Are you sure want to delete this Scheme Master?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.schemeMasterService.deleteSchemeMaster(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Scheme Master has been deleted successfully.");
						this.loadSchemeMastersPage();
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
		tableID: "schemeMasters-table",
		tableClass: "table table-border ",
		tableName: 'Scheme Master List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Scheme Name', width: '50%', internalName: 'schemeName', sort: true, type: "" },
			{ headerName: 'Condition', width: '50%', internalName: 'condition', sort: false, type: "" },
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
		newRecordButtonText: 'New Scheme Master'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newSchemeMaster();
		}
		else if (event.action == "edit-item") {
			this.editSchemeMaster(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteSchemeMaster(event.record.id);
		}
	}
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new SchemeMasterQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadSchemeMastersPage();
	}
}

