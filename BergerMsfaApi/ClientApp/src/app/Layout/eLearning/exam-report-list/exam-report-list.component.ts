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
import { UserQuestionAnswer, UserQuestionAnswerQuery } from 'src/app/Shared/Entity/ELearning/userQuestionAnswer';
import { ExamService } from 'src/app/Shared/Services/ELearning/exam.service';
import { QueryObject } from 'src/app/Shared/Entity/Common/query-object';

@Component({
	selector: 'app-exam-report-list',
	templateUrl: './exam-report-list.component.html',
	styleUrls: ['./exam-report-list.component.css']
})
export class ExamReportListComponent implements OnInit, OnDestroy {

	query: UserQuestionAnswerQuery;
	PAGE_SIZE: number;
	examReports: UserQuestionAnswer[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private examService: ExamService,
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
			this.loadUserQuestionAnswersPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	getDownloadDataApiUrl = (query) => this.examService.downloadAllExamReport(query);

	loadUserQuestionAnswersPage() {
		// this.searchConfiguration();
		this.alertService.fnLoading(true);
		const questionsSubscription = this.examService.getAllExamReport(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.examReports = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					// this.examReports.forEach(obj => {
					// 	obj.statusText = obj.status == 0 ? 'Inactive' : 'Active';
					// 	obj.passedText = obj.passed ? 'YES' : 'NO';
					// 	obj.questionSetTitle = obj.questionSet != null ? obj.questionSet.title : '';
					// 	obj.questionSetLevel = obj.questionSet != null ? `${obj.questionSet.level}` : '';
					// 	obj.questionSetTotalMark = obj.questionSet != null ? `${obj.questionSet.totalMark}` : '';
					// 	obj.questionSetPassMark = obj.questionSet != null ? `${obj.questionSet.passMark}` : '';
					// });
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(questionsSubscription);
	}

	searchConfiguration() {
		this.query = new UserQuestionAnswerQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'examDate',
			isSortAscending: false,
			globalSearchValue: ''
		});
		this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(this.query);
	}

	// toggleActiveInactive(id) {
	// 	const actInSubscription = this.questionService.activeInactive(id).subscribe(res => {
	// 		this.loadQuestionsPage();
	// 	});
	// 	this.subscriptions.push(actInSubscription);
	// }

	public ptableSettings: IPTableSetting = {
		tableID: "exam-report-table",
		tableClass: "table table-border ",
		tableName: 'Exam Report List',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'User Full Name', width: '15%', internalName: 'userFullName', sort: true, type: "" },
			{ headerName: 'EmployeeId', width: '10%', internalName: 'employeeId', sort: false, type: "" },
			{ headerName: 'Question Set Title', width: '20%', internalName: 'questionSetTitle', sort: true, type: "" },
			{ headerName: 'Set Level', width: '10%', internalName: 'questionSetLevel', sort: true, type: "" },
			{ headerName: 'Exam Date', width: '10%', internalName: 'examDate', sort: true, type: "" },
			{ headerName: 'Total Mark', width: '10%', internalName: 'totalMark', sort: true, type: "" },
			{ headerName: 'Pass Mark', width: '10%', internalName: 'passMark', sort: false, type: "" },
			{ headerName: 'User Mark', width: '10%', internalName: 'userMark', sort: true, type: "" },
			{ headerName: 'Pass Status', width: '10%', internalName: 'passStatus', sort: true, type: "" },
			// { headerName: 'Status', width: '10%', internalName: 'statusText', sort: false, type: "" },
		],
		enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		enabledDeleteBtn: false,
		enabledEditBtn: false,
		enabledColumnFilter: false,
		enabledRecordCreateBtn: false,
		enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
		enabledExcelDownload: true,
		downloadDataApiUrl: `${this.getDownloadDataApiUrl(
								new QueryObject({
									page: 1,
									pageSize: 2147483647, // Int32 max value
									sortBy: 'examDate',
									isSortAscending: false,
									globalSearchValue: ''
								}))}`,
	};
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query = new UserQuestionAnswerQuery({
			page: queryObj.pageNo,
			pageSize: queryObj.pageSize,
			sortBy: queryObj.orderBy,
			isSortAscending: queryObj.isOrderAsc,
			globalSearchValue: queryObj.searchVal
		});
		this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(this.query);
		this.loadUserQuestionAnswersPage();
	}
}
