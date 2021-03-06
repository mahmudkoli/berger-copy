import { MapObject } from "./mapObject";

export enum EnumClubSupreme {
    None = 0,
    Gold = 1,
    Platinum = 2,
    PlatinumPlus = 3
}

export enum EnumBussinesCategory {
    None = 0,
    Exclusive = 1,
    NonExclusive = 2,
    NonAPNonExclusive = 3,
    NewDealer = 4
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

export class EnumBussinesCategoryLabel {
    public static enumBussinesCategoryLabel: MapObject[] = [
        { id: 0, label: "None" },
        { id: 1, label: "Exclusive" },
        { id: 2, label: "Non-Exclusive" },
        { id: 3, label: "Non AP Non-Exclusive" },
        { id: 4, label: "New Dealer" },
    ];

    constructor() {
    }
}

export enum EnumDealerStatusExcelImportType {
    // Exclusive = 1,
    LastYearAppointed = 2,
    ClubSupreme = 3,
    // AP = 4,
    BussinessCategory = 5,

}

export class EnumDealerStatusExcelImportTypeLabel {
    public static enumDealerStatusExcelImportTypeLabel: MapObject[] = [
        // { id: 1, label: "Exclusive" },
        { id: 2, label: "Last Year Appointed" },
        { id: 3, label: "Club Supreme" },
        // { id: 4, label: "AP" },
        { id: 5, label: "Bussiness Category" },

    ];

    constructor() {
    }
}