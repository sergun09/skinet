import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { User, Address } from '../../shared/models/user';
import { map, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null)
  
  constructor(private http: HttpClient) { }

  login(infos : any){
    let params = new HttpParams();
    params = params.append('useCookies',true);
    return this.http.post<User>(this.baseUrl + 'login', infos, {params});
  }

  register(infos : any){
    return this.http.post(this.baseUrl + 'accounts/register', infos);
  }

  getUserInfo(){
    return this.http.get<User>(this.baseUrl + 'accounts/user-info').pipe(
      map(user => {
        this.currentUser.set(user);
        return user;
      })
    );
  }

  logout(){
    return this.http.post(this.baseUrl + 'accounts/logout', {});
  }

  getAuthState(){
    return this.http.get<{isAuthenticated: boolean}>(this.baseUrl + "accounts/auth-status");
  }

  updateAddress(address: Address){
    return this.http.post(this.baseUrl + 'accounts/address', address).pipe(
      tap(() => {
        this.currentUser.update(user => {
          if(user) user.address = address;
          return user;
        });
      })
    );
  }
}
