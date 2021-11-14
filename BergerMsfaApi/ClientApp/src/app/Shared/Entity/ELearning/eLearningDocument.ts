import { QueryObject } from '../Common/query-object';
import { Dropdown } from '../Setup/dropdown';
import { ELearningAttachment } from './eLearningAttachment';

export class ELearningDocument {
    id: number;
    title: string;
    categoryId: number;
    category: Dropdown;
    uploadDate: string;
    status: number;
    eLearningAttachments: ELearningAttachment[];
    attachedFileName: string;
    attachedLinkAddress: string;

    statusText: string;
    categoryText: string;
    
    constructor(init?: Partial<ELearningDocument>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.title = '';
        this.categoryId = null;
        this.status = null;
        this.eLearningAttachments = [];
        this.statusText = '';
        this.categoryText = '';
        this.uploadDate = '';
        this.attachedFileName = '';
        this.attachedLinkAddress = '';
    }
}

export class SaveELearningDocument {
    id: number;
    title: string;
    categoryId: number;
    status: number;
    eLearningAttachmentFiles: File[];
    eLearningAttachmentUrls: string[];
    eLearningAttachments: ELearningAttachment[];
    
    constructor(init?: Partial<SaveELearningDocument>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.title = '';
        this.categoryId = null;
        this.status = null;
        this.eLearningAttachmentFiles = [];
        this.eLearningAttachmentUrls = [];
        this.eLearningAttachments = [];
    }
}

export class ELearningDocumentQuery extends QueryObject {
    title: string;
    
    constructor(init?: Partial<ELearningDocumentQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.title = '';
    }
}