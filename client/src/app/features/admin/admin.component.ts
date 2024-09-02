import { AfterViewInit, Component, inject, OnInit, ViewChild } from '@angular/core';
import {MatTableDataSource,MatTableModule} from '@angular/material/table';
import { Order } from '../../shared/models/order';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { AdminService } from '../../core/services/admin.service';
import { OrderParams } from '../../shared/models/orderParams';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatLabel, MatSelectChange, MatSelectModule } from '@angular/material/select';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { RouterLink } from '@angular/router';
import { DialogService } from '../../core/services/dialog.service';
import { ShopService } from '../../core/services/shop.service';
import { ShopParams } from '../../shared/models/shopParams';
import { Product } from '../../shared/models/Product';
import { SnackbarService } from '../../core/services/snackbar.service';


@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [MatTableModule,MatPaginator,MatButton,MatIcon,MatSelectModule, DatePipe,CurrencyPipe,MatLabel,MatTooltipModule,MatTabsModule,RouterLink],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.scss'
})
export class AdminComponent implements OnInit{
  
  displayedColumns: string[] = ['Id', "Email de l'acheteur", 'Date Commande', 'Total','Statut', 'Actions'];
  displayedColumnsProduct: string[] = ['Id', "Nom", 'Prix', 'Type','Marque', 'Quantité', 'Actions'];
  dataSource = new MatTableDataSource<Order>([]);
  dataSourceProduct = new MatTableDataSource<Product>([]);
  private adminService = inject(AdminService);
  private shopService = inject(ShopService);
  private dialogService = inject(DialogService);
  private snackBarService = inject(SnackbarService);
  orderParams = new OrderParams();
  shopParams = new ShopParams();
  totalItems = 0;
  totalProductItems = 0;
  statusOptions = ["All", "Paiement Reçu", "Rembourser", "En cours", "Problème paiement"]
  
  @ViewChild(MatPaginator) paginator!: MatPaginator 
  
  ngOnInit(): void {
    this.loadOrders();
    this.loadProducts();
  }

  loadOrders(){
    this.adminService.getOrders(this.orderParams).subscribe({
      next : response => {
        if(response.data){
          this.dataSource.data = response.data;
          this.totalItems = response.count;
        }
      }
    });
  }

  loadProducts(){
    this.shopService.getProducts(this.shopParams).subscribe({
      next : response => {
        if(response.data){
          this.dataSourceProduct.data = response.data;
          this.totalProductItems = response.count;
        }
      }
    });
  }

  deleteProduct(id : number){
    this.shopService.deleteProduct(id).subscribe({
      next : () => this.snackBarService.success("Le produit a été supprimé !")
    })
  }

  onPageChange(event: PageEvent){
    this.orderParams.pageNumber = event.pageIndex + 1;
    this.orderParams.pageSize = event.pageSize;
    this.loadOrders();
  }

  onFilterSelect(event: MatSelectChange){
    this.orderParams.filter = event.value;
    this.orderParams.pageNumber = 1;
    this.loadOrders();
  }

  onPageChangeProduct(event: PageEvent){
    this.shopParams.pageIndex = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.loadProducts();
  }



  async openConfirmDialog(id: number){
    const confirmed = await this.dialogService.confirm(
      "Confirmer le remboursement ?",
      "Cette action est irréversible"
    ).then(() => {
      this.refundOrder(id)
    });
  }

  async openConfirmDialogProductDelete(id: number){
    const confirmed = await this.dialogService.confirm(
      "Confirmer la suppression du produit ?",
      "Cette action est irréversible"
    ).then(() => {
      this.deleteProduct(id);
      this.loadProducts();
    });
  }

  refundOrder(id: number){
    this.adminService.refundOrder(id).subscribe({
      next: order => {
        this.dataSource.data = this.dataSource.data.map(o => o.id === id ? order : o);
      }
    })
  }
}
