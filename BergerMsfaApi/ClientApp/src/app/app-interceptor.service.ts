import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, finalize, switchMap, take } from 'rxjs/operators';
import { AlertService } from './Shared/Modules/alert/alert.service';
import { ActivityPermissionService } from './Shared/Services/Activity-Permission/activity-permission.service';
import { AuthService } from './Shared/Services/Users';

@Injectable({ providedIn: 'root' })
export class AppInterceptorService implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  constructor(
    private alertService: AlertService,
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private router: Router,
  ) {}

  handleError = (error: HttpErrorResponse, request?, next?) => {
    setTimeout(() => {
      //this.alertService.fnLoading(false);
      let statusCode = error.status;
      let errorMsg =
        (error.error == null ? error.message : error.error.msg) ||
        'Something went wrong.';
      if (statusCode == 0) {
        errorMsg =
          'You may have internet connection problem. Check your network and try again.';
      } else if (statusCode == 404) {
        errorMsg =
          'You may have application issues. Please contact with system admin.';
      } else if (statusCode == 401 || statusCode == 403) {
        errorMsg = 'You are not authorized. Please contact with system admin.';
      } else if (statusCode == 400) {
        errorMsg =
          'Validation failed for ' +
          error.error.errors[0].propertyName +
          '. ' +
          error.error.errors[0].errorList[0];
      }

      //showing message
      if (errorMsg != '') {
        this.alertService.titleTosterDanger(errorMsg);
      }
    }, 1000);

    return throwError(error);
  };
  
  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((authUser: any) => {
          this.isRefreshing = false;
          const jwt = authUser && authUser.token;
          this.refreshTokenSubject.next(jwt);
          return next.handle(this.addToken(request, jwt));
        }),
        catchError((error) => {
          this.goLogin();
          return throwError(error);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter((token) => token != null),
        take(1),
        switchMap((jwt) => {
          return next.handle(this.addToken(request, jwt));
        }),
        catchError((error) => {
          this.goLogin();
          return throwError(error);
        })
      );
    }
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    this.alertService.fnLoading(true);

    const token = this.authService.currentUserToken;

    if (request.method === 'POST' || request.method === 'PUT') {
      this.shiftDates(request.body);
    }
    
    return this.nextHandleCall(request, next, token);
  }

  nextHandleCall(request: HttpRequest<any>, next: HttpHandler, token: string) {
    return next.handle(this.addToken(request, token)).pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(request, next);
        } else {
          return this.handleError(error, request, next);
        }
      }),
      finalize(() => {
        setTimeout(() => {
          this.alertService.fnLoading(false);
        }, 1000);
      })
    );
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
        body[key] = new Date(
          Date.UTC(
            value.getFullYear(),
            value.getMonth(),
            value.getDate(),
            value.getHours(),
            value.getMinutes(),
            value.getSeconds()
          )
        );
      } else if (typeof value === 'object') {
        this.shiftDates(value);
      }
    }
  }
  
  private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
  }
	
	goLogin() {
    this.alertService.fnLoading(false);
		this.router.navigate([`/auth/login`], { relativeTo: this.activatedRoute });
	}
}
