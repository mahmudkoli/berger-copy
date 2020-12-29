import { Question } from "./question";

export class QuestionOption {
    id: number;
    title: string;
    sequence: number;
    questionId: number;
    question: Question;
    status: number;
    statusText: string;
    editDeleteId: number; // for frontend
    
    constructor(init?: Partial<QuestionOption>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = 0;
        this.title = '';
        this.sequence = null;
        this.questionId = 0;
        this.status = null;
        this.statusText = '';
    }
}