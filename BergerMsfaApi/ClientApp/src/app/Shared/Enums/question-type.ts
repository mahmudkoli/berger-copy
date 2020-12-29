import { MapObject } from "./mapObject";

export enum EnumQuestionType {
    SingleChoice = 1,
    MultipleChoice = 2
}

export class EnumQuestionTypeLabel {
    public static enumQuestionTypeLabel: MapObject[] = [
        { id: 1, label: "Single Choice" },
        { id: 2, label: "Multiple Choice" }
    ];

    constructor() {
    }
}
