import { Component, OnInit, OnDestroy } from '@angular/core';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { finalize, take, delay } from 'rxjs/operators';
import { Subscription, of } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ELearningDocument, ELearningDocumentQuery } from 'src/app/Shared/Entity/ELearning/eLearningDocument';
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

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private eLearningDocumentService: ELearningService,
		private modalService: NgbModal,
		private commonService: CommonService) {
		this.PAGE_SIZE = 5000;//commonService.PAGE_SIZE;
	}

	ngOnInit() {
		of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
			this.loadELearningDocumentsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadELearningDocumentsPage() {
		this.searchConfiguration();
		this.alertService.fnLoading(true);
		const eLearningDocumentsSubscription = this.eLearningDocumentService.getELearnings(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.eLearningDocuments = res.data;
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
			// page: 1,
			// pageSize: this.PAGE_SIZE,
			// sortBy: 'name',
			// isSortAscending: true,
			// name: '',
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
		this.alertService.confirm("Are you sure want to delete this ELearning?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.eLearningDocumentService.delete(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("ELearning has been deleted successfully.");
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
			{ headerName: 'Title', width: '40%', internalName: 'title', sort: true, type: "" },
			{ headerName: 'Category', width: '45%', internalName: 'categoryText', sort: false, type: "" },
			{ headerName: 'Status', width: '15%', internalName: 'statusText', sort: true, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		pageSize: 10,
		enabledPagination: true,
		enabledDeleteBtn: true,
		enabledEditBtn: true,
		enabledColumnFilter: true,
		enabledRadioBtn: false,
		enabledRecordCreateBtn: true,
		// newRecordButtonText: 'New Promotional Banner'
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
}
