import { EnumQuestionType } from '../../Enums/question-type';
import { Dropdown } from '../Setup/dropdown';
import { ELearningAttachment } from './eLearningAttachment';
import { ELearningDocument } from './eLearningDocument';
import { QuestionOption } from './questionOption';

export class Question {
    id: number;
    title: string;
    eLearningDocumentId: number;
    eLearningDocument: ELearningDocument;
    type: EnumQuestionType;
    status: number;
    questionOptions: QuestionOption[];

    statusText: string;
    eLearningDocumentText: string;
    typeText: string;
    
    constructor(init?: Partial<Question>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.title = '';
        this.eLearningDocumentId = null;
        this.type = null;
        this.status = null;
        this.questionOptions = [];
        this.statusText = '';
        this.eLearningDocumentText = '';
        this.typeText = '';
    }
}

export class SaveQuestion {
    id: number;
    title: string;
    eLearningDocumentId: number;
    type: EnumQuestionType;
    status: number;
    questionOptions: QuestionOption[];
    
    constructor(init?: Partial<SaveQuestion>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.title = '';
        this.eLearningDocumentId = null;
        this.type = null;
        this.status = null;
        this.questionOptions = [];
    }
}

export class QuestionQuery {
    name: string;
    
    constructor(init?: Partial<QuestionQuery>) {
        Object.assign(this, init);
    }

    clear() {
        this.name = '';
    }
}