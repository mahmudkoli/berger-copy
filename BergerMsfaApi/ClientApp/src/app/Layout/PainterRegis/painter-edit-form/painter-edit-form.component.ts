import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DynamicDropdownService } from '../../../Shared/Services/Setup/dynamic-dropdown.service';
import { forkJoin, Subscription } from 'rxjs';
import { Status } from '../../../Shared/Enums/status';
import { CommonService } from '../../../Shared/Services/Common/common.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { finalize } from 'rxjs/operators';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { PainterUpdate } from 'src/app/Shared/Entity/Painter/Painter';
import { PainterRegisService } from 'src/app/Shared/Services/Painter-Regis/painterRegister.service';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';

@Component({
    selector: 'app-painter-edit-form',
    templateUrl: './painter-edit-form.component.html',
    styleUrls: ['./painter-edit-form.component.css']
})
export class PainterEditFormComponent implements OnInit {

    public painter: PainterUpdate;
	painterForm: FormGroup;

    depots: any[] = [];
    territories:any[]=[];
    zones: any[] = [];
    painterTypes: any[] = [];
    users: any[] = [];
    dealers: any[] = [];

	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
    id: number;

// 	// Private properties
    private subscriptions: Subscription[] = [];

    constructor(
        private activatedRoute: ActivatedRoute,
		private router: Router,
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private painterService: PainterRegisService,
        private commonSvc: CommonService,
        private dynamicDropdownService: DynamicDropdownService) { }

