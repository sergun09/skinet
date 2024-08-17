import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';

@Pipe({
  name: 'payment',
  standalone: true
})
export class PaymentPipe implements PipeTransform {

  transform(value?: ConfirmationToken["payment_method_preview"], ...args: unknown[]): unknown {
    if(value?.card){
      const {last4,exp_year,exp_month} = value.card;
      return `${'Numéro de la carte :**** **** ****' + last4}, ${'Date de validité : ' +exp_month}'/'${exp_year}`
    }
    return null;
  }

}
