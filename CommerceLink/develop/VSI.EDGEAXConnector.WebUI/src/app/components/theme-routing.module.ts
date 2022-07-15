import { NgModule } from "@angular/core";
import { ThemeComponent } from "./theme.component";
import { Routes, RouterModule } from "@angular/router";
import { AuthGuard } from "../auth/_guards/auth.guard";

const routes: Routes = [
    {
        path: "",
        component: ThemeComponent,
        canActivate: [AuthGuard],
        children: [
            {
                "path": "merchandise",
                "loadChildren": ".\/pages\/dashboard\/merchandise\/merchandise.module#MerchandiseModule"
            }, {
                "path": "saleorder",
                "loadChildren": ".\/pages\/dashboard\/salesorder\/saleorder.module#SaleOrderModule"
            }, {
                "path": "logs",
                "loadChildren": ".\/pages\/dashboard\/logs\/logs.module#LogModule"
            },
            {
                "path": "jobs",
                "loadChildren": ".\/pages\/dashboard\/jobs\/jobs.module#JobsModule"
            },
            {
                "path": "storeconfig",
                "loadChildren": ".\/pages\/administration\/storeconfig\/storeconfiguration.module#StoreConfigurationModule"
            },
            {
                "path": "ApplicationSettings",
                "loadChildren": ".\/pages\/administration\/ApplicationSetting\/ApplicationSetting.module#ApplicationSettingModule"
            }, {
                "path": "mapping",
                "loadChildren": ".\/pages\/administration\/mapping\/mapping.module#MappingModule"
            },
            {
                "path": "emailconfig",
                "loadChildren": ".\/pages\/administration\/Email\/email.module#EmailModule"
            },
            {
                "path": "jobsconfig",
                "loadChildren": ".\/pages\/administration\/JobsConfig\/jobsconfig.module#jobsconfigModule"
            },
            {
                "path": "paymentmethod",
                "loadChildren": ".\/pages\/administration\/paymentmethod\/paymentmethod.module#PaymentMethodModule"
            },
            {
                "path": "configobjects",
                "loadChildren": ".\/pages\/administration\/ConfigObjects\/configobjects.module#ConfigObjectModule"
            },
            // {
            //     "path": "storeactions",
            //     "loadChildren": ".\/pages\/storeconfig\/storeaction.module#StoreActionModule"
            // },
            {
                "path": "dimension",
                "loadChildren": ".\/pages\/administration\/dimensionset\/dimensionset.module#DimensionSetModule"
            },
            {
                "path": "header\/actions",
                "loadChildren": ".\/pages\/header\/header-actions\/header-actions.module#HeaderActionsModule"
            },
            {
                "path": "header\/profile",
                "loadChildren": ".\/pages\/header\/header-profile\/header-profile.module#HeaderProfileModule"
            },
            {
                "path": "applogs",
                "loadChildren": ".\/pages\/administration\/applogs\/applogs.module#AppLogsModule"
            },
            {
                "path": "apilogs",
                "loadChildren": ".\/pages\/administration\/apilogs\/apilogs.module#ApiLogsModule"
            },
            {
                "path": "404",
                "loadChildren": ".\/pages\/not-found\/not-found\/not-found.module#NotFoundModule"
            },
            {
                "path": "",
                "redirectTo": "ApplicationSettings",
                "pathMatch": "full"
            },
            {
                "path": "index",
                "loadChildren": ".\/pages\/index\/index.module#IndexModule"
            }
        ]
    },

    {
        path: "**",
        redirectTo: "404",
        pathMatch: "full"
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ThemeRoutingModule { }
