import { Component, OnInit, Inject } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { ActivatedRoute, Router } from '@angular/router';

import { Painter } from '../../../Shared/Entity/Painter/Painter';
import { PainterRegisService } from '../../../Shared/Services/Painter-Regis/painterRegister.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-painter-regis-detail',
    templateUrl: './painter-regis-detail.component.html',
    styleUrls: ['./painter-regis-detail.component.css']
})
export class PainterRegisDetailComponent implements OnInit {
    public baseUrl: string;
    painter:any;
    constructor(

        private alertService: AlertService,
        private route: ActivatedRoute,
        private painterRegisSvc: PainterRegisService,
        private router: Router,
        @Inject('BASE_URL') baseUrl: string) { this.baseUrl = baseUrl;}

    ngOnInit() {
        if (Object.keys(this.route.snapshot.params).length !== 0 && this.route.snapshot.params.id !== 'undefined') {
            console.log("id", this.route.snapshot.params.id);
            let id = this.route.snapshot.params.id;
            this._getRegisterPainterById(id);
        }
    }

    private _getRegisterPainterById(id) {
        this.painterRegisSvc.GetRegisterPainterById(id).subscribe(
            (result: any) => {

                this.painter = result.data;
            },
            (err: any) => console.log(err)
        );
    }
    back() {
        this.router.navigate(['painter/register-list']);
    }

}
