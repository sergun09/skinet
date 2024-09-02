import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { SnackbarService } from '../services/snackbar.service';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  const snack = inject(SnackbarService);

  if(!accountService.isAdmin()){
    snack.error("Vous ne pouvez pas accéder à cette ressource !")
    router.navigateByUrl("/shop");
    return false;
  }
  return true;
};
