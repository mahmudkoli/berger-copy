"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.CollectionEntryModule = void 0;
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var ng_bootstrap_1 = require("@ng-bootstrap/ng-bootstrap");
var forms_1 = require("@angular/forms");
var angular_font_awesome_1 = require("angular-font-awesome");
var ng_select_1 = require("@ng-select/ng-select");
var shared_master_module_1 = require("../../Shared/Modules/shared-master/shared-master.module");
var collection_entry_list_component_1 = require("./collection-entry-list/collection-entry-list.component");
var collectionEntry_routing_1 = require("./collectionEntry-routing");
//import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';
var CollectionEntryModule = /** @class */ (function () {
    function CollectionEntryModule() {
    }
    CollectionEntryModule = __decorate([
        core_1.NgModule({
            declarations: [collection_entry_list_component_1.CollectionEntryListComponent],
            imports: [
                common_1.CommonModule,
                shared_master_module_1.SharedMasterModule,
                angular_font_awesome_1.AngularFontAwesomeModule,
                collectionEntry_routing_1.CollectionEntryRoutingModule,
                ng_bootstrap_1.NgbModule,
                forms_1.FormsModule,
                forms_1.ReactiveFormsModule,
                ng_select_1.NgSelectModule
            ]
        })
    ], CollectionEntryModule);
    return CollectionEntryModule;
}());
exports.CollectionEntryModule = CollectionEntryModule;
//# sourceMappingURL=collectionEntry.module.js.map