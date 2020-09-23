import { Status } from "../../Enums/status";
import { YesNo } from "../../Enums/yesno";

export class PosmProduct {
    public id: number;
    public name: string;
    public type: number;
    public isJTIProduct: boolean;
    public isDigitalSignatureEnable: boolean;
    public isPlanogram: boolean;
    public planogramImageUrl: string;
    public status: number;
    public code: string;
    public planogramImageFile: File;
    public wFStatus: number;

    //for client side
    isSelected: boolean;
    isDigitalSignatureEnableLabel: string;

    constructor() {
        this.id = 0;
        this.name = '';
        //this.isJTIProduct = YesNo.Yes;
        //this.isDigitalSignatureEnable = YesNo.Yes;
        this.status = Status.Active;
        this.isSelected = false;
    }
}