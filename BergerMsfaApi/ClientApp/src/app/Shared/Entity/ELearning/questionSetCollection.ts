import { Question } from './question';
import { QuestionSet } from './questionSet';

export class QuestionSetCollection {
    id: number;
    questionSetId: number;
    questionSet: QuestionSet;
    questionId: number;
    question: Question;
    mark: number;
    status: number;

    statusText: string;
    isSelected: boolean;
    questionTitle: string;
    questionTypeText: string;
    editDeleteId: number;
    
    constructor(init?: Partial<QuestionSetCollection>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = 0;
        this.questionSetId = 0;
        this.questionId = null;
        this.mark = null;
        this.status = null;
        this.statusText = '';
        this.isSelected = false;
        this.questionTitle = '';
        this.questionTypeText = '';
    }
}