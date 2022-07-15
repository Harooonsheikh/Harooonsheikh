import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthGuard } from './_shared';
import { fwcAPIInterceptor } from './interceptors/httpconfig.interceptor';
import { SidebarComponent } from './_shared/layouts/sidebar/sidebar.component';
import { HeaderComponent } from './_shared/layouts/header/header.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { LoaderComponent } from './_shared/layouts/loader/loader.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoaderService } from './_shared/services/loader.service';
import { http } from './_shared/services/http';
import { MenuComponent } from './_shared/layouts/menu/menu.component';
import { LoginComponent } from './components/login/login.component';
import { SaleOrderTransactionComponent } from './components/sale-order/sale-order-transaction/sale-order-transaction.component';
import { CustomerInformationComponent } from './components/sale-order/customer/customer-information/customer-information.component';
import { PurchaseCartComponent } from './components/sale-order/cart/purchase-cart/purchase-cart.component';
import { OrderDetailsComponent } from './components/sale-order/customer/order-details/order-details.component';
import { AddContactPersonComponent } from './components/sale-order/contact-person/add-contact-person/add-contact-person.component';
import { AccessDeniedComponent } from './_shared/errors/access-denied/access-denied.component';
import { NotFoundComponent } from './_shared/errors/not-found/not-found.component';
import { ServerErrorComponent } from './_shared/errors/server-error/server-error.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AgGridModule } from 'ag-grid-angular';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { jqxDropDownButtonComponent } from 'jqwidgets-scripts/jqwidgets-ts/angular_jqxdropdownbutton';
import { jqxGridComponent } from 'jqwidgets-scripts/jqwidgets-ts/angular_jqxgrid';
import { QuotationComponent } from './components/quotation/quotation.component';
import { NgxMaskInputModule } from 'ngx-mask-input';

import { DatepickerModule, BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BehaviourSubjectService} from './_shared/services/behaviour-subject-service'; 
 
@NgModule({
    declarations: [
        AppComponent,
        MenuComponent,
        LoginComponent,
        SidebarComponent,
        HeaderComponent,
        LoaderComponent,
        SaleOrderTransactionComponent,
        CustomerInformationComponent,
        PurchaseCartComponent,
        OrderDetailsComponent,
        AddContactPersonComponent,
        AccessDeniedComponent,
        NotFoundComponent,
        ServerErrorComponent,
        QuotationComponent,
        jqxDropDownButtonComponent,
        jqxGridComponent
    ],
    imports: [
        CommonModule,
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        AppRoutingModule,
        NgbDropdownModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot(),
        MatProgressSpinnerModule,
        AgGridModule.withComponents([PurchaseCartComponent]),
        FormsModule,
        ReactiveFormsModule,
        NgxSpinnerModule,
        NgbModalModule,
        NgbModule,
        NgMultiSelectDropDownModule.forRoot(),
        NgxMaskInputModule,
        BsDatepickerModule.forRoot(),
        DatepickerModule.forRoot(),
        
        
    ],
    entryComponents: [],
    providers: [
        http,
        LoaderService,
        BehaviourSubjectService,
        AuthGuard,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: fwcAPIInterceptor,
            multi: true 
        }
    ],
    bootstrap: [AppComponent],
    exports:[
        jqxDropDownButtonComponent,
        jqxGridComponent
    ]
})
export class AppModule { }
