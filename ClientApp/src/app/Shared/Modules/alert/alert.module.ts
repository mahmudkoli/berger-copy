import {NgModule} from '@angular/core'
import {CommonModule} from '@angular/common'
import { FormsModule,  } from '@angular/forms';
import {AlertComponent} from './alert.component';
import { AngularFontAwesomeModule } from 'angular-font-awesome';

@NgModule({
    imports:[CommonModule,
        FormsModule,
        AngularFontAwesomeModule],
    declarations:[
        AlertComponent
    ],
    exports:[AlertComponent],
})

export class AlertModule{}