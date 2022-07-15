import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { HeaderProfileComponent } from "./header-profile.component";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { FormsModule } from "@angular/forms";

//import { UserService } from '../../../../_services/user.service';
const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "",
                component: HeaderProfileComponent
            }
        ]
    }
];
@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        RouterModule.forChild(routes),
        LayoutModule
    ],
    exports: [RouterModule],
    declarations: [HeaderProfileComponent],
    providers: [
        //  UserService
    ]
})
export class HeaderProfileModule { }
