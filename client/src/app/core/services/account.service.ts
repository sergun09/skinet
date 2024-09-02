import { HttpClient, HttpParams } from '@angular/common/http';
import { computed, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User, Address } from '../../shared/models/user';
import { map, tap } from 'rxjs';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null)
  isAdmin = computed(() => {
    const roles = this.currentUser()?.roles;
    return Array.isArray(roles) ? roles.includes("Admin") : roles === 'Admin';
  });
  
  constructor(private http: HttpClient, private signalrService: SignalrService) { }

  login(infos : any){
    let params = new HttpParams();
    params = params.append('useCookies',true);
    return this.http.post<User>(this.baseUrl + 'login', infos, {params}).pipe(
      tap(() => this.signalrService.createHubConnection())
    );
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
    return this.http.post(this.baseUrl + 'accounts/logout', {}).pipe(
      tap(() => this.signalrService.stopHubConnection())
    );
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
