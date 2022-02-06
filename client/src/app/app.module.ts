import { Injectable, NgModule } from '@angular/core';
import {
  BrowserModule,
  HammerModule,
  HammerGestureConfig,
  HAMMER_GESTURE_CONFIG,
} from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import * as Hammer from 'hammerjs';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { MaterialModule } from './modules/material.module';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './authentication/login/login.component';
import { ErrorInterceptor } from './interceptors/error.interceptor';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { SearchComponent } from './product/search/search.component';
import { ProductFilterComponent } from './product/search/product-filter/product-filter.component';
import { RangeInputComponent } from './components/range-input/range-input.component';
import { ReversePipe } from './components/pipes/reverse.pipe';
import { ToIntArrayPipe } from './components/pipes/to-int-array.pipe';
import { MultiSelectComponent } from './components/multi-select/multi-select.component';
import { ProductDetailComponent } from './product/product-detail/product-detail.component';
import { GalleryComponent } from './components/gallery/gallery.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { RegisterComponent } from './authentication/register/register.component';
import { UserExistDirective } from './components/directives/user-exist.directive';
import { InputComponent } from './components/forms/input/input.component';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { ReducePipe } from './components/pipes/reduce.pipe';
import { ScrollContentComponent } from './components/scroll-content/scroll-content.component';
import { CartComponent } from './cart/cart.component';
import { ProductCheckoutComponent } from './order/product-checkout/product-checkout.component';
import { OrderPaymentComponent } from './order/order-payment/order-payment.component';
import { OrderDetailComponent } from './order/order-detail/order-detail.component';
import { OrderListComponent } from './order/order-list/order-list.component';

@Injectable()
export class HammerConfig extends HammerGestureConfig {
  overrides = {
    swipe: { direction: Hammer.DIRECTION_ALL },
  };
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    LoginComponent,
    SpinnerComponent,
    SearchComponent,
    ProductFilterComponent,
    RangeInputComponent,
    ReversePipe,
    ToIntArrayPipe,
    MultiSelectComponent,
    ProductDetailComponent,
    GalleryComponent,
    PaginationComponent,
    RegisterComponent,
    UserExistDirective,
    InputComponent,
    EditProfileComponent,
    ReducePipe,
    ScrollContentComponent,
    CartComponent,
    ProductCheckoutComponent,
    OrderPaymentComponent,
    OrderDetailComponent,
    OrderListComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HammerModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HAMMER_GESTURE_CONFIG, useClass: HammerConfig },
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
