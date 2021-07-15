export class SyncSetup {
  public id: number;
  public syncHourlyInterval: number;
  public lastSyncTime: Date | string;

  //for client side

  constructor() {
    this.id = 0;
  }
}
