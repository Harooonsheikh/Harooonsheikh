import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { CLControlModule } from "../../../../controls/clcontrol.module";
import { FormsModule } from "@angular/forms";
import { } from "./";
import { ConfigObjectsComponent } from "./configobjects.component";

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: ConfigObjectsComponent
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
    declarations: [ConfigObjectsComponent]
})
export class ConfigObjectModule { }
