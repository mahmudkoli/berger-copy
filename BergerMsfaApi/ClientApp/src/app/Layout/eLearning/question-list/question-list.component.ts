import { Component, OnInit, OnDestroy } from '@angular/core';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { finalize, take, delay } from 'rxjs/operators';
import { Subscription, of } from 'rxjs';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Question, QuestionQuery } from 'src/app/Shared/Entity/ELearning/question';
import { QuestionService } from 'src/app/Shared/Services/ELearning/question.service';

@Component({
	selector: 'app-question-list',
	templateUrl: './question-list.component.html',
	styleUrls: ['./question-list.component.css']
})
export class QuestionListComponent implements OnInit, OnDestroy {

	query: QuestionQuery;
	PAGE_SIZE: number;
	questions: Question[];

	// Subscriptions
	private subscriptions: Subscription[] = [];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	constructor(
		private router: Router,
		private alertService: AlertService,
		private questionService: QuestionService,
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
			this.loadQuestionsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadQuestionsPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const questionsSubscription = this.questionService.getQuestions(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.questions = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					this.questions.forEach(obj => {
						obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
						obj.eLearningDocumentText = obj.eLearningDocument != null ? obj.eLearningDocument.title : '';
						obj.typeText = obj.type == 1 ? 'Single Choice' : 'Multiple Choice';
					});
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(questionsSubscription);
	}

	searchConfiguration() {
		this.query = new QuestionQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'title',
			isSortAscending: true,
			globalSearchValue: ''
		});
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.questionService.activeInactive(id).subscribe(res => {
	// 		this.loadQuestionsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	editQuestion(id) {
		this.router.navigate(['/eLearning/question/edit', id]);
	}

	newQuestion() {
		this.router.navigate(['/eLearning/question/new']);
	}

	deleteQuestion(id) {
		this.alertService.confirm("Are you sure to delete this Question?",
			() => {
				this.alertService.fnLoading(true);
				const deleteSubscription = this.questionService.delete(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Question has been deleted successfully.");
						this.loadQuestionsPage();
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
		tableID: "questions-table",
		tableClass: "table table-border ",
		tableName: 'Question List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Title', width: '40%', internalName: 'title', sort: true, type: "" },
			{ headerName: 'ELearning Document', width: '25%', internalName: 'eLearningDocumentText', sort: true, type: "" },
			{ headerName: 'Type', width: '15%', internalName: 'typeText', sort: true, type: "" },
			{ headerName: 'Mark', width: '10%', internalName: 'mark', sort: true, type: "" },
			{ headerName: 'Status', width: '10%', internalName: 'statusText', sort: false, type: "" },
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
		newRecordButtonText: 'New Question'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newQuestion();
		}
		else if (event.action == "edit-item") {
			this.editQuestion(event.record.id);
		}
		else if (event.action == "delete-item") {
			this.deleteQuestion(event.record.id);
		}
	}
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new QuestionQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.loadQuestionsPage();
	}
}
