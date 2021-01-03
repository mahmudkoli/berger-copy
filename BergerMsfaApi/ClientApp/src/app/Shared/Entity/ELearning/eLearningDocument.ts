import { Dropdown } from '../Setup/dropdown';
import { ELearningAttachment } from './eLearningAttachment';

export class ELearningDocument {
    id: number;
    title: string;
    categoryId: number;
    category: Dropdown;
    status: number;
    eLearningAttachments: ELearningAttachment[];

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

export class ELearningDocumentQuery {
    name: string;
    
    constructor(init?: Partial<ELearningDocumentQuery>) {
        Object.assign(this, init);
    }

    clear() {
        this.name = '';
    }
}