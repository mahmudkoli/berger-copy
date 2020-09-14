export class APIResponse{
    public data: any;
    public errors: any [];
    public msg: string;
    public status: string;
    public statusCode: number;
}
export class APIResponsePage{
    public data: APIModel;
    public errors: any [];
    public msg: string;
    public status: string;
    public statusCode: number;
}

export class APIModel {
    public firstItemOnPage: number;
    public hasNextPage: boolean;
    public isLastPage: boolean;
    public lastItemOnPage: number;
    public model: any[];
    public pageCount: number;
    public pageNumber: number;
    public pageSize: number;
    public totalItemCount: number;
}