import { Status } from "src/app/Shared/Enums/status";
import { YesNo } from "src/app/Shared/Enums/yesno";

export class Dropdown {

    public id: number;
    public typeId: number;
    public typeName: string;
    public typeCode: string;
    public dropdownCode: string;
    public dropdownName: string;
    public description: string;
    public sequence: number;
    public status: number;

    //for client side


    constructor() {
        this.id = 0;
        this.typeId=0,
        this.typeCode = '';
        this.typeName = '';
        this.dropdownCode = '';
        this.dropdownName = '';
        this.description = '';
        this.sequence = 0;
        this.status = 1;
    }
}