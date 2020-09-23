export class Node{
    public id:number;
    public nodeId:number;
    public code:string;
    public name:string;
    public parentId:number;
    public status:number;
    public nodeIdList : number[];

    constructor() {
        this.nodeIdList = [];
    }
}

