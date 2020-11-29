import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from "rxjs";
import { catchError, map, finalize } from "rxjs/operators"
import { AlertService } from './Shared/Modules/alert/alert.service';
import { ActivityPermissionService } from './Shared/Services/Activity-Permission/activity-permission.service';
import { ActivatedRoute } from '@angular/router';

import { NgbPaginationNext } from '@ng-bootstrap/ng-bootstrap';

@Injectable({ providedIn: 'root' })
export class AppInterceptorService implements HttpInterceptor {
    /**
     *
     */
    constructor(private alertService: AlertService,
        private activityPermissionService: ActivityPermissionService, private activatedRoute: ActivatedRoute,
        ) { }
    handleError = (error: HttpErrorResponse, request?, next?) => {
        console.log("api error:", error);
        setTimeout(() => {
            //this.alertService.fnLoading(false);   
            let statusCode = error.status;
            let errorMsg = error.error.msg || "";
            if (statusCode == 0) {
                errorMsg = "You may have internet connection problem. Check your network and try again";
            } else if (statusCode == 404) {
                errorMsg = "You may have application issues. Please contact with system admin.";
            }
            else if (error.url.indexOf('graph.microsoft.com') > -1 && error.status == 401) {
                
            }
            else if (statusCode == 401) {

                errorMsg = "You are not authorized. Please contact with system admin.";
            }

            //showing message            
            if (errorMsg != "") {
                this.alertService.titleTosterDanger(errorMsg);
            }
        }, 1000);

        return throwError(error)
    }
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.alertService.fnLoading(true);
        console.log('processing request', request);
        console.log('url', request.url);
        debugger;
        let token = localStorage.getItem('bergermsfa');

        // local storege for ad
        //if (request.url.indexOf('graph.microsoft.com') > -1) {
        //    console.log('add request');

        //    token = localStorage.getItem('adtoken');
        //} else {
        //    token = localStorage.getItem('bergermsfa');
        //}

        if (request.method === 'POST' || request.method === 'PUT') {
            this.shiftDates(request.body);
        }



        if (request.method == "DELETE") {
            let activityPermission = this.activityPermissionService.getActivityPermissionFromSession();
            let hasPermission = false;
            const reqUrl = window.location.href; //request.url
            console.log(activityPermission);
            if (activityPermission && activityPermission.length > 0) {
                const filterActPer = activityPermission.filter(x => reqUrl.indexOf(x.url) > -1);
                console.log(filterActPer);
                if (filterActPer && filterActPer.length > 0) {
                    if (request.method == "DELETE" && filterActPer[0].canDelete) {
                        hasPermission = true;
                    }
                }
            }

            if (!hasPermission) {
                let errorMsg = "You don't have permission to delete.";
                this.alertService.titleTosterDanger(errorMsg);
                setTimeout(() => {
                    this.alertService.fnLoading(false);
                }, 1000);
                return; //next.handle(request);
            }
        }

        // console.log("token: ", token);
        const headers = new HttpHeaders({
            Authorization: 'Bearer ' + token
        });

        const haderClone = request.clone({
            headers
        });

        return next
            .handle(haderClone)
            .pipe(
                catchError(error => {
                    return this.handleError(error, request, next);
                }),
                finalize(() => {
                    setTimeout(() => {
                        this.alertService.fnLoading(false);
                    }, 1000);
                })
            )
    }

    shiftDates(body) {
        if (body === null || body === undefined) {
            return body;
        }
    
        if (typeof body !== 'object') {
            return body;
        }
    
        for (const key of Object.keys(body)) {
            const value = body[key];
            if (value instanceof Date) {
                body[key] = new Date(Date.UTC(value.getFullYear(), value.getMonth(), value.getDate(), value.getHours(), value.getMinutes()
                    , value.getSeconds()));
            } else if (typeof value === 'object') {
                this.shiftDates(value);
            }
        }
      }

}