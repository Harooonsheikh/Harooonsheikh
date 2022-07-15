import { KeyValue, CatalogModel } from "./../../../../../../Entities/Common";
import { Observable } from "rxjs";
import { WorkFlow } from "./../../../../../../Entities/WorkFlow";
import { WorkFlowService } from "./../../../../../../_services/workflow.service";
import { CatalogService } from "./../../../../../../_services/catalog.service";
import { Component, OnInit, AfterViewInit, Input } from "@angular/core";
declare var jquery: any;
declare var $: any;

export class Product {
    public Name: string = "";
    public ProdId: string = "";
    public longDescription: string = "";
    public shortDescription: string = "";
    public taxClass: string = "";
    public sku: string = "";
    public variations: Array<KeyValue<string>> = null;
    public customAttribute: Array<KeyValue<string>> = null;
    public variants: Array<string> = null;
    constructor() {
        this.variations = new Array<KeyValue<string>>();
        this.customAttribute = new Array<KeyValue<string>>();
        this.variants = new Array<string>();
    }
}

@Component({
    selector: "catalog-product",
    templateUrl: "product.component.html"
})
export class CatalogProductComponent implements OnInit, AfterViewInit {
    @Input() public inputFile: string = "";
    public totalProducts: number = null;
    private catProductsOb: Observable<Object> = null;
    public catProducts: Array<Product> = null;
    public dataTable: any = null;
    public searchQuery: string = "";
    public searchResults: Array<Product> = null;
    public selectedProduct: Product = null;
    public selectedProductColor: string = "";
    public selectedProductSize: string = "";
    public selectedProductStyle: string = "";

    constructor(
        private _cService: CatalogService,
        private _wfService: WorkFlowService
    ) {
        this.selectedProduct = new Product();
    }

    ngOnInit() { }
    ngAfterViewInit() {

        this._cService.getCatalogModalCount(this.inputFile, CatalogModel.Product).subscribe(
            (number: number) => {
                this.totalProducts = number;
                this.setupProductTable();
            },
            (error: Response) => this.handleError(error)
        );
    }

    private setupProductTable(): void {

        this.catProductsOb = this._cService.getProductByCatalog(
            this.inputFile,
            0 * 10,
            10
        );
        this.catProducts = new Array<Product>();
        for (var i = 0; i < this.totalProducts; i++) {
            var element = new Product();
            this.catProducts.push(element);
        }

        this.dataTable = $("#productLog").mDatatable({
            data: {
                type: "local",
                source: this.catProducts,
                pageSize: 10
            },
            layout: {
                theme: "default",
                class: "",
                scroll: !1,
                height: 450,
                footer: !1
            },
            pagination: true,
            filterable: !1,
            toolbar: {
                items: {
                    pagination: {
                        navigation: {
                            prev: true,
                            next: true,
                            first: true,
                            last: true
                        },
                        pages: {
                            desktop: {
                                layout: "default",
                                pagesNumber: 10
                            }
                        }
                    }
                }
            },
            columns: [
                {
                    field: "ProdId",
                    title: "ProductId",
                    selector: !1,
                    width: "100",
                    textAlign: "center"
                },
                {
                    field: "Name",
                    title: "Name",
                    textAlign: "center"
                },
                {
                    field: "sku",
                    title: "SKU",
                    width: "100",
                    textAlign: "center"
                },
                {
                    field: "variants",
                    title: "Type",
                    width: "70",
                    textAlign: "center",
                    template: function(e: any) {
                        var tv = e.getDatatable().jsonData[e.rowIndex].variants.length;
                        if (tv > 0) {
                            return "Master (" + tv + ")";
                        }
                        return "variant";
                    }
                },
                {
                    field: "customAttribute",
                    title: "Size",
                    width: "30",
                    textAlign: "center",
                    template: function(e: any) {
                        if (e.customAttribute.filter(m => m.Key == "size").length > 0) {
                            return e.customAttribute.filter(m => m.Key == "size")[0].Value;
                        }
                        return "";
                    }
                },
                {
                    field: "customAttributeColor",
                    title: "Color",
                    width: "100",
                    textAlign: "center",
                    template: function(e: any) {
                        if (e.customAttribute.filter(m => m.Key == "color").length > 0) {
                            return e.customAttribute.filter(m => m.Key == "color")[0].Value;
                        }
                        return "";
                    }
                },
                {
                    field: "customAttributestyle",
                    title: "Style",
                    width: "100",
                    textAlign: "center",
                    template: function(e: any) {
                        if (e.customAttribute.filter(m => m.Key == "style").length > 0) {
                            return e.customAttribute.filter(m => m.Key == "style")[0].Value;
                        }
                        return "";
                    }
                },
                {
                    field: "actions",
                    title: "Actions",
                    width: "70",
                    template: function(e: any) {

                        let html: string = "";
                        html =
                            "<a href='#' data-toggle='modal' data-target='#m_modal_1_2' class='m-portlet__nav-link btn m-btn m-btn--hover-accent m-btn--icon m-btn--icon-only m-btn--pill' title='View'><i class='flaticon-list-3'></i></a>";
                        return html;
                    }
                }
            ]
        });

        this.initEvents();
    }

