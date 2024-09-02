import { Pipe, PipeTransform } from '@angular/core';
import { ConfirmationToken } from '@stripe/stripe-js';
import { PaymentSummary } from '../models/order';

@Pipe({
  name: 'payment',
  standalone: true
})
export class PaymentPipe implements PipeTransform {

  transform(value?: ConfirmationToken["payment_method_preview"] | PaymentSummary, ...args: unknown[]): unknown {
    if(value && 'card' in value){
      const {last4,exp_year,exp_month} = (value as ConfirmationToken["payment_method_preview"]).card!;
      return `${'Numéro de la carte :**** **** ****' + last4}, ${'Date de validité : ' +exp_month}'/'${exp_year}`
    }
    else if(value && 'last4' in value){
      const {last4,expYear,expMonth} = value as PaymentSummary;
      return `${'Numéro de la carte :**** **** ****' + last4}, ${'Date de validité : ' +expMonth}'/'${expYear}`
    }
    return null;
  }

}
