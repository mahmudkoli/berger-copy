//export enum PosmProductType {
//    Permanent = 1,
//    SemiPermanent = 2,
//    Temporary = 3
//}


import { MapObject } from './mapObject';

export class PosmProductType {


    public static posmProductType: MapObject[] = [
        { id: 1, label: "Permanent" },
        { id: 2, label: "Semi Permanent" },
        { id: 3, label: "Temporary" }
    ];


    constructor() {

    }

}
