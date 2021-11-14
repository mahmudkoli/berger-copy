import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { finalize } from 'rxjs/operators';
import { Dropdown } from 'src/app/Shared/Entity/Setup/dropdown';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DynamicDropdownService } from 'src/app/Shared/Entity/Setup/dynamic-dropdown.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { FileUpload } from 'primeng/fileupload';
import { Question, SaveQuestion } from 'src/app/Shared/Entity/ELearning/question';
import { QuestionService } from 'src/app/Shared/Services/ELearning/question.service';
import { ELearningService } from 'src/app/Shared/Services/ELearning/eLearning.service';
import { QuestionOption } from 'src/app/Shared/Entity/ELearning/questionOption';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ModalQuestionOptionFormComponent } from '../modal-question-option-form/modal-question-option-form.component';
import { EnumQuestionTypeLabel } from 'src/app/Shared/Enums/question-type';
import { Status } from 'src/app/Shared/Enums/status';

@Component({
	selector: 'app-question-form',
	templateUrl: './question-form.component.html',
	styleUrls: ['./question-form.component.css']
})
export class QuestionFormComponent implements OnInit, OnDestroy {

	question: Question;
	questionForm: FormGroup;
	eLearningDocuments: MapObject[] = [];
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	questionTypes: MapObject[] = EnumQuestionTypeLabel.enumQuestionTypeLabel;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload; 
	questionOptions: QuestionOption[] = [];
	
