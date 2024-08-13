import { Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { Observable, of } from 'rxjs';
import { Cart } from '../../shared/models/cart';

@Injectable({
  providedIn: 'root'
})
export class InitService {


  constructor(private cartService:CartService) { }

  init(){
    const cardId = localStorage.getItem("cart_id");
    const cart$ = cardId ? this.cartService.getCart(cardId) : of(null);
    return cart$;
  }
}
