import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './authentication/login/login.component';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';
import { SearchComponent } from './product/search/search.component';
import { RegisterComponent } from './authentication/register/register.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { CartComponent } from './cart/cart.component';
import { ProductCheckoutComponent } from './order/product-checkout/product-checkout.component';
import { OrderPaymentComponent } from './order/order-payment/order-payment.component';
import { OrderDetailComponent } from './order/order-detail/order-detail.component';
import { OrderListComponent } from './order/order-list/order-list.component';
import { WalletComponent } from './wallet/wallet.component';
import { AboutComponent } from './about/about.component';
import { TrackListComponent } from './track/track-list/track-list.component';
import { StoreOrderListComponent } from './store/store-order-list/store-order-list.component';
import { StoreOrderComponent } from './store/store-order/store-order.component';
import { TrackOrderComponent } from './track/track-order/track-order.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'about', component: AboutComponent },
  { path: 'search', component: SearchComponent },
  { path: 'product/:id', component: ProductDetailComponent },
  { path: 'edit-profile', component: EditProfileComponent },
  { path: 'order/:id', component: OrderDetailComponent },
  { path: 'order', component: OrderListComponent },
  { path: 'cart', component: CartComponent },
  { path: 'checkout', component: ProductCheckoutComponent },
  { path: 'order-payment', component: OrderPaymentComponent },
  { path: 'wallet', component: WalletComponent },
  { path: 'track/:id', component: TrackOrderComponent },
  { path: 'track', component: TrackListComponent },
  { path: 'store/order/:id', component: StoreOrderComponent },
  { path: 'store/order', component: StoreOrderListComponent },
  { path: '**', component: HomeComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
