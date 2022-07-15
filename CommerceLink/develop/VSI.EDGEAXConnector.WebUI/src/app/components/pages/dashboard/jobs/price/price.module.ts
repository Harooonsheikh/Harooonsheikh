import { PriceBookComponent } from "./pricetable/pricebook.component";
import { PriceComponent } from "./price.component";
import { CLControlModule } from "./../../../../../controls/clcontrol.module";
import { DefaultComponent } from "./../../../default.component";
import { LayoutModule } from "./../../../../layouts/layout.module";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: PriceComponent
            }
        ]
    }
];

@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule,
        CLControlModule
    ],
    exports: [RouterModule],
    declarations: [PriceComponent, PriceBookComponent]
})
export class PriceModule { }