    ngOnInit() {
        this.populateDropdwonDataList();
		
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
            this.id = id;
			if (id && !isNaN(Number(id))) {

				this.alertService.fnLoading(true);
				this.painterService.GetPainterForEdit(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.painter = res.data as PainterUpdate;
							this.initPainter();
						}
					});
			} else {
				// this.goBack();
			}
		});
		this.subscriptions.push(routeSubscription);
    }

    populateDropdwonDataList() {
        forkJoin([
            this.commonSvc.getDepotList(),
            this.commonSvc.getTerritoryList(),
            this.commonSvc.getZoneList(),
            this.commonSvc.getUserInfoListByCurrentUser(),
            this.commonSvc.getDealerList('',[]),
            this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Painter),
        ]).subscribe(([plants, territories, zones, users, dealers, painterTypes]) => {
            this.depots = plants.data;
            this.territories = territories.data;
            this.zones = zones.data;
            this.users = users.data;
            this.dealers = dealers.data;
            this.painterTypes = painterTypes.data;
        }, (err) => { }, () => { });
    }

    ngOnDestroy() {
        this.subscriptions.forEach(sb => sb.unsubscribe());
    }

    initPainter() {
        this.createForm();
    }

    createForm() {
        this.painterForm = this.formBuilder.group({
            depot: [this.painter.depot, [Validators.required]],
            territory: [this.painter.territory, [Validators.required]],
            zone: [this.painter.zone, [Validators.required]],
            painterCatId: [this.painter.painterCatId],
            painterName: [this.painter.painterName],
            address: [this.painter.address],
            phone: [this.painter.phone],
            noOfPainterAttached: [this.painter.noOfPainterAttached],
            hasDbbl: [this.painter.hasDbbl],
            accDbblNumber: [this.painter.accDbblNumber],
            accDbblHolderName: [this.painter.accDbblHolderName],
            passportNo: [this.painter.passportNo],
            nationalIdNo: [this.painter.nationalIdNo],
            brithCertificateNo: [this.painter.brithCertificateNo],
            isAppInstalled: [this.painter.isAppInstalled],
            remark: [this.painter.remark],
            avgMonthlyVal: [this.painter.avgMonthlyVal],
            loyality: [this.painter.loyality],
            employeeId: [this.painter.employeeId,  [Validators.required]],
            attachedDealerIds: [this.painter.attachedDealerIds],
        });
    } 

    get formControls() { return this.painterForm.controls; }

    onSubmit() {
        const controls = this.painterForm.controls;
        
        if (this.painterForm.invalid) {
            Object.keys(controls).forEach(controlName =>
                controls[controlName].markAsTouched()
            );
            return;
        }

        const editedPainter = this.preparePainter();

        if (editedPainter.id && !isNaN(Number(editedPainter.id))) {
            this.updatePainter(editedPainter);
        }
        else {
            // this.goBack();
        }
    }

    preparePainter(): PainterUpdate {
        const controls = this.painterForm.controls;
        const _painter = new PainterUpdate();
        _painter.clear();
        _painter.id = this.painter.id;
        _painter.depot = controls['depot'].value;
        _painter.territory = controls['territory'].value;
        _painter.zone = controls['zone'].value;
        _painter.painterCatId = controls['painterCatId'].value;
        _painter.painterName = controls['painterName'].value;
        _painter.address = controls['address'].value;
        _painter.phone = controls['phone'].value;
        _painter.noOfPainterAttached = controls['noOfPainterAttached'].value;
        _painter.hasDbbl = controls['hasDbbl'].value;
        _painter.accDbblNumber = controls['accDbblNumber'].value;
        _painter.accDbblHolderName = controls['accDbblHolderName'].value;
        _painter.passportNo = controls['passportNo'].value;
        _painter.nationalIdNo = controls['nationalIdNo'].value;
        _painter.brithCertificateNo = controls['brithCertificateNo'].value;
        _painter.isAppInstalled = controls['isAppInstalled'].value;
        _painter.remark = controls['remark'].value;
        _painter.avgMonthlyVal = controls['avgMonthlyVal'].value;
        _painter.loyality = controls['loyality'].value;
        _painter.employeeId = controls['employeeId'].value;
        _painter.attachedDealerIds = controls['attachedDealerIds'].value;
        _painter.painterImageBase64 = this.painter.painterImageBase64;
        return _painter;
    }

    updatePainter(_painter: PainterUpdate) {
        this.alertService.fnLoading(true);
        const updateSubscription = this.painterService.UpdatePainter(_painter)
            .pipe(finalize(() => this.alertService.fnLoading(false)))
            .subscribe(res => {
                this.alertService.tosterSuccess(`Painter successfully has been updated.`);
                this.goBack();
            },
            error => {
                this.throwError(error);
            });
        this.subscriptions.push(updateSubscription);
    }

    onChangeDepot() {
        this.callTerritories();
        this.callZones();
        const controls = this.painterForm.controls;
        controls['territory'].setValue(null);
        controls['zone'].setValue(null);
    }

    onChangeTerritory() {
        this.callZones();
        const controls = this.painterForm.controls;
        controls['zone'].setValue(null);
    }

    callTerritories () {
        const controls = this.painterForm.controls;
        const depots = controls['depot'].value;
        
            this.commonSvc.getTerritoryListByDepot({'depots':depots}).subscribe(res => {
            this.territories = res.data;
            });
    }
 
    callZones () {
        const controls = this.painterForm.controls;
        const depots = controls['depot'].value;
        const territories = controls['territory'].value;
        
            this.commonSvc.getZoneListByDepotTerritory({'depots':depots,'territories':territories}).subscribe(res => {
            this.zones = res.data;
            });
    }

    changeDealerShow() {
        const controls = this.painterForm.controls;
        const depots = controls['depot'].value;
        const territory = controls['territory'].value;
        const zone = controls['zone'].value;
        let territories = [];
        let zones = [];

        if(territory) territories.push(territory);
        if(zone) zones.push(zone);
    
        this.commonSvc.getDealerByArea({'depots':depots,'territories':territories,'zones':zones}).subscribe(res=>{
            this.dealers = res.data;
        })
    }

    getComponentTitle() {
        let result = 'Create Painter';
        if (!this.painter || !this.painter.id) {
            return result;
        }

        result = `Edit Painter - ${this.painter.painterName}`;
        return result;
    }

    goBack() {
        this.router.navigate([`/painter/register-list`], { relativeTo: this.activatedRoute });
    }

    stringToInt(value): number {
        return Number.parseInt(value);
    }

    private throwError(errorDetails: any) {
        this.alertService.fnLoading(false);
        console.error(`error - ${JSON.stringify(errorDetails)}`);
        let errMsg = (errorDetails.error.errors && errorDetails.error.errors[0] 
                        && errorDetails.error.errors[0].errorList[0])
                    || errorDetails.error.message;
        this.alertService.tosterDanger(errMsg);
    }

    fileChangeEvents(fileInput: any) {
        if (fileInput.target.files && fileInput.target.files[0]) {
            const reader = new FileReader();
            reader.onload = (e: any) => {
                const image = new Image();
                image.src = e.target.result;
                image.onload = (rs) => {
                    const imgBase64Path = e.target.result;
                    this.painter.painterImageBase64 = imgBase64Path;
                };
            };
            reader.readAsDataURL(fileInput.target.files[0]);
        }
    }

    imageDelete($event) {
        this.painterService.DeletePainterImage($event).subscribe((res) => {
            if (res.statusCode == 200) {
                if ($event.type == 'painterImageUrl') {
                    this.painter.painterImageUrl = null;
                }
            } else {
                //this.alertService.tosterWarning('Painter Update failed.');
            }
        });
        console.log($event);
    }
}
