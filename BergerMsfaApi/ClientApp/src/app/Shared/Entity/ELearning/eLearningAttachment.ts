
export class ELearningAttachment {
    id: number;
    name: string;
    path: string;
    size: number;
    format: string;
    attachmentType: number;
    
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
        this.attachmentType = 0;
    }
}
