export class KeyValue<T>{
    public Key: string = null;
    public Value: T = null;
    constructor() {
        this.Key = null;
        this.Value = null;
    }
}
export class KeyValuePair<T>{
    public Key: number = -1;
    public Value: T = null;
}
export enum FlowStep {
    Start,
    Success = 1,
    Failure = 2,
    Processing = 3
}
export enum CatalogModel {
    Product = 1,
    Category = 2,
    CategoryAssignment = 3,
}