import { Route } from './route';
import { SalesPoint } from './salespoint';

export class Outlet{
    public id:number;
    public outletId:number;
    public code:string;
    public name:string;
    public banglaName:string;
    public ownerName:string;
    public ownerNameBangla:string;
    public address:string;
    public addressBangla:string;
    public contactNumber:string;
    public location:string;
    public latitude:string;
    public longitude:string;
    public salesPointId:number;
    public routeId:number;
    public nodeId:number;
    
    public route : Route = new Route();
    public salesPoint: SalesPoint = new SalesPoint();
    
    constructor() {
        
    }
}