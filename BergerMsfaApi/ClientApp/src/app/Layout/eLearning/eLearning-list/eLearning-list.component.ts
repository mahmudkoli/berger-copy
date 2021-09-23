import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { of, Subscription } from 'rxjs';
import { delay, finalize, take } from 'rxjs/operators';
import { ELearningDocument, ELearningDocumentQuery } from 'src/app/Shared/Entity/ELearning/eLearningDocument';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { ELearningService } from 'src/app/Shared/Services/ELearning/eLearning.service';

@Component({
	selector: 'app-eLearning-list',
	templateUrl: './eLearning-list.component.html',
	styleUrls: ['./eLearning-list.component.css']
})
export class ELearningListComponent implements OnInit, OnDestroy {

	query: ELearningDocumentQuery;
	PAGE_SIZE: number;
	eLearningDocuments: ELearningDocument[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private eLearningDocumentService: ELearningService,
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
			this.loadELearningDocumentsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadELearningDocumentsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const eLearningDocumentsSubscription = this.eLearningDocumentService.getELearnings(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.eLearningDocuments = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					this.eLearningDocuments.forEach(obj => {
						obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
						obj.categoryText = obj.category != null ? obj.category.dropdownName : '';
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(eLearningDocumentsSubscription);
	}

	searchConfiguration() {
		this.query = new ELearningDocumentQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'uploadDate',
			isSortAscending: false,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.eLearningDocumentService.activeInactive(id).subscribe(res => {
	// 		this.loadELearningDocumentsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editELearningDocument(id) {
		this.router.navigate(['/eLearning/edit', id]);
	}

	newELearningDocument() {
		this.router.navigate(['/eLearning/new']);
	}

	deleteELearningDocument(id) {
		this.alertService.confirm("Are you sure want to delete this E-Learning?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.eLearningDocumentService.delete(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("E-Learning has been deleted successfully.");
						this.loadELearningDocumentsPage();
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
		tableID: "eLearnings-table",
		tableClass: "table table-border ",
		tableName: 'ELearning List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Title', width: '15%', internalName: 'title', sort: true, type: "" },
			{ headerName: 'Category', width: '15%', internalName: 'categoryText', sort: true, type: "" },
			{ headerName: 'Status', width: '10%', internalName: 'statusText', sort: false, type: "" },
			{ headerName: 'Upload Date', width: '10%', internalName: 'uploadDate', sort: true, type: "" },
			{ headerName: 'Attached File Name', width: '25%', internalName: 'attachedFileName', sort: false, type: "" },
			{ headerName: 'Attached Link Address', width: '25%', internalName: 'attachedLinkAddress', sort: false, type: "" },
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
		newRecordButtonText: 'New ELearning'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newELearningDocument();
		}
		else if (event.action == "edit-item") {
			this.editELearningDocument(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteELearningDocument(event.record.id);
		}
	}

	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new ELearningDocumentQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadELearningDocumentsPage();
	}
}
