import { Component, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/Product';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatDivider } from '@angular/material/divider';
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CurrencyPipe,MatButton,MatIcon,MatFormField,MatInput,MatLabel,MatDivider,FormsModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit{

  product?:Product
  quantityInCart = 0;
  quantity = 1;
  
  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute, private cartService: CartService){}
  
  
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct(){
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if(!id) return;

    this.shopService.getProduct(+id).subscribe({
      next: product => {
        this.product = product;
        this.updateQuantityInCart();
      },
      error: err => console.log(err)
    });
  }

  updateQuantityInCart(){
    this.quantityInCart = this.cartService.cart()?.items.find(x => x.productId == this.product?.id)?.quantity || 0;
    this.quantity = this.quantityInCart || 1;
  }

  getButtontext(){
    return this.quantityInCart > 0 ? 'Mettre à jour' : 'Ajouter au panier'
  }

  updateCart(){
    if(!this.product) return;
    if(this.quantity > this.quantityInCart){
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.quantityInCart += itemsToAdd;
      this.cartService.addItemToCart(this.product, itemsToAdd)
    } else{
      const itemsToRemove = this.quantityInCart - this.quantity;
      this.quantityInCart += itemsToRemove;
      this.cartService.removeItemFromCart(this.product.id, itemsToRemove)
    }
  }
}
