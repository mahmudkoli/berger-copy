import { Component, OnInit, OnDestroy, ViewChild, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { QuestionOption } from 'src/app/Shared/Entity/ELearning/questionOption';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { QuestionSetCollection } from 'src/app/Shared/Entity/ELearning/questionSetCollection';

@Component({
	selector: 'app-modal-question-set-option-form',
	templateUrl: './modal-question-set-option-form.component.html',
	styleUrls: ['./modal-question-set-option-form.component.css']
})
export class ModalQuestionSetOptionFormComponent implements OnInit, OnDestroy {

	@Input() questionSetOption: QuestionSetCollection;
	questionSetOptionForm: FormGroup;
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload; 
	
	private subscriptions: Subscription[] = [];


	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private promotionalBannerFB: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private modalService: NgbModal,
		private activeModal: NgbActiveModal) { }

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
		this.questionSetOptionForm = this.promotionalBannerFB.group({
			mark: [this.questionSetOption.mark, [Validators.required]],
			// status: [this.questionSetOption.status, [Validators.required]]
		});
	}

	get formControls() { return this.questionSetOptionForm.controls; }

	onSubmit() {

		const controls = this.questionSetOptionForm.controls;

		if (this.questionSetOptionForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedQuestions = this.prepareQuestionSetOption();
		this.submitQuestionSetOption(editedQuestions);
	}

	prepareQuestionSetOption(): QuestionSetCollection {
		const controls = this.questionSetOptionForm.controls;

		const _questionOption = new QuestionSetCollection();
		_questionOption.clear();
		_questionOption.id = this.questionSetOption.id;
		_questionOption.editDeleteId = this.questionSetOption.editDeleteId;
		_questionOption.mark = controls['mark'].value;
		// _questionOption.status = controls['status'].value;
			
		return _questionOption;
	}

	submitQuestionSetOption(_questionOption: QuestionSetCollection) {
        this.activeModal.close(_questionOption);
	}

	getComponentTitle() {
		let result = 'Create Question Set Option';
		// if (!this.questionSetOption || !this.questionSetOption.id) {
		// 	return result;
		// }

		// result = `Edit Question Set Option - ${this.questionSetOption.questionTitle}`;
		result = `Edit Question Mark - ${this.questionSetOption.questionTitle}`;
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
