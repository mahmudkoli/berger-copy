import { Component, OnInit, OnDestroy, ViewChild, Input } from '@angular/core';
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
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

@Component({
	selector: 'app-modal-question-option-form',
	templateUrl: './modal-question-option-form.component.html',
	styleUrls: ['./modal-question-option-form.component.css']
})
export class ModalQuestionOptionFormComponent implements OnInit, OnDestroy {

	@Input() questionOption: QuestionOption;
	questionOptionForm: FormGroup;
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload; 
	
	private subscriptions: Subscription[] = [];


	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private promotionalBannerFB: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private dynamicDropdownService: DynamicDropdownService,
		private modalService: NgbModal,
		private activeModal: NgbActiveModal,
		private eLearningService: ELearningService,
		private questionService: QuestionService) { }

	ngOnInit() {
		this.initQuestions();
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	initQuestions() {
		this.createForm();
	}

	createForm() {
		this.questionOptionForm = this.promotionalBannerFB.group({
			title: [this.questionOption.title, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
			sequence: [this.questionOption.sequence, [Validators.required]],
			status: [this.questionOption.status, [Validators.required]]
		});
	}

	get formControls() { return this.questionOptionForm.controls; }

	onSubmit() {

		const controls = this.questionOptionForm.controls;

		if (this.questionOptionForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedQuestions = this.prepareQuestionOption();
		this.submitQuestionOption(editedQuestions);
	}

	prepareQuestionOption(): QuestionOption {
		const controls = this.questionOptionForm.controls;

		const _questionOption = new QuestionOption();
		_questionOption.clear();
		_questionOption.id = this.questionOption.id;
		_questionOption.editDeleteId = this.questionOption.editDeleteId;
		_questionOption.title = controls['title'].value;
		_questionOption.sequence = controls['sequence'].value;
		_questionOption.status = controls['status'].value;
			
		return _questionOption;
	}

	submitQuestionOption(_questionOption: QuestionOption) {
        this.activeModal.close(_questionOption);
	}

	getComponentTitle() {
		let result = 'Create Question Option';
		if (!this.questionOption || !this.questionOption.id) {
			return result;
		}

		result = `Edit Question Option - ${this.questionOption.title}`;
		return result;
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
