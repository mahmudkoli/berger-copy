import { Component, OnInit, OnDestroy } from '@angular/core';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { finalize, take, delay } from 'rxjs/operators';
import { Subscription, of } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { QuestionSet, QuestionSetQuery } from 'src/app/Shared/Entity/ELearning/questionSet';
import { QuestionSetService } from 'src/app/Shared/Services/ELearning/questionSet.service';

@Component({
	selector: 'app-question-set-list',
	templateUrl: './question-set-list.component.html',
	styleUrls: ['./question-set-list.component.css']
})
export class QuestionSetListComponent implements OnInit, OnDestroy {

	query: QuestionSetQuery;
	PAGE_SIZE: number;
	questionSets: QuestionSet[];

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private questionSetService: QuestionSetService,
		private modalService: NgbModal,
		private commonService: CommonService) {
		this.PAGE_SIZE = 5000;//commonService.PAGE_SIZE;
	}

	ngOnInit() {
		of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
			this.loadQuestionSetsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadQuestionSetsPage() {
		this.searchConfiguration();
		this.alertService.fnLoading(true);
		const questionSetsSubscription = this.questionSetService.getQuestionSets(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.questionSets = res.data;
					this.questionSets.forEach(obj => {
						obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(questionSetsSubscription);
	}

	searchConfiguration() {
		this.query = new QuestionSetQuery({
			// page: 1,
			// pageSize: this.PAGE_SIZE,
			// sortBy: 'name',
			// isSortAscending: true,
			// name: '',
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.questionSetService.activeInactive(id).subscribe(res => {
	// 		this.loadQuestionSetsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editQuestionSet(id) {
		this.router.navigate(['/eLearning/questionSet/edit', id]);
	}

	newQuestionSet() {
		this.router.navigate(['/eLearning/questionSet/new']);
	}

	deleteQuestionSet(id) {
		this.alertService.confirm("Are you sure want to delete this QuestionSet?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.questionSetService.delete(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("QuestionSet has been deleted successfully.");
						this.loadQuestionSetsPage();
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
		tableID: "questionSets-table",
		tableClass: "table table-border ",
		tableName: 'QuestionSet List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Title', width: '40%', internalName: 'title', sort: true, type: "" },
			{ headerName: 'Level', width: '15%', internalName: 'level', sort: true, type: "" },
			{ headerName: 'Total Mark', width: '15%', internalName: 'totalMark', sort: false, type: "" },
			{ headerName: 'Pass Mark', width: '15%', internalName: 'passMark', sort: false, type: "" },
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
			this.newQuestionSet();
		}
		else if (event.action == "edit-item") {
			this.editQuestionSet(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteQuestionSet(event.record.id);
		}
	}
}
