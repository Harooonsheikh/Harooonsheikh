import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { CLControlModule } from "../../../../controls/clcontrol.module";
import { FormsModule } from "@angular/forms";
import { } from "./";
import { TemplateComponent } from "./Template/Template.component";
import { SubscriberComponent } from "./Subscriber/subscriber.component";
import { EmailComponent } from "./email.component";
import { AceEditorModule } from 'ng2-ace-editor';
const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "subscriber",
                component: SubscriberComponent
            },
            {
                path: "template",
                component: TemplateComponent
            },
            {
                path: "",
                component: EmailComponent
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
        AceEditorModule
    ],
    exports: [RouterModule],
    declarations: [TemplateComponent, EmailComponent, SubscriberComponent]
})
export class EmailModule { }
