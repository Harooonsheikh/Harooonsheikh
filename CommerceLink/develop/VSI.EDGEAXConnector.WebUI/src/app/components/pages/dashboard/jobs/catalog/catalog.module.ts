import { CLControlModule } from "./../../../../../controls/clcontrol.module";
import { CatalogCatAssignComponent } from "./catassign/catassign.component";
import { CatalogCategoryComponent } from "./category/category.component";
import { CatalogProductComponent } from "./products/product.component";
import { DefaultComponent } from "./../../../default.component";
import { LayoutModule } from "./../../../../layouts/layout.module";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { CatalogComponent } from "./catalog.component";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: CatalogComponent
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
    exports: [RouterModule, CatalogComponent],
    declarations: [
        CatalogComponent,
        CatalogProductComponent,
        CatalogCategoryComponent,
        CatalogCatAssignComponent
    ]
})
export class CatalogModule { }
