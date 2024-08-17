import { Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, Observable, of } from 'rxjs';
import { Cart } from '../../shared/models/cart';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {


  constructor(private cartService:CartService, private accountService: AccountService) { }

  init(){
    const cardId = localStorage.getItem("cart_id");
    const cart$ = cardId ? this.cartService.getCart(cardId) : of(null);
    return forkJoin({
      cart: cart$,
      user: this.accountService.getUserInfo()
    });
  }
}
