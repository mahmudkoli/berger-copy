import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgReduxModule } from '@angular-redux/store';
import { NgRedux, DevToolsExtension } from '@angular-redux/store';
import { rootReducer, ArchitectUIState } from './ThemeOptions/store';
import { ConfigActions } from './ThemeOptions/store/config.actions';
import { AppRoutingModule } from './app-routing.module';
import { LoadingBarRouterModule } from '@ngx-loading-bar/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent } from './app.component';

// BOOTSTRAP COMPONENTS
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { AppInterceptorService } from './app-interceptor.service';
import { AuthGuard } from './Shared/Guards/auth.guard';
import { PermissionGuard } from './Shared/Guards/permission.guard';
import { IAuthUser } from './Shared/Entity/Users/auth';
import { MustMatchDirective } from './Shared/Directive/mustmatch.directive';
import { JwtModule } from '@auth0/angular-jwt';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
    suppressScrollX: true
};

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

export function tokenGetter(): string {
	const authUser = JSON.parse(localStorage.getItem("currentUser")) as IAuthUser;
	return authUser ? authUser.token : "";
}

@NgModule({
    declarations: [
        AppComponent,
        MustMatchDirective
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        NgReduxModule,
        CommonModule,
        LoadingBarRouterModule,

        // Angular Bootstrap Components
        PerfectScrollbarModule,
        NgbModule,
        AngularFontAwesomeModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
		JwtModule.forRoot({
			config: {
				tokenGetter: tokenGetter
			}
		})
    ],
    providers: [{
        provide: HTTP_INTERCEPTORS,
        useClass: AppInterceptorService,
        multi: true
    },
    {
        provide:
            PERFECT_SCROLLBAR_CONFIG,
        // DROPZONE_CONFIG,
        useValue:
            DEFAULT_PERFECT_SCROLLBAR_CONFIG,
        // DEFAULT_DROPZONE_CONFIG,
    },
        ConfigActions,
        AuthGuard,
        PermissionGuard
    ],
    bootstrap: [AppComponent]
})

export class AppModule {
    constructor(private ngRedux: NgRedux<ArchitectUIState>,
        private devTool: DevToolsExtension) {
        this.ngRedux.configureStore(
            rootReducer,
            {} as ArchitectUIState,
            [],
            [devTool.isEnabled() ? devTool.enhancer() : f => f]
        );
    }
}
