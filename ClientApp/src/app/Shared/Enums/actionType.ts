import { MapObject } from './mapObject';

export enum ActionType {
    DistributionCheckProduct = 0,
    FacingCountProduct = 1,
    PlanogramCheckProduct = 2,
    PriceAuditProduct = 3
}

export class ActionTypeLabel{

    public static ActionType :  MapObject[] = [
    { id : 0, label : "Distribution Check Product" },
    { id : 1, label : "Facing Count Product" },
    { id : 2, label : "Planogram Check Product" },
    { id : 3, label : "Price Audit Product" }
    
    ];


}


