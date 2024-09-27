import { Component, inject, OnInit } from '@angular/core';
import { FormGroup, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { Product } from '../../../shared/models/Product';
import { ActivatedRoute } from '@angular/router';
import { ShopService } from '../../../core/services/shop.service';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatInput, MatInputModule } from '@angular/material/input';
import { MatOption, MatSelect, MatSelectChange } from '@angular/material/select';
import { CommonModule, JsonPipe } from '@angular/common';
import { UpsertProductDto } from '../../../shared/models/upsertProductDto';

@Component({
  selector: 'app-product-upsert',
  standalone: true,
  imports: [ReactiveFormsModule,MatCard,MatFormField,MatInput,MatLabel,MatButton,MatInputModule,MatSelect,MatOption,JsonPipe,CommonModule],
  templateUrl: './product-upsert.component.html',
  styleUrl: './product-upsert.component.scss'
})
export class ProductUpsertComponent implements OnInit{

  shopService = inject(ShopService)
  //private activatedRoute = inject(ActivatedRoute);
  
  brands = this.shopService.brands;
  types = this.shopService.types;

  buttonText: string = 'Créer';
  // productId : number = 0;  
  // product? : Product;

  productForm = new FormGroup({
    name: new FormControl("",{ validators : Validators.required, nonNullable : true}),
    description: new FormControl('', {validators: [Validators.required, Validators.minLength(20)], nonNullable : true}),
    price: new FormControl(1, {validators :[Validators.required, Validators.min(1)], nonNullable : true}),
    brand: new FormControl('', {validators : Validators.required, nonNullable: true}),
    type: new FormControl('', {validators : Validators.required, nonNullable : true}),
    quantityInStock: new FormControl(1, {validators:[Validators.required, Validators.min(1)], nonNullable:true}),
  }); 
  selectedFile: File | null = null;

  
  ngOnInit(): void {
    // this.loadProduct();
    // this.buttonText = this.productId === 0 || undefined ? "Créer" : "Mettre à jour";
  }

  onSubmit(): void {
    if(this.productForm.valid && this.selectedFile){
      const formData = new FormData();
      formData.append('file', this.selectedFile, this.selectedFile?.name);
      formData.append('name', this.productForm.get("name")!.value);
      formData.append('description', this.productForm.get("description")!.value);
      formData.append('price', this.productForm.get("price")!.value.toString());
      formData.append('brand', this.productForm.get("brand")!.value);
      formData.append('type', this.productForm.get("type")!.value);
      formData.append('quantityInStock', this.productForm.get("quantityInStock")!.value.toString());

      console.log(this.productForm.getRawValue())
      this.shopService.createProduct(formData).subscribe({
        next: p => console.log(p),
        error: err => console.log(err)
      })
    }
    else{
      this.productForm.markAllAsTouched();
    }
  }
  
  // private loadProduct(){
  //   const id = this.activatedRoute.snapshot.paramMap.get('id');
    
  //   if(id && +id > 0){
  //     this.productId = +id;
  //     this.shopService.getProduct(+id).subscribe({
  //       next : p => {
  //         this.product = p;
  //       },
  //       error : err => console.log(err)
  //     })
  //   }
  // }   
  
  onFileChange(event : Event){
    const input = event.target as HTMLInputElement; // Type casting the target
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }
}
