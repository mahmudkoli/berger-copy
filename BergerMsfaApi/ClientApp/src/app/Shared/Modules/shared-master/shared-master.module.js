"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.SharedMasterModule = void 0;
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
//import { PageTitleComponent } from 'src/app/Layout/LayoutComponent/Components/page-title/page-title.component';
var angular_font_awesome_1 = require("angular-font-awesome");
var alert_module_1 = require("../alert/alert.module");
var p_table_module_1 = require("../p-table/p-table.module");
//import { ImageUploaderComponent } from 'src/app/Shared/Modules/image-uploader/image-upload.component';
//import { TableModule } from 'primeng-lts/table'
var table_1 = require("primeng/table");
var button_1 = require("primeng-lts/button");
var SharedMasterModule = /** @class */ (function () {
    function SharedMasterModule() {
    }
    SharedMasterModule = __decorate([
        core_1.NgModule({
            declarations: [
            //PageTitleComponent,
            //ImageUploaderComponent
            ],
            imports: [
                common_1.CommonModule,
                angular_font_awesome_1.AngularFontAwesomeModule,
                alert_module_1.AlertModule,
                p_table_module_1.PTableModule,
                table_1.TableModule,
                button_1.ButtonModule
            ],
            exports: [
                //   PageTitleComponent,
                //ImageUploaderComponent, 
                // AlertModule,
                p_table_module_1.PTableModule,
                button_1.ButtonModule,
                table_1.TableModule,
            ]
        })
    ], SharedMasterModule);
    return SharedMasterModule;
}());
exports.SharedMasterModule = SharedMasterModule;
//# sourceMappingURL=shared-master.module.js.map