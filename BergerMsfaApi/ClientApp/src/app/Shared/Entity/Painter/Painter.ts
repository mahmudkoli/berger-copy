


export class Painter {

    public depotName: string;
    public painterName: string;
    public saleGroup: string;
    public Address: string;
    public Phone: string;
    public HasDbbl: boolean;
    public AccNumber: string;
    public AccHolderName: string;
    public PersonlIdentityNo: string;
    public painterImageUrl: string;
    public IsAppInstalled: boolean;
    public Remark: string;
    public AvgMonthlyVal: number;
    public Loality: number
    public DealerId: number;
    public PainterCatId: number;
    public TerritoryId: number;
    public status: number;
    public statusText: string;
    public statusBtnClass: string;
    public statusBtnIcon: string;
    

    constructor() {

    }
}

export class PainterUpdate {
    id: number;
    depot: string;
    // saleGroup: string;
    territory: string;
    zone: string;
    painterCatId: number;
    painterName: string;
    address: string;
    phone: string;
    noOfPainterAttached: number;
    hasDbbl: boolean;
    accDbblNumber: string;
    accDbblHolderName: string;
    passportNo: string;
    nationalIdNo: string;
    brithCertificateNo: string;
    painterImageUrl: string;
    painterImageBase64: string;
    // attachedDealerCd: string;
    isAppInstalled: boolean;
    remark: string;
    avgMonthlyVal: number;
    loyality: number;
    employeeId: string;
    painterNo: string;
    painterCode: string;
    attachedDealerIds: number[];

    constructor(init?: Partial<PainterUpdate>) {
        this.attachedDealerIds = [];
        Object.assign(this, init);
    }

    clear() {
        this.attachedDealerIds = [];
    }
}