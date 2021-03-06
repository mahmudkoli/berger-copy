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
import { QuestionSet, SaveQuestionSet } from 'src/app/Shared/Entity/ELearning/questionSet';
import { QuestionSetService } from 'src/app/Shared/Services/ELearning/questionSet.service';
import { ELearningService } from 'src/app/Shared/Services/ELearning/eLearning.service';
import { IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { QuestionSetCollection } from 'src/app/Shared/Entity/ELearning/questionSetCollection';
import { ModalQuestionSetOptionFormComponent } from '../modal-question-set-option-form/modal-question-set-option-form.component';
import { QuestionService } from 'src/app/Shared/Services/ELearning/question.service';
import { Question } from 'src/app/Shared/Entity/ELearning/question';
import { Status } from 'src/app/Shared/Enums/status';

@Component({
	selector: 'app-question-set-form',
	templateUrl: './question-set-form.component.html',
	styleUrls: ['./question-set-form.component.css']
})
export class QuestionSetFormComponent implements OnInit, OnDestroy {

	questionSet: QuestionSet;
	questionSetForm: FormGroup;
	eLearningDocuments: MapObject[];
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload; 
	questionSetCollections: QuestionSetCollection[] = [];
	depots: any[] = [];
	
	private subscriptions: Subscription[] = [];


	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private promotionalBannerFB: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private dynamicDropdownService: DynamicDropdownService,
		private modalService: NgbModal,
		private eLearningService: ELearningService,
		private questionService: QuestionService,
		private questionSetService: QuestionSetService) { }

	ngOnInit() {
		this.loadELearningDocuments();
		this.loadDepots();

		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			console.log(id);
			if (id) {

				this.alertService.fnLoading(true);
				this.questionSetService.getQuestionSet(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.questionSet = res.data as QuestionSet;
							// if(this.questionSet.questionSetCollections && this.questionSet.questionSetCollections.length > 0) {
							// 	this.questionSetCollections = this.questionSet.questionSetCollections;
							// 	this.questionSetCollections.forEach((element, index) => {
							// 		element.statusText = element.status == 0 ? 'Inactive' : 'Active';
							// 	});
							// }
							this.populateQuestionSetCollection(this.questionSet.eLearningDocumentId, true);
							this.initQuestionSets();
						}
					});
			} else {
				this.questionSet = new QuestionSet();
				this.questionSet.clear();
				this.questionSet.status = Status.Active;
				this.initQuestionSets();
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

	loadDepots() {
		this.alertService.fnLoading(true);
		const categorySubscription = this.commonService.getDepotList()
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.depots = res.data;
			},
			error => {
				this.throwError(error);
			});
		this.subscriptions.push(categorySubscription);
	}

	onChangeELearningDocument() {
		var elId = this.questionSetForm.get('eLearningDocumentId').value;
		if (elId) {
			this.populateQuestionSetCollection(elId, false);
		} else {
			this.questionSetCollections = [];
			this.updateTotalMark();
		}
	}

	populateQuestionSetCollection(elId:any, isEdit: boolean) {
		this.alertService.fnLoading(true);
		const categorySubscription = this.questionService.getQuestionsByELearningDocumentId(elId)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				let qsData = (res.data as Question[]);
				let edId = 100;
				if (qsData && qsData.length > 0) {
					this.questionSetCollections = qsData.map(x => {
						let qsc: QuestionSetCollection = new QuestionSetCollection();
							qsc.clear();
							qsc.questionId = x.id;
							qsc.questionTitle = x.title;
							qsc.mark = x.mark;
							qsc.status = x.status;
							qsc.editDeleteId = edId++;
							qsc.questionTypeText = x.type == 1 ? 'Single Choice' : 'Multiple Choice';
							qsc.statusText = x.status == 0 ? 'Inactive' : 'Active';
						return qsc;
					});

					// for edit
					if(isEdit && this.questionSet.questionSetCollections && this.questionSet.questionSetCollections.length > 0) {
						this.questionSetCollections.forEach(qsc => {
							let existQsc = this.questionSet.questionSetCollections.find(x => x.questionId == qsc.questionId);
							if(existQsc) {
								qsc.id = existQsc.id;
								qsc.isSelected = true;
								qsc.mark = existQsc.mark;
								qsc.status = existQsc.status;
								qsc.statusText = existQsc.status == 0 ? 'Inactive' : 'Active';
							}
						});
					}
				} else {
					this.questionSetCollections = []
				}
				this.updateTotalMark();
			},
			error => {
				this.throwError(error);
				this.questionSetCollections = [];
				this.updateTotalMark();
			});
		this.subscriptions.push(categorySubscription);
	}

	initQuestionSets() {
		this.createForm();
	}

	createForm() {
		this.questionSetForm = this.promotionalBannerFB.group({
			title: [this.questionSet.title, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
			eLearningDocumentId: [this.questionSet.eLearningDocumentId, [Validators.required]],
			level: [this.questionSet.level, [Validators.required]],
			totalMark: [this.questionSet.totalMark, [Validators.required]],
			passMark: [this.questionSet.passMark, [Validators.required]],
			timeOutMinute: [this.questionSet.timeOutMinute, [Validators.required]],
			startDate: [],
			endDate: [],
			depots: [this.questionSet.depots],
			status: [this.questionSet.status, [Validators.required]]
		});

		if(this.questionSet.startDate) {
			const dateStr = new Date(this.questionSet.startDate);
			this.questionSetForm.controls.startDate.setValue({
				year: dateStr.getFullYear(),
				month: dateStr.getMonth()+1,
				day: dateStr.getDate()
			});
		}
		if(this.questionSet.endDate) {
			const dateStr = new Date(this.questionSet.endDate);
			this.questionSetForm.controls.endDate.setValue({
				year: dateStr.getFullYear(),
				month: dateStr.getMonth()+1,
				day: dateStr.getDate()
			});
		}
		this.questionSetForm.controls.startDate.setValidators([Validators.required]);
		this.questionSetForm.controls.startDate.updateValueAndValidity();
		this.questionSetForm.controls.endDate.setValidators([Validators.required]);
		this.questionSetForm.controls.endDate.updateValueAndValidity();
	}

	get formControls() { return this.questionSetForm.controls; }

	onSubmit() {

		const controls = this.questionSetForm.controls;

		if (this.questionSetForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedQuestionSets = this.prepareQuestionSets();
		if (editedQuestionSets.id) {
			this.updateQuestionSets(editedQuestionSets);
		}
		else {
			this.createQuestionSets(editedQuestionSets);
		}
	}

	prepareQuestionSets(): SaveQuestionSet {
		const controls = this.questionSetForm.controls;

		const _questionSet = new SaveQuestionSet();
		_questionSet.clear();
		_questionSet.id = this.questionSet.id;
		_questionSet.title = controls['title'].value;
		_questionSet.eLearningDocumentId = controls['eLearningDocumentId'].value;
		_questionSet.level = controls['level'].value;
		_questionSet.totalMark = controls['totalMark'].value;
		_questionSet.passMark = controls['passMark'].value;
		_questionSet.timeOutMinute = controls['timeOutMinute'].value;
		_questionSet.depots = controls['depots'].value;
		_questionSet.status = controls['status'].value;
		if(this.questionSetCollections && this.questionSetCollections.length > 0)
			_questionSet.questionSetCollections = this.questionSetCollections.filter(x => x.isSelected);
			
		const startDate = controls['startDate'].value;
		if(startDate && startDate.year && startDate.month && startDate.day) {
			_questionSet.startDate = new Date(startDate.year,startDate.month-1,startDate.day);
		}	
		const endDate = controls['endDate'].value;
		if(endDate && endDate.year && endDate.month && endDate.day) {
			_questionSet.endDate = new Date(endDate.year,endDate.month-1,endDate.day);
		}

		return _questionSet;
	}

	createQuestionSets(_questionSet: SaveQuestionSet) {
		this.alertService.fnLoading(true);
		const createSubscription = this.questionSetService.create(_questionSet)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New QuestionSet has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateQuestionSets(_questionSet: SaveQuestionSet) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.questionSetService.update(_questionSet)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`QuestionSet has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	public ptableSettings: IPTableSetting = {
		tableID: "questionSet-options-table",
		tableClass: "table table-border ",
		tableName: 'QuestionSet Collections',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: "Select", width: "10%", internalName: "isSelected", sort: false, type: "checkbox", onClick:'true' },
			{ headerName: 'Question Title', width: '45%', internalName: 'questionTitle', sort: true, type: "" },
			{ headerName: 'Question Type', width: '15%', internalName: 'questionTypeText', sort: true, type: "" },
			{ headerName: 'Mark', width: '15%', internalName: 'mark', sort: true, type: "", onClick:'false' },
			{ headerName: 'Status', width: '15%', internalName: 'statusText', sort: true, type: "" },
		],
		enabledSearch: false,
		enabledSerialNo: true,
		pageSize: 10,
		enabledPagination: true,
		enabledCellClick: true,
		enabledEditBtn: true,
	}; 

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "edit-item") {
			this.editQuestionSetOption(event.record);
		}
	}

	editQuestionSetOption(obj:QuestionSetCollection) {
		this.openQuestionSetOptionModal(obj);
	}
	
	openQuestionSetOptionModal(questionSetOption: QuestionSetCollection) {
		let ngbModalOptions: NgbModalOptions = {
		  backdrop: "static",
		  keyboard: false,
		  size: "lg",
		};
		const modalRef = this.modalService.open(
		  ModalQuestionSetOptionFormComponent,
		  ngbModalOptions
		);
		modalRef.componentInstance.questionSetOption = questionSetOption;
	
		modalRef.result.then(
		  	(result: QuestionSetCollection) => {
				console.log(result);
				let obj = this.questionSetCollections.find(x => x.editDeleteId==result.editDeleteId);
				obj.status = result.status;
				obj.statusText = result.status == 0 ? 'Inactive' : 'Active';
				obj.mark = result.mark;

				this.updateTotalMark();
		  	},
		  	(reason) => {
				console.log(reason);
		  	}
		);
	}
	
	public fnPtableCellClick(event) {
		if (event.cellName == "isSelected") {
		  let obj = this.questionSetCollections.find(x => x.editDeleteId == event.record.editDeleteId);
		  obj.isSelected = !obj.isSelected;
		  this.updateTotalMark();
		}
	}

	updateTotalMark() {
		let selectedQC = this.questionSetCollections.filter(x => x.isSelected);
		let totalMark = 0;
		selectedQC.forEach(x => {
			totalMark += x.mark;
		});
		this.questionSetForm.controls.totalMark.setValue(totalMark);
	}

	getComponentTitle() {
		let result = 'Create QuestionSet';
		if (!this.questionSet || !this.questionSet.id) {
			return result;
		}

		result = `Edit QuestionSet - ${this.questionSet.title}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/eLearning/questionSet/list`], { relativeTo: this.activatedRoute });
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