	private subscriptions: Subscription[] = [];


	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private promotionalBannerFB: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private dynamicDropdownService: DynamicDropdownService,
		private modalService: NgbModal,
		private eLearningService: ELearningService,
		private questionService: QuestionService) { }

	ngOnInit() {
		this.loadELearningDocuments();

		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			console.log(id);
			if (id) {

				this.alertService.fnLoading(true);
				this.questionService.getQuestion(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.question = res.data as Question;
							if(this.question.questionOptions && this.question.questionOptions.length > 0) {
								this.questionOptions = this.question.questionOptions;
								this.questionOptions.forEach((element, index) => {
									element.statusText = element.status == 0 ? 'Inactive' : 'Active';
									element.isCorrectAnswerText = element.isCorrectAnswer ? 'YES' : 'NO';
									element.editDeleteId = index+1;
								});
							}
							this.initQuestions();
						}
					});
			} else {
				this.question = new Question();
				this.question.clear();
				this.question.status = Status.Active;
				this.initQuestions();
			}
		});
		this.subscriptions.push(routeSubscription);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	loadELearningDocuments() {
		this.alertService.fnLoading(true);
		const categorySubscription = this.eLearningService.getAllForSelect()
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.eLearningDocuments = res.data;
			},
			error => {
				this.throwError(error);
			});
		this.subscriptions.push(categorySubscription);
	}

	initQuestions() {
		this.createForm();
	}

	createForm() {
		this.questionForm = this.promotionalBannerFB.group({
			title: [this.question.title, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
			eLearningDocumentId: [this.question.eLearningDocumentId, [Validators.required]],
			type: [this.question.type, [Validators.required]],
			mark: [this.question.mark, [Validators.required]],
			status: [this.question.status, [Validators.required]]
		});
	}

	get formControls() { return this.questionForm.controls; }

	onSubmit() {

		const controls = this.questionForm.controls;

		if (this.questionForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedQuestions = this.prepareQuestions();
		if (editedQuestions.id) {
			this.updateQuestions(editedQuestions);
		}
		else {
			this.createQuestions(editedQuestions);
		}
	}

	prepareQuestions(): SaveQuestion {
		const controls = this.questionForm.controls;

		const _question = new SaveQuestion();
		_question.clear();
		_question.id = this.question.id;
		_question.title = controls['title'].value;
		_question.eLearningDocumentId = controls['eLearningDocumentId'].value;
		_question.type = controls['type'].value;
		_question.mark = controls['mark'].value;
		_question.status = controls['status'].value;
		if(this.questionOptions && this.questionOptions.length > 0)
			_question.questionOptions = this.questionOptions;
			
		return _question;
	}

	createQuestions(_question: SaveQuestion) {
		this.alertService.fnLoading(true);
		const createSubscription = this.questionService.create(_question)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Question has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateQuestions(_question: SaveQuestion) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.questionService.update(_question)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Question has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	public ptableSettings: IPTableSetting = {
		tableID: "question-options-table",
		tableClass: "table table-border ",
		tableName: 'Question Options',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Title', width: '50%', internalName: 'title', sort: true, type: "" },
			{ headerName: 'Sequence', width: '20%', internalName: 'sequence', sort: true, type: "" },
			{ headerName: 'Is Correct Answer', width: '20%', internalName: 'isCorrectAnswerText', sort: true, type: "" },
			{ headerName: 'Status', width: '10%', internalName: 'statusText', sort: true, type: "" },
		],
		enabledSearch: false,
		enabledSerialNo: true,
		pageSize: 10,
		enabledPagination: false,
		enabledDeleteBtn: true,
		enabledEditBtn: true,
		enabledColumnFilter: false,
		enabledRadioBtn: false,
		enabledRecordCreateBtn: true,
		// newRecordButtonText: 'New Promotional Banner'
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "new-record") {
			this.newQuestionOption();
		}
		else if (event.action == "edit-item") {
			this.editQuestionOption(event.record);
		}
		else if (event.action == "delete-item") {
			this.deleteQuestionOption(event.record);
		}
	}

	newQuestionOption() {
		let qo = new QuestionOption();
		qo.clear();
		qo.status = Status.Active;
		this.openQuestionOptionModal(qo);
	}

	editQuestionOption(obj:QuestionOption) {
		this.openQuestionOptionModal(obj);
	}

	deleteQuestionOption(obj:QuestionOption) {
		let index = this.questionOptions.findIndex(x => x.editDeleteId == obj.editDeleteId);
		this.questionOptions.splice(index, 1);
	}
	
	openQuestionOptionModal(questionOption: QuestionOption) {
		let ngbModalOptions: NgbModalOptions = {
		  backdrop: "static",
		  keyboard: false,
		  size: "lg",
		};
		const modalRef = this.modalService.open(
		  ModalQuestionOptionFormComponent,
		  ngbModalOptions
		);
		modalRef.componentInstance.questionOption = questionOption;
	
		modalRef.result.then(
		  	(result: QuestionOption) => {
				console.log(result);
				if(result.editDeleteId) {
					let obj = this.questionOptions.find(x => x.editDeleteId==result.editDeleteId);
					obj.title = result.title;
					obj.sequence = result.sequence;
					obj.isCorrectAnswer = result.isCorrectAnswer;
					obj.status = result.status;
					obj.statusText = result.status == 0 ? 'Inactive' : 'Active';
					obj.isCorrectAnswerText = result.isCorrectAnswer ? 'YES' : 'NO';
				} else {
					result.editDeleteId = this.questionOptions.length+1;
					result.statusText = result.status == 0 ? 'Inactive' : 'Active';
					result.isCorrectAnswerText = result.isCorrectAnswer ? 'YES' : 'NO';
					this.questionOptions.push(result);
				}
		  	},
		  	(reason) => {
				console.log(reason);
		  	}
		);
	}

	getComponentTitle() {
		let result = 'Create Question';
		if (!this.question || !this.question.id) {
			return result;
		}

		result = `Edit Question - ${this.question.title}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/eLearning/question/list`], { relativeTo: this.activatedRoute });
	}

	stringToInt(value): number {
		return Number.parseInt(value);
	}

	private throwError(errorDetails: any) {
		// this.alertService.fnLoading(false);
		console.log("error", errorDetails);
		let errList = errorDetails.error.errors;
		if (errList.length) {
			console.log("error", errList, errList[0].errorList[0]);
			// this.alertService.tosterDanger(errList[0].errorList[0]);
		} else {
			// this.alertService.tosterDanger(errorDetails.error.message);
		}
	}
}