    private initEvents(): void {
        let pagi: any = this.dataTable.getDataSourceParam("pagination");
        pagi.page = 1;
        pagi.perpage = 10;
        this.dataTable.setDataSourceParam("pagination", pagi);

        var pages = this.dataTable.getCurrentPage();
        if (pages - 1 < 0) {
            pages = 0;
        } else {
            pages = pages - 1;
        }
        //var perPage = this.dataTable.getPageSize();
        var perPage = 10;
        //this._cService.getProductByCatalog(this.selectedCatalogFile, pages * perPage, perPage)
        this.catProductsOb.subscribe(
            (m: string) => {
                self.reloadTable.apply(self, [m, pages, perPage]);
            },
            (err: Response) => {
                self.handleError(err);
            }
        );

        let self: CatalogProductComponent = this;
        (function(component: any) {
            $("#productLog").on("click", "tr > td:nth-child(8) a", function() {
                var data = $(this)
                    .closest("tr")
                    .find("td:nth-child(1) span")
                    .first()
                    .html();
                component.getProductDetail(data);
                return false;
            });
        })(self);

        self.dataTable.on("m-datatable--on-goto-page", function(
            e: any,
            settings: any
        ) {
            var pages = settings.page;
            if (pages - 1 < 0) {
                pages = 0;
            } else {
                pages = pages - 1;
            }
            var perPage = settings.perpage;

            if (self.searchQuery == "") {
                self._cService
                    .getProductByCatalog(self.inputFile, pages * perPage, perPage)
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._cService
                    .getSearchResult(
                    self.inputFile,
                    self.searchQuery,
                    1,
                    pages * perPage,
                    perPage
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            }
        });

        self.dataTable.on("m-datatable--on-update-perpage", function(
            e: any,
            settings: any
        ) {
            var pages = settings.page;
            if (pages - 1 < 0) {
                pages = 0;
            } else {
                pages = pages - 1;
            }
            var perPage = settings.perpage;


            if (self.searchQuery == "") {
                self._cService
                    .getProductByCatalog(self.inputFile, pages * perPage, perPage)
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._cService
                    .getSearchResult(
                    self.inputFile,
                    self.searchQuery,
                    1,
                    pages * perPage,
                    perPage
                    )
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            }
        });

        self.dataTable.on("m-datatable--on-reloaded", function(
            e: any,
            settings: any
        ) {
            setTimeout(function() {
                $("#productLog")
                    .find("tbody > tr")
                    .each(function(a, b) {
                        if ($(b).find("td:nth-child(1) span").length > 0) {
                            if (
                                $(b)
                                    .find("td:nth-child(1) span")
                                    .first()
                                    .html() == ""
                            ) {

                                $(b).hide();
                            }
                        } else {
                            if (a != 0) {

                                $(b).hide();
                            }
                        }
                    });
            }, 50);
        });
    }

    public searchData(event: any): void {
        if (event.which == 13 || event.which == 1) {
            if (this.searchQuery != "") {
                this._cService
                    .getSearchResultCount(this.inputFile, this.searchQuery, 1)
                    .subscribe(
                    (res: string) => {
                        let numb: number = Number.parseInt(res);
                        this.searchResults = new Array<Product>();
                        for (var n = 0; n < numb; n++) {
                            let prod: Product = new Product();
                            this.searchResults.push(prod);
                        }

                        var pages = 0;
                        var perPage = this.dataTable.getPageSize();

                        this._cService
                            .getSearchResult(
                            this.inputFile,
                            this.searchQuery,
                            1,
                            pages * perPage,
                            perPage
                            )
                            .subscribe(
                            (m: string) => {
                                let pagi: any = this.dataTable.getDataSourceParam(
                                    "pagination"
                                );
                                pagi.page = 1;
                                pagi.perpage = 10;


                                this.dataTable.setDataSourceParam("pagination", pagi);

                                this.dataTable.load();
                                this.reloadTable.apply(this, [m, pages, perPage]);
                            },
                            (err: Response) => {
                                this.handleError(err);
                            }
                            );
                    },
                    (err: Response) => { }
                    );
            } else {
                this.dataTable.jsonData = this.catProducts; // Object.assign({},this.catProducts);
                this.dataTable.fullJsonData = this.catProducts;
                let pagi: any = this.dataTable.getDataSourceParam("pagination");
                pagi.page = 1;
                pagi.perpage = 10;
                this.dataTable.setDataSourceParam("pagination", pagi);
                this.dataTable.load();
            }
        }
    }
    public getProductDetail(prodId: string) {
        this._cService
            .getProductDetail(this.inputFile, prodId)
            .subscribe(
            (cat: string) => this.fillModel(cat),
            (err: Response) => this.handleError(err)
            );
    }

    public fillModel(product: string) {
        let projectJSON: Object = JSON.parse(product).map(m => m["Value"]);

        let newProduct: Product = this.fillViewModel(projectJSON[0]);
        this.selectedProduct = newProduct;
        this.selectedProductColor =
            this.selectedProduct.customAttribute.length > 0
                ? this.selectedProduct.customAttribute.filter(m => m.Key == "color")[0]
                    .Value
                : "";
        this.selectedProductSize =
            this.selectedProduct.customAttribute.length > 0
                ? this.selectedProduct.customAttribute.filter(m => m.Key == "size")[0]
                    .Value
                : "";
        this.selectedProductStyle =
            this.selectedProduct.customAttribute.length > 0
                ? this.selectedProduct.customAttribute.filter(m => m.Key == "style")[0]
                    .Value
                : "";
        $("#m_modal_1_2").modal("show");
    }

    public fillViewModel(proObj: Object): Product {
        let proVM = new Product();
        proVM.ProdId = proObj["@product-id"];
        proVM.Name = proObj["display-name"]["#text"];
        proVM.sku = proObj["manufacturer-sku"];
        proVM.shortDescription = proObj["short-description"]["#text"];
        proVM.longDescription = proObj["long-description"]["#text"];
        proVM.taxClass = proObj["tax-class-id"];
        proVM.variations = new Array<KeyValue<string>>();
        if (proObj["variations"]) {
            if (proObj["variations"]["attributes"]) {
                if (proObj["variations"]["attributes"]["variation-attribute"]) {
                    let arr: Array<Object> =
                        proObj["variations"]["attributes"]["variation-attribute"];
                    arr.forEach(vari => {
                        let keyV: KeyValue<string> = new KeyValue<string>();
                        keyV.Key = vari["@attribute-id"];
                        let val =
                            vari["variation-attribute-values"]["variation-attribute-value"];
                        if (Object.prototype.toString.call(val) === "[object Array]") {
                            val.forEach(a => {
                                keyV.Value = a["@value"];
                                let c = Object.assign({}, keyV);
                                proVM.variations.push(c);
                            });
                        } else {
                            if (val) {
                                keyV.Value = val["@value"];
                                proVM.variations.push(keyV);
                            }
                        }
                    });
                }
            }
            if (proObj["variations"]["variants"]) {
                if (proObj["variations"]["variants"]["variant"]) {
                    (proObj["variations"]["variants"]["variant"] as Array<
                        Object
                        >).forEach(a => {
                            proVM.variants.push(a["@product-id"]);
                        });
                }
            }
        }
        proVM.customAttribute = new Array<KeyValue<string>>();
        if (proObj["custom-attributes"]) {
            if (proObj["custom-attributes"]["custom-attribute"]) {
                let arr: Array<Object> = new Array<Object>();
                Object.assign(arr, proObj["custom-attributes"]["custom-attribute"]);
                arr.forEach(ar => {
                    let keyV: KeyValue<string> = new KeyValue<string>();
                    keyV.Key = ar["@attribute-id"];
                    keyV.Value = ar["#text"];
                    proVM.customAttribute.push(keyV);
                });
            }
        }

        return proVM;
    }

    private reloadTable(products: string, page: number, perPage: number): void {
        let prodArray: Array<Product> = new Array<Product>();
        let json: Array<Object> = JSON.parse(products).map(m => m["Value"]);

        json.forEach(pro => {
            prodArray.push(this.fillViewModel(pro));
        });
        if (this.searchQuery == "") {
            this.catProducts.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.catProducts;
        } else {
            this.searchResults.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.searchResults;
        }

        this.dataTable.jsonData = Object.assign({}, prodArray);
        this.dataTable.load();
    }

    private handleError(err: Response): void {
        console.error(err);
    }

    public uniqueKeys(arr, keyName) {
        var u = {},
            a = [];
        for (var i = 0, l = arr.length; i < l; ++i) {
            if (!u.hasOwnProperty(arr[i][keyName])) {
                a.push(arr[i][keyName]);
                u[arr[i][keyName]] = 1;
            }
        }
        return a;
    }
}
