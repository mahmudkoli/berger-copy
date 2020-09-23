import { Status } from "src/app/Shared/Enums/status";
import { YesNo } from "src/app/Shared/Enums/yesno";

export class Product {
    public id: number;
    public name: string;
    public type: string;
    public actionType: number;
    public isJTIProduct: boolean;
    public status: number;
    public code: string;

    //for client side
    isSelected: boolean;
    isJTIProductLabel: string;

    constructor() {
        this.id = 0;
        this.name = '';
        this.type = '';
        //this.isJTIProduct = YesNo.Yes;
        this.status = Status.Active;
        this.isSelected = false;
    }
}