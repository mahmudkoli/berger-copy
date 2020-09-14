import { MapObject } from './mapObject';

export enum POSMActionType {
    Installation = 1,
    Repair = 2,
    Removal = 3
}

export class POSMActionTypeLabel{

    public static POSMActionType :  MapObject[] = [
    { id : 1, label : "Installation" },
    { id : 2, label : "Repair" },
    { id : 3, label : "Removal" }
    
    ];


}
