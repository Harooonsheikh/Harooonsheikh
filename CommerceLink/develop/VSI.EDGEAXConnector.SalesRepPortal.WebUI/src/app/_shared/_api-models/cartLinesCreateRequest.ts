import { LineToLineMappedSource } from "webpack-sources";
import { CartItem } from "../models/cart-items";

export class CartLinesCreateRequest{
    CartId: string;
    CalculationModes: string;
    CartVersion: number;
    CartLines: Array<CartItem>;
}


export class CartLinesRemoveRequest{
    CartId: string;
    CalculationModes: string;
    CartVersion: number;
    LineIds: Array<string>;
}

