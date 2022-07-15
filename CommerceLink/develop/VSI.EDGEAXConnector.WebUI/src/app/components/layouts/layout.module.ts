import { DiscountService } from "./../../_services/discount.service";
import { PriceService } from "./../../_services/price.service";
import { InventoryService } from "./../../_services/inventory.service";
import { CatalogService } from "./../../_services/catalog.service";
import { LogsService } from "./../../_services/log.service";
import { NgModule } from "@angular/core";
import { LayoutComponent } from "./layout/layout.component";
import { HeaderNavComponent } from "./header-nav/header-nav.component";
import { DefaultComponent } from "../pages/default.component";
import { AsideNavComponent } from "./aside-nav/aside-nav.component";
import { NavBarComponent } from "./nav-bar/nav-bar.component";
import { FooterComponent } from "./footer/footer.component";
import { QuickSidebarComponent } from "./quick-sidebar/quick-sidebar.component";
import { ScrollTopComponent } from "./scroll-top/scroll-top.component";
// import { TooltipsComponent } from './tooltips/tooltips.component';
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { HrefPreventDefaultDirective } from "../../_directives/href-prevent-default.directive";
import { UnwrapTagDirective } from "../../_directives/unwrap-tag.directive";
import { UserService } from "../../_services/user.service";
import { WorkFlowService } from "../../_services/workflow.service";
import { EntityService } from "../../_services/entity.service";
import { StoreService } from "../../_services/store.service";
import { SaleOrderService } from "../../_services/saleorder.service";
import { AppSettingService } from "../../_services/appsettings.service";
import { PaymentMethodService } from "../../_services/paymentmethod.service";
import { ConfigObjectService } from "../../_services/ConfigObjects.service";
import { EmailSettingsService } from "../../_services/emailsettings.service";
import { ScreenNamesService } from "../../_services/screennames.service";
import { MappingService } from "../../_services/mapping.service";
import { JobsService } from "../../_services/jobs.service";
import { DataFileService } from "../../_services/datafiles.service";
import { ActionService } from "../../_services/storeaction.service";
import { EcomTypeService } from "../../_services/ecomtype.service";
import { DimensionSetService } from "../../_services/dimensionset.service";
import { ApiLogsService } from "../../_services/apilog.service";

@NgModule({
    declarations: [
        LayoutComponent,
        HeaderNavComponent,
        DefaultComponent,
        AsideNavComponent,
        NavBarComponent,

        FooterComponent,
        QuickSidebarComponent,
        ScrollTopComponent,
        // TooltipsComponent,
        HrefPreventDefaultDirective,
        UnwrapTagDirective
    ],
    exports: [
        LayoutComponent,
        HeaderNavComponent,
        DefaultComponent,
        AsideNavComponent,
        NavBarComponent,
        FooterComponent,
        QuickSidebarComponent,
        ScrollTopComponent,
        // TooltipsComponent,
        HrefPreventDefaultDirective
    ],
    imports: [CommonModule, RouterModule],
    providers: [
        UserService,
        WorkFlowService,
        LogsService,
        CatalogService,
        AppSettingService,
        InventoryService,
        PriceService,
        DiscountService,
        EntityService,
        StoreService,
        SaleOrderService,
        PaymentMethodService,
        ConfigObjectService,
        EmailSettingsService,
        JobsService,
        ConfigObjectService,
        ScreenNamesService,
        DataFileService,
        MappingService,
        ActionService,
        EcomTypeService,
        DimensionSetService,
        ApiLogsService
    ]
})
export class LayoutModule { }
