import { UserInfo } from '../Users/userInfo';
import { ELearningDocument } from './eLearningDocument';
import { Question } from './question';
import { QuestionSet } from './questionSet';
import { QuestionSetCollection } from './questionSetCollection';

export class UserQuestionAnswerCollection {
    id: number;
    questionId: number;
    question: Question;
    mark: number;
    answer: string;
    isCorrectAnswer: boolean;
    status: number;

    statusText: string;
    
    constructor(init?: Partial<UserQuestionAnswerCollection>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.questionId = null;
        this.question = null;
        this.mark = null;
        this.answer = '';
        this.isCorrectAnswer = null;
        this.status = null;
        this.statusText = '';
    }
}