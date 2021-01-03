
export class ELearningAttachment {
    id: number;
    name: string;
    path: string;
    size: number;
    format: string;
    type: number;
    status: number;
    
    constructor(init?: Partial<ELearningAttachment>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = 0;
        this.name = '';
        this.path = '';
        this.size = 0;
        this.name = '';
        this.format = '';
        this.type = 0;
        this.status = 0;
    }
}

export class SaveELearningAttachment {
    eLearningDocumentId: number;
    attachmentLink: string;
    attachmentFile: File;
    
    constructor(init?: Partial<SaveELearningAttachment>) {
        Object.assign(this, init);
    }

    clear() {
        this.eLearningDocumentId = 0;
        this.attachmentLink = '';
        this.attachmentFile = null;
    }
}