import { QueryObject } from '../Common/query-object';
import { UserInfo } from '../Users/userInfo';
import { QuestionSet } from './questionSet';
import { UserQuestionAnswerCollection } from './userQuestionAnswerCollection';

export class UserQuestionAnswer {
    id: number;
    // userInfoId: number;
    // userInfo: UserInfo;
    userFullName: string
    employeeId: string
    // questionSetId: number;
    // questionSet: QuestionSet;
    questionSetTitle: string;
    questionSetLevel: string;
    examDate: string;
    totalMark: number;
    passMark: number;
    userMark: number;
    // totalCorrectAnswer: number;
    // passed: boolean;
    passStatus: string;
    // status: number;
    // questionAnswerCollections: UserQuestionAnswerCollection[];

    // statusText: string;
    // passedText: string;
    // questionSetTitle: string;
    // questionSetLevel: string;
    // questionSetTotalMark: string;
    // questionSetPassMark: string;
    
    constructor(init?: Partial<UserQuestionAnswer>) {
        Object.assign(this, init);
    }

    clear() {
        // this.id = null;
        // this.userInfoId = null;
        // this.userInfo = null;
        // this.userFullName = '';
        // this.employeeId = '';
        // this.questionSetId = null;
        // this.questionSet = null;
        // this.examDate = '';
        // this.totalMark = null;
        // this.totalCorrectAnswer = null;
        // this.passed = null;
        // this.status = null;
        // this.questionAnswerCollections = [];
        // this.statusText = '';
        // this.passedText = '';
        // this.questionSetTitle = '';
        // this.questionSetLevel = '';
        // this.questionSetTotalMark = '';
        // this.questionSetPassMark = '';
    }
}

export class UserQuestionAnswerQuery extends QueryObject  {
    
    constructor(init?: Partial<UserQuestionAnswerQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}