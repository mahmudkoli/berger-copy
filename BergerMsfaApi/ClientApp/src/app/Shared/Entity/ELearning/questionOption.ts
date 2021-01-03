import { Question } from "./question";

export class QuestionOption {
    id: number;
    title: string;
    sequence: number;
    isCorrectAnswer: boolean;
    questionId: number;
    question: Question;
    status: number;
    statusText: string;
    isCorrectAnswerText: string;
    editDeleteId: number; // for frontend
    
    constructor(init?: Partial<QuestionOption>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = 0;
        this.title = '';
        this.sequence = null;
        this.isCorrectAnswer = false;
        this.questionId = 0;
        this.status = null;
        this.statusText = '';
        this.isCorrectAnswerText = '';
    }
}