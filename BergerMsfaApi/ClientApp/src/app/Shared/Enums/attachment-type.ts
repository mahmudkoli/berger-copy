import { MapObject } from "./mapObject";

export enum EnumAttachmentType {
    File = 1,
    Link = 2
}

export class EnumAttachmentTypeLabel {
    public static enumAttachmentTypeLabel: MapObject[] = [
        { id: 1, label: "File" },
        { id: 2, label: "Link" }
    ];

    constructor() {
    }
}
