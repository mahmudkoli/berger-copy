import { Status } from "../../Enums/status";

export class Role {
    public id: number;
    public name: string;
    public status: number;
    constructor() {
        this.id = 0;
        this.name = '';
        this.status = Status.Active;
    }
}