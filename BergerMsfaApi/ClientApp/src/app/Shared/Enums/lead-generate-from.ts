import { MapObject } from './mapObject';

export enum EnumLeadGenerateFrom {
    MSFA = 0,
    HappyWallet = 1,
}

export class EnumLeadGenerateFromLabel {
    public static LeadGenerateFroms :  MapObject[] = [
        { id : -1, label : "All" },
        { id : 0, label : "MSFA" },
        { id : 1, label : "HappyWallet" },
    ];
}
