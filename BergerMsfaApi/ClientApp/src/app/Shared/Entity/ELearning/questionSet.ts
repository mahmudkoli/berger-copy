import { QueryObject } from '../Common/query-object';
import { ELearningDocument } from './eLearningDocument';
import { QuestionSetCollection } from './questionSetCollection';

export class QuestionSet {
    id: number;
    title: string;
    level: number;
    totalMark: number;
    passMark: number;
    eLearningDocumentId: number;
    eLearningDocument: ELearningDocument;
    status: number;
    questionSetCollections: QuestionSetCollection[];

    statusText: string;
    
    constructor(init?: Partial<QuestionSet>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.title = '';
        this.level = null;
        this.totalMark = null;
        this.passMark = null;
        this.eLearningDocumentId = null;
        this.status = null;
        this.questionSetCollections = [];
        this.statusText = '';
    }
}

export class SaveQuestionSet {
    id: number;
    title: string;
    level: number;
    totalMark: number;
    passMark: number;
    eLearningDocumentId: number;
    status: number;
    questionSetCollections: QuestionSetCollection[];
    
    constructor(init?: Partial<SaveQuestionSet>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.title = '';
        this.level = null;
        this.totalMark = null;
        this.passMark = null;
        this.eLearningDocumentId = null;
        this.status = null;
        this.questionSetCollections = [];
    }
}

export class QuestionSetQuery extends QueryObject  {
    title: string;
    
    constructor(init?: Partial<QuestionSetQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.title = '';
    }
}