import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { CLControlModule } from "../../../../controls/clcontrol.module";

import { FormsModule } from "@angular/forms";

import { SettingComponent } from "./Setting/Setting.component";
import { } from "./";
import { ApplicationSettingsComponent } from "./applicationsetting.component";
import { NgxPasswordToggleModule } from 'ngx-password-toggle';
const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "setting",
                component: SettingComponent
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
        CLControlModule,
        NgxPasswordToggleModule
    ],
    exports: [RouterModule],
    declarations: [ApplicationSettingsComponent, SettingComponent]
})
export class ApplicationSettingModule { }
