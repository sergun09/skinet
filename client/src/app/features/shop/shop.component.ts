import { Component, inject, OnInit } from '@angular/core';
import { Product } from '../../shared/models/Product';
import { ShopService } from '../../core/services/shop.service';
import { MatCard } from '@angular/material/card';
import { ProductItemComponent } from "./product-item/product-item.component";
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { ShopParams } from '../../shared/models/shopParams';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Pagination } from '../../shared/models/Pagination';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [MatCard, ProductItemComponent,MatButton,MatIcon,MatMenu,MatSelectionList,MatListOption,MatMenuTrigger,MatPaginator,FormsModule],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{

  products?: Pagination<Product>;
  pageSizeOptions = [5,10,15,20];
  private dialogService = inject(MatDialog)
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Prix croissant', value: 'priceAsc'},
    {name: 'Prix décroissant', value: 'priceDesc'},
  ]
  shopParams = new ShopParams();

  constructor(private shopService: ShopService){}
  
  ngOnInit(): void {
    this.initShop();
  }

  initShop(){
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  }

  getProducts()
  {
    this.shopService.getProducts(this.shopParams).subscribe({
      next : response => this.products = response,
      error : err => console.log(err)
    });
  }

  openFiltersDialog()
  {
    const dialogRef = this.dialogService.open(FiltersDialogComponent,{
      minWidth: 500,
      data:{
        selectedBrands: this.shopParams.brands,
        selectedTypes: this.shopParams.types
      }
    });
    dialogRef.afterClosed().subscribe({
      next: result => {
        if(result){
          console.log(result);
            this.shopParams.brands = result.selectedBrands;
            this.shopParams.types = result.selectedTypes;
            this.shopParams.pageIndex = 1;
            this.getProducts();
        }
      }
    });
  }

  onSortChange(event : MatSelectionListChange) {
    const selectedOption = event.options[0];
    if(selectedOption)
      {
        this.shopParams.sort = selectedOption.value;
        this.getProducts();
      }
  }

  handlePageEvent(event: PageEvent) {
    this.shopParams.pageIndex = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }

  onSearchChanged(){
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }
}
