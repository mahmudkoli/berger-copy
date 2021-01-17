import { UserInfo } from '../Users/userInfo';
import { QuestionSet } from './questionSet';
import { UserQuestionAnswerCollection } from './userQuestionAnswerCollection';

export class UserQuestionAnswer {
    id: number;
    userInfoId: number;
    // userInfo: UserInfo;
    userFullName: string
    questionSetId: number;
    questionSet: QuestionSet;
    totalMark: number;
    totalCorrectAnswer: number;
    passed: boolean;
    status: number;
    questionAnswerCollections: UserQuestionAnswerCollection[];

    statusText: string;
    passedText: string;
    questionSetTitle: string;
    questionSetLevel: string;
    questionSetTotalMark: string;
    questionSetPassMark: string;
    
    constructor(init?: Partial<UserQuestionAnswer>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.userInfoId = null;
        // this.userInfo = null;
        this.userFullName = '';
        this.questionSetId = null;
        this.questionSet = null;
        this.totalMark = null;
        this.totalCorrectAnswer = null;
        this.passed = null;
        this.status = null;
        this.questionAnswerCollections = [];
        this.statusText = '';
        this.passedText = '';
        this.questionSetTitle = '';
        this.questionSetLevel = '';
        this.questionSetTotalMark = '';
        this.questionSetPassMark = '';
    }
}

export class UserQuestionAnswerQuery {
    name: string;
    
    constructor(init?: Partial<UserQuestionAnswerQuery>) {
        Object.assign(this, init);
    }

    clear() {
        this.name = '';
    }
}