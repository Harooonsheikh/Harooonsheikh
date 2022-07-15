import { Injectable } from '@angular/core';
import { Cart } from '../models/cart-items';
import { http } from './http';
import { AppConfig } from '../constants/app-config'; 
import { Observable } from 'rxjs';
import { text } from '@angular/core/src/render3';
import { CartLinesCreateRequest, CartLinesRemoveRequest } from '../_api-models/cartLinesCreateRequest';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private cartService:http) { }

  //Get Cart
  public GetCart(cart: Cart){
    return this.cartService.get(AppConfig.GetCart+""+cart);
  }

  //Create or Update Cart
  public CreateOrUpdateCart(cart: Cart){
    return this.cartService.post(AppConfig.CreateOrUpdateCart, cart);
  }

  
  public CreateCartMerged(cart: Cart){
    return this.cartService.post(AppConfig.createCartMerged, cart);
  }

  //Add Cart Lines
  public AddCartLines(cartLinesModel: CartLinesCreateRequest){
    return this.cartService.post(AppConfig.AddCartLines, cartLinesModel);
  }

  //Update Cart Lines
  public UpdateCartLines(cartLinesModel: CartLinesCreateRequest){
    return this.cartService.post(AppConfig.UpdateCartLines, cartLinesModel);
  }

  //Remove Cart Lines
  public RemoveCartLines(cartId: string, LineIds: Array<string>){
    let model: CartLinesRemoveRequest = new CartLinesRemoveRequest();
    model.CartId = cartId;
    model.CalculationModes = "All";
    model.LineIds = LineIds;

    return this.cartService.post(AppConfig.RemoveCartLines, model);
  }
}
