import { Injectable } from '@angular/core';
import { CartItem, Cart } from '../models/cart-items';
import { http } from './http';
import { AppConfig } from '../constants/app-config'; 
import { Observable } from 'rxjs';
import { text } from '@angular/core/src/render3';
import { GetQouteRequest } from '../_api-models/qouteGetRequest';
import { GetQouteResponse, TestItems, TestCart } from '../_api-models/qouteGetResponse';
import { injectTemplateRef } from '@angular/core/src/render3/view_engine_compatibility';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class QouteService {
  public cart: Cart = new Cart();
  

  
  constructor(private qouteService:http, private myhttp: HttpClient) { }


  extractData(res: Response) {
    const body = res;
    if(body['Success']){
      body['Quotations'].forEach((value, key) => {
          let testCart = new TestCart();  
          value && value.Items && value.Items.length &&  value.Items.forEach((item,index) => {
                let itemsTest = new TestItems();
                itemsTest.sku = item.ITEMID;
                testCart.Items.push(itemsTest)
          });
          return testCart;
          
      }); 
    }
  }

  //Get Qoute
 GetQoute(getQoute: GetQouteRequest){
    return this.myhttp.post(AppConfig.getQoute, getQoute).pipe(map(this.extractData));
  }

}
