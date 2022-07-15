import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { CLControlModule } from "../../../../controls/clcontrol.module";
import { FormsModule } from "@angular/forms";
import { } from "./";
import { PaymentMethodComponent } from "./paymentmethod.component";
import { PaymentMethodTableComponent } from "./paymentmethodtable/paymentmethodtable.component";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: PaymentMethodComponent
            }
        ]
    }
];
@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule,
        FormsModule,
        CLControlModule
    ],
    exports: [RouterModule],
    declarations: [PaymentMethodComponent, PaymentMethodTableComponent]
})
export class PaymentMethodModule { }
