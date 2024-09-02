import { Component } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [MatButton],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss'
})
export class TestErrorComponent {
  baseUrl = environment.apiUrl;
  validationErrors?: string[]

  constructor(private httpClient: HttpClient){}

  get400(){
    this.httpClient.get(this.baseUrl + "errors/badrequest").subscribe({
      error: err => console.log(err)
    });
  }
  
  get400ValidationError(){
    this.httpClient.post(this.baseUrl + "errors/validationerror", {}).subscribe({
      error: err => this.validationErrors = err
    });
  }

  get401(){
    this.httpClient.get(this.baseUrl + "errors/unauthorized").subscribe({
      error: err => console.log(err)
    });
  }
  
  get404(){
    this.httpClient.get(this.baseUrl + "errors/notfound").subscribe({
      error: err => console.log(err)
    });
  }

  get500(){
    this.httpClient.get(this.baseUrl + "errors/internalerror", {}).subscribe({
      error: err => console.log(err)
    });
  }
}
