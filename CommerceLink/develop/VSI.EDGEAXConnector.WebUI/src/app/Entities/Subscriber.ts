export class Subscriber {
    constructor() {
        this.EmailSubscribers = new Array<EmailSubscriber>();
    }
    public Id: number = 0;
    public Email: string = null;
    public Name: string = null;
    public IsActive: boolean = false;
    public StoreId_FK: number = 0;
    public EmailSubscribers: Array<EmailSubscriber> = null;
    public CreatedByUser: string = null;
    public ModifiedByUser: string = null;
}
export class EmailSubscriber {
    public TemplateId: number = 0;
    public SubscriberId: number = 0;
}
export class Template {
    public TemplateId: number = 0;
    public Name: string = null;
    public IsSelected: boolean = false;
}