//export enum RejectedStatus {
//    ReturnFromAFMM = 1,
//    ReturnFromRFMM = 2,
//    ReturnFromNFMM = 3,
//    ReturnFromFMD = 4
//}

import { MapObject } from './mapObject';

export class RejectedStatuses {

    public static rejectedStatus: MapObject[] = [
        { id: 1, label: "Return From AFMM" },
        { id: 2, label: "Return From RFMM" },
        { id: 3, label: "Return From NFMM" },
        { id: 4, label: "Return From FMD" }
    ];

    constructor() {

    }

}