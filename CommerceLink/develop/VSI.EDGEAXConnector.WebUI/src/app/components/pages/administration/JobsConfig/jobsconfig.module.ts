import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Routes, RouterModule } from "@angular/router";
import { LayoutModule } from "../../../layouts/layout.module";
import { DefaultComponent } from "../../default.component";
import { CLControlModule } from "../../../../controls/clcontrol.module";
import { FormsModule } from "@angular/forms";
import { } from "./";
import { CLJobsComponent } from "./CLJobs/cljobs.component";
import { JobsConfigComponent } from "./jobsconfig.component";
import { FileSyncJobsComponent } from "./FileSyncJobs/filesyncjobs.component";
import { NKDatetimeModule } from 'ng2-datetime/ng2-datetime';
const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        children: [
            {
                path: "cljobs",
                component: CLJobsComponent
            },
            {
                path: "filesyncjobs",
                component: FileSyncJobsComponent
            },
            {
                path: "",
                component: JobsConfigComponent
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
        NKDatetimeModule
    ],
    exports: [RouterModule],
    declarations: [CLJobsComponent, JobsConfigComponent, FileSyncJobsComponent]
})
export class jobsconfigModule { }
