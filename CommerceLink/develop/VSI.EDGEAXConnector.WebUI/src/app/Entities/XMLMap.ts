export class XMLMap {
    public SourceEntity: string;
    public Name: string;
    public Type: string;
    public XML: string;
    public isActive: boolean;
    constructor() {
        this.SourceEntity = "";
        this.Name = "";
        this.Type = "";
        this.XML = "";
        this.isActive = true;
    }
}