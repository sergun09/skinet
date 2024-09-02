import { DeliveryMethod } from '../../core/services/deliveryMethod';
export interface Order {
    id: number
    orderDate: string
    buyerEmail: string
    shippingAddress: ShippingAddress
    deliveryMethod: string
    shippingPrice: number
    paymentSummary: PaymentSummary
    orderItems: OrderItem[]
    subTotal: number
    total: number
    orderStatus: string
    paymentIntentId: string
  }
  
  export interface ShippingAddress {
    name: string
    line1: string
    line2?: string
    city: string
    state: string
    country: string
    postalCode: string
  }
  
  export interface PaymentSummary {
    last4: number
    brand: string
    expMonth: number
    expYear: number
  }
  
  export interface OrderItem {
    productId: number
    productName: string
    pictureUrl: string
    price: number
    quantity: number
  }

  export interface OrderToCreate{
    cartId: string,
    deliveryMethodId: number,
    shippingAddress: ShippingAddress,
    paymentSummary: PaymentSummary
  }
  