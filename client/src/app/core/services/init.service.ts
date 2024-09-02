import { Injectable } from '@angular/core';
import { CartService } from './cart.service';
import { forkJoin, Observable, of, tap } from 'rxjs';
import { Cart } from '../../shared/models/cart';
import { AccountService } from './account.service';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {


  constructor(private cartService:CartService, private accountService: AccountService, private signalrService: SignalrService) { }

  init(){
    const cardId = localStorage.getItem("cart_id");
    const cart$ = cardId ? this.cartService.getCart(cardId) : of(null);
    return forkJoin({
      cart: cart$,
      user: this.accountService.getUserInfo().pipe(
        tap(user => {
          if(user){
            this.signalrService.createHubConnection();
          }
        })
      )
    });
  }
}
