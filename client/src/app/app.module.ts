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
import { WalletComponent } from './wallet/wallet.component';
import { AboutComponent } from './about/about.component';
import { ScrollToTopComponent } from './components/scroll-to-top/scroll-to-top.component';
import { TrackListComponent } from './track/track-list/track-list.component';
import { FilterListComponent } from './components/filter-list/filter-list.component';
import { SpaceBetweenPipe } from './components/pipes/space-between.pipe';
import { SingleSelectComponent } from './components/single-select/single-select.component';
import { SelectLocationComponent } from './components/select-location/select-location.component';
import { StoreOrderListComponent } from './store/store-order-list/store-order-list.component';
import { AddItemsComponent } from './components/add-items/add-items.component';
import { StoreOrderComponent } from './store/store-order/store-order.component';
import { TrackOrderComponent } from './track/track-order/track-order.component';
import { AdminRolesComponent } from './roles/admin/admin-roles/admin-roles.component';
import { GetUserDirective } from './components/directives/get-user.directive';
import { AddAdminRoleComponent } from './roles/admin/add-admin-role/add-admin-role.component';
import { AddStoreRoleComponent } from './roles/store/add-store-role/add-store-role.component';
import { StoreAdminAddComponent } from './roles/store/store-admin-add/store-admin-add.component';
import { StoreAdminComponent } from './roles/store/store-admin/store-admin.component';
import { StoreModeratorAddComponent } from './roles/store/store-moderator-add/store-moderator-add.component';
import { StoreModeratorComponent } from './roles/store/store-moderator/store-moderator.component';
import { StoreRoleComponent } from './roles/store/store-role/store-role.component';
import { AddTrackRoleComponent } from './roles/track/add-track-role/add-track-role.component';
import { TrackAdminAddComponent } from './roles/track/track-admin-add/track-admin-add.component';
import { TrackAdminComponent } from './roles/track/track-admin/track-admin.component';
import { TrackModeratorAddComponent } from './roles/track/track-moderator-add/track-moderator-add.component';
import { TrackModeratorComponent } from './roles/track/track-moderator/track-moderator.component';
import { TrackRolesComponent } from './roles/track/track-roles/track-roles.component';
import { PageNotFoundComponent } from './error/page-not-found/page-not-found.component';
import { InRoleDirective } from './components/directives/in-role.directive';
import { ServerErrorComponent } from './error/server-error/server-error.component';

@Injectable()
export class HammerConfig extends HammerGestureConfig {
  overrides = {
    swipe: { direction: Hammer.DIRECTION_HORIZONTAL },
    pinch: { enable: false },
    rotate: { enable: false },
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
    WalletComponent,
    AboutComponent,
    ScrollToTopComponent,
    TrackListComponent,
    FilterListComponent,
    SpaceBetweenPipe,
    SingleSelectComponent,
    SelectLocationComponent,
    StoreOrderListComponent,
    AddItemsComponent,
    StoreOrderComponent,
    TrackOrderComponent,
    StoreAdminComponent,
    TrackAdminComponent,
    TrackRolesComponent,
    TrackModeratorComponent,
    StoreModeratorComponent,
    StoreRoleComponent,
    AdminRolesComponent,
    AddTrackRoleComponent,
    GetUserDirective,
    TrackAdminAddComponent,
    TrackModeratorAddComponent,
    AddStoreRoleComponent,
    StoreAdminAddComponent,
    StoreModeratorAddComponent,
    AddAdminRoleComponent,
    PageNotFoundComponent,
    InRoleDirective,
    ServerErrorComponent,
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
