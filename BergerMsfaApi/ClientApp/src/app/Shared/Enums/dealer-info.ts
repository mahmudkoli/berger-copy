import { MapObject } from "./mapObject";

export enum EnumClubSupreme {
    None = 0,
    Gold = 1,
    Platinum = 2,
    PlatinumPlus = 3
}

export class EnumClubSupremeLabel {
    public static enumClubSupremeLabel: MapObject[] = [
        { id: 0, label: "None" },
        { id: 1, label: "Gold" },
        { id: 2, label: "Platinum" },
        { id: 3, label: "Platinum Plus" },
    ];

    constructor() {
    }
}

export enum EnumDealerStatusExcelImportType {
    Exclusive = 1,
    LastYearAppointed = 2,
    ClubSupreme = 3,
    AP = 4
}

export class EnumDealerStatusExcelImportTypeLabel {
    public static enumDealerStatusExcelImportTypeLabel: MapObject[] = [
        { id: 1, label: "Exclusive" },
        { id: 2, label: "Last Year Appointed" },
        { id: 3, label: "Club Supreme" },
        { id: 4, label: "AP" },
    ];

    constructor() {
    }
}