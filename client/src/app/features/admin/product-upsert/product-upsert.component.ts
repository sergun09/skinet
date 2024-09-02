import { Component, inject, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { Product } from '../../../shared/models/Product';
import { ActivatedRoute } from '@angular/router';
import { ShopService } from '../../../core/services/shop.service';

@Component({
  selector: 'app-product-upsert',
  standalone: true,
  imports: [MatFormField,MatLabel,ReactiveFormsModule],
  templateUrl: './product-upsert.component.html',
  styleUrl: './product-upsert.component.scss'
})
export class ProductUpsertComponent implements OnInit{

  shopService = inject(ShopService)
  productForm!: FormGroup;
  buttonText: string = '';
  productId : number = 0;
  product! : Product

  constructor(private fb: FormBuilder,private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if(id && +id !== 0){
      this.productId = +id;
    }

    this.buttonText = this.productId === 0 ? "Créer" : "Mettre à jour";

    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      pictureUrl: ['', Validators.required],
      brand: ['', Validators.required],
      type: ['', Validators.required],
      quantityInStock: [0, [Validators.required, Validators.min(0)]]
    });
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      this.product = this.productForm.value;

      if(this.productId && this.productId != 0){
        this.shopService.updateProduct(this.product, this.productId)
      }
      else{
        this.shopService.createProduct(this.product)
      }
    }
  }

}
