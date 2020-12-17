import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { finalize } from 'rxjs/operators';
import { ELearningDocument, SaveELearningDocument } from 'src/app/Shared/Entity/ELearning/eLearningDocument';
import { ELearningService } from 'src/app/Shared/Services/ELearning/eLearning.service';
import { Dropdown } from 'src/app/Shared/Entity/Setup/dropdown';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { DynamicDropdownService } from 'src/app/Shared/Entity/Setup/dynamic-dropdown.service';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { FileUpload } from 'primeng/fileupload';

@Component({
	selector: 'app-eLearning-form-new',
	templateUrl: './eLearning-form-new.component.html',
	styleUrls: ['./eLearning-form-new.component.css']
})
export class ELearningFormComponent implements OnInit, OnDestroy {

	eLearningDocument: ELearningDocument;
	eLearningDocumentForm: FormGroup;
	categories: Dropdown[] = [];
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	@ViewChild('fileInput', {static:false}) fileInput: FileUpload; 
	attachmentLinkUrls: string[] = [];
	attachmentFiles: File[] = [];
	
	private subscriptions: Subscription[] = [];


	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private promotionalBannerFB: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private dynamicDropdownService: DynamicDropdownService,
		private eLearningDocumentService: ELearningService) { }

	ngOnInit() {
		this.loadCategories();

		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			console.log(id);
			if (id) {

				this.alertService.fnLoading(true);
				this.eLearningDocumentService.getELearning(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.eLearningDocument = res.data as ELearningDocument;
							this.initELearningDocuments();
						}
					});
			} else {
				this.eLearningDocument = new ELearningDocument();
				this.eLearningDocument.clear();
				this.initELearningDocuments();
			}
		});
		this.subscriptions.push(routeSubscription);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	loadCategories() {
		this.alertService.fnLoading(true);
		const categoryCode = 'C01';
		const categorySubscription = this.dynamicDropdownService.GetDropdownByTypeCd(categoryCode)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.categories = res.data;
			},
			error => {
				this.throwError(error);
			});
		this.subscriptions.push(categorySubscription);
	}

	initELearningDocuments() {
		this.createForm();
	}

	createForm() {
		this.eLearningDocumentForm = this.promotionalBannerFB.group({
			title: [this.eLearningDocument.title, [Validators.required, Validators.pattern(/^(?!\s+$).+/)]],
			categoryId: [this.eLearningDocument.categoryId, [Validators.required]],
			status: [this.eLearningDocument.status, [Validators.required]]
		});
	}

	get formControls() { return this.eLearningDocumentForm.controls; }

	onSubmit() {

		const controls = this.eLearningDocumentForm.controls;

		if (this.eLearningDocumentForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedELearningDocuments = this.prepareELearningDocuments();
		if (editedELearningDocuments.id) {
			this.updateELearningDocuments(editedELearningDocuments);
		}
		else {
			this.createELearningDocuments(editedELearningDocuments);
		}
	}

	prepareELearningDocuments(): SaveELearningDocument {
		const controls = this.eLearningDocumentForm.controls;

		const _eLearningDocument = new SaveELearningDocument();
		_eLearningDocument.clear();
		_eLearningDocument.id = this.eLearningDocument.id;
		_eLearningDocument.title = controls['title'].value;
		_eLearningDocument.categoryId = controls['categoryId'].value;
		_eLearningDocument.status = controls['status'].value;
		// if(this.fileInput.files && this.fileInput.files.length > 0)
		// 	_eLearningDocument.eLearningAttachmentFiles = this.fileInput.files;
		if(this.attachmentFiles && this.attachmentFiles.length > 0)
			_eLearningDocument.eLearningAttachmentFiles = this.attachmentFiles;
		if(this.attachmentLinkUrls && this.attachmentLinkUrls.length > 0)
			_eLearningDocument.eLearningAttachmentUrls = this.attachmentLinkUrls;
			
		return _eLearningDocument;
	}

	createELearningDocuments(_eLearningDocument: SaveELearningDocument) {
		this.alertService.fnLoading(true);
		const createSubscription = this.eLearningDocumentService.create(_eLearningDocument)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New ELearning has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateELearningDocuments(_eLearningDocument: SaveELearningDocument) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.eLearningDocumentService.update(_eLearningDocument)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`ELearning has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	onChangeInputFile(event: any) {
		if (event.target.files && event.target.files.length > 0) {
			const files = event.target.files as File[];
			this.attachmentFiles = [...this.attachmentFiles,...files];
		}
	}

	removeAttachmentFiles(index) {
		this.attachmentFiles.splice(index, 1);
	}

	addAttachmentLinkUrls(value) {
		if(value)
			this.attachmentLinkUrls.push(value);
	}

	removeAttachmentLinkUrls(index) {
		this.attachmentLinkUrls.splice(index, 1);
	}

	getComponentTitle() {
		let result = 'Create ELearning';
		if (!this.eLearningDocument || !this.eLearningDocument.id) {
			return result;
		}

		result = `Edit ELearning - ${this.eLearningDocument.title}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/eLearning`], { relativeTo: this.activatedRoute });
	}

	stringToInt(value): number {
		return Number.parseInt(value);
	}

	// 	onAlertClose($event) {
	// 		this.resetErrors();
	// 	}

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

    // onChangeFile(file: File) {
    //     console.log("image file", file);
    //     this.imageFile = file;
    //     if(this.imageFile == null)
    //     {
    //         this.eLearningDocument.imageUrl = '';
    //     }
    // }
}
