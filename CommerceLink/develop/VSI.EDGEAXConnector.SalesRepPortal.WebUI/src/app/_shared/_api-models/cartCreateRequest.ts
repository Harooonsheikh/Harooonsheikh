
import * as uuid from 'uuid';

export class CartCreateRequest{
    CalculationModes: string;
    IsUpdate: boolean;
    Cart: CartDetail;
    constructor(){
      this.CalculationModes = "All";
      this.IsUpdate = false;  
      this.Cart = new CartDetail();
    }
}

export class CartDetail{
    Id: string;
    CartTypeValue: string;
    constructor(){
        this.Id = "NAL-"+ uuid.v4();
        this.CartTypeValue = "1";
    }
}

