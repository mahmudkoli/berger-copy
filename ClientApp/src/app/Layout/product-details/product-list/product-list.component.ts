import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/Shared/Services/Product/product.service';
import { Product } from 'src/app/Shared/Entity/Products/product';
import { ActivatedRoute, Router } from '@angular/router';
import { Status } from 'src/app/Shared/Enums/status';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { MapObject } from "../../../Shared/Enums/mapObject";
import { PosmProductType } from "../../../Shared/Enums/posmproducttype";
import { StatusTypes } from "../../../Shared/Enums/statusTypes";
import { PermissionGroup, ActivityPermissionService } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';

@Component({
    selector: 'app-product-list',
    templateUrl: './product-list.component.html',
    styleUrls: ['./product-list.component.sass']
})
export class ProductListComponent implements OnInit {

    //heading = 'Product List';
    //subheading = 'Get all products information';
    //icon = 'pe-7s-drawer icon-gradient bg-happy-itmeo';

    enumPosmProductType: MapObject[] = PosmProductType.posmProductType;
    enumStatusTypes: MapObject[] = StatusTypes.statusType;
    yesnoOptions = {};
    permissionGroup: PermissionGroup = new PermissionGroup();

    constructor(private productService: ProductService,
         private alertService: AlertService,
         private activityPermissionService: ActivityPermissionService,
         private activatedRoute: ActivatedRoute,
          private router: Router) {
            this.initPermissionGroup();

    }
    
    ngOnInit() {
        this.fnGetProductList();
    }


    private initPermissionGroup() {
        this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
        console.log(this.permissionGroup);
        this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
        this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
        this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
    }

    public productList:Product[]=[];
    private fnGetProductList() {
        this.alertService.fnLoading(true);
        this.productService.getProductList().subscribe(
            (res) => {
                let productsData = res.data.model || [];
                productsData.forEach(obj => {
                    obj.statusText = this.enumStatusTypes.filter(k => k.id == obj.status)[0].label;
                    obj.isJTIProductText = obj.isJTIProduct ? "Yes" : "No";
                });
                console.log("productsData", productsData);
                this.productList = productsData;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }

    private fnRouteAddProd() {
        this.router.navigate(['/product/product-add']);
    }

    private edit(id: number) {
        console.log('edit product',id);
        this.router.navigate(['/product/product-add/' + id]);
    }

    private delete(id: number) {
        // console.log("Id:", id);
        this.alertService.confirm("Are you sure you want to delete this item?", () => {
            this.productService.deleteProduct(id).subscribe(
                (res: any) => {
                    console.log('res from del func',res);          
                    this.alertService.tosterSuccess("Product has been deleted successfully.");
                    this.fnGetProductList();
                },
                (error) => {
                    console.log(error);
                }
            );
        }, () => {

        });
    }

    public fnPtableCellClick(event) {
        console.log("cell click: ");
    }

    public ptableSettings = {
        tableID: "Products-table",
        tableClass: "table table-border ",
        tableName: 'Products List',
        tableRowIDInternalName: "Id",
        tableColDef: [
            { headerName: 'Product Code ', width: '10%', internalName: 'code', sort: true, type: "" },
            { headerName: 'Product Name ', width: '30%', internalName: 'name', sort: true, type: "" },
            { headerName: 'Product Type ', width: '20%', internalName: 'type', sort: true, type: "" },
            { headerName: 'Is JTI Product?', width: '20%', internalName: 'isJTIProductText', sort: true, type: "" },
            { headerName: 'Status', width: '20%', internalName: 'statusText', sort: true, type: "" }
        ],
        enabledSearch: true,
        enabledSerialNo: true,
        pageSize: 10,
        enabledPagination: true,
        //enabledAutoScrolled:true,
        enabledDeleteBtn: true,
        enabledEditBtn: true,
        // enabledCellClick: true,
        enabledColumnFilter: true,
        // enabledDataLength:true,
        // enabledColumnResize:true,
        // enabledReflow:true,
        // enabledPdfDownload:true,
        // enabledExcelDownload:true,
        // enabledPrint:true,
        // enabledColumnSetting:true,
        enabledRecordCreateBtn: true,
        // enabledTotal:true,
    };

    public fnCustomTrigger(event) {
        console.log("custom  click: ", event);

        if (event.action == "new-record") {
            this.fnRouteAddProd();
        }
        else if (event.action == "edit-item") {
            this.edit(event.record.id);
        }
        else if (event.action == "delete-item") {
            this.delete(event.record.id);
        }
    }

}

