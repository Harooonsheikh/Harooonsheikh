export class Action {
    constructor() {
        this.ActionParams = new Array<ActionParam>();
    }
    public ActionId: number = 0;
    public StoreId: number = 0;
    public ActionName: string = null;
    public ActionRoute: string = null;
    public RequestType: string = null;
    public ActionPath: string = null;
    public APIURL: string = null;
    public APIKey: string = null;
    public ActionParams: Array<ActionParam> = null;
    public Request: any = null;
}
export class ActionParam {
    public ParamId: number = 0;
    public ActionId: number = 0;
    public Key: string = null;
    public Value: string = null;
    public Type: string = null;
}
