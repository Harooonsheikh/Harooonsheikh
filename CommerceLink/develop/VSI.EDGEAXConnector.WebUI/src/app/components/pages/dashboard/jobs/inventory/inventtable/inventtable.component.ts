import { InventoryService } from "./../../../../../../_services/inventory.service";
import { KeyValue } from "./../../../../../../Entities/Common";
import { Observable } from "rxjs";
import {
    Component,
    OnInit,
    AfterViewInit,
    Input,
    enableProdMode
} from "@angular/core";
declare var jquery: any;
declare var $: any;

export class InventProduct {
    public prodId: string = "";
    public allocation: string = "";
    public allocationTime: string = "";
    public perpetual: string = "";
    public preOrder: string = "";
    public ats: string = "";
    public onOrder: string = "";
    public turnOver: string = "";
}

@Component({
    selector: "inventory-table",
    templateUrl: "inventtable.component.html"
})
export class InventTableComponent implements OnInit, AfterViewInit {
    @Input() public inputFile: string = "";
    public totalInventary: number = null;
    private inventProductOb: Observable<Object> = null;
    public inventProduct: Array<InventProduct> = null;
    public dataTable: any = null;
    public searchQuery: string = "";
    public searchResults: Array<InventProduct> = null;

    constructor(private _inventService: InventoryService) { }

    ngOnInit() { }
    ngAfterViewInit() {

        this._inventService.getRecordCount(this.inputFile).subscribe(
            (number: number) => {
                this.totalInventary = number;
                this.setupProductTable();
            },
            (error: Response) => this.handleError(error)
        );
    }

    private setupProductTable(): void {

        this.inventProductOb = this._inventService.getRecords(
            this.inputFile,
            0 * 10,
            10
        );
        this.inventProduct = new Array<InventProduct>();
        for (var i = 0; i < this.totalInventary; i++) {
            var element = new InventProduct();
            this.inventProduct.push(element);
        }

        this.dataTable = $("#inventProduct").mDatatable({
            data: {
                type: "local",
                source: this.inventProduct,
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
                    field: "prodId",
                    title: "Product Id",
                    width: "150",
                    selector: !1,
                    textAlign: "center"
                },
                {
                    field: "allocation",
                    title: "Allocation",
                    width: "150",
                    textAlign: "center"
                },
                {
                    field: "allocationTime",
                    title: "Allocation Date",
                    width: "100",
                    textAlign: "center"
                },
                {
                    field: "new Date(e.allocationTime).toLocaleTimeString()",
                    title: "Allocation Time",
                    width: "100",
                    textAlign: "center",
                    template: function(e) {
                        return new Date(e.allocationTime).toLocaleTimeString();
                    }
                },
                {
                    field: "perpetual",
                    title: "Perpetual",
                    width: "100",
                    textAlign: "center"
                },
                {
                    field: "preOrder",
                    title: "Pre Order",
                    width: "100",
                    textAlign: "center"
                },
                {
                    field: "ats",
                    title: "ATS",
                    width: "100",
                    textAlign: "center"
                },
                {
                    field: "onOrder",
                    title: "On Order",
                    width: "100",
                    textAlign: "center"
                },
                {
                    field: "turnOver",
                    title: "Turn Over",
                    width: "100",
                    textAlign: "center"
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
        //this._inventService.getProductByCatalog(this.selectedCatalogFile, pages * perPage, perPage)
        this.inventProductOb.subscribe(
            (m: string) => {
                self.reloadTable.apply(self, [m, pages, perPage]);
            },
            (err: Response) => {
                self.handleError(err);
            }
        );

        let self: InventTableComponent = this;
        // (function (component: any) {
        //     $("#productLog").on('click', 'tr > td:nth-child(8) a', function () {

        //         var data = $(this).closest("tr").find("td:nth-child(1) span").first().html();
        //         component.getProductDetail(data);
        //         return false;
        //     });
        // })(self);

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
                self._inventService
                    .getRecords(self.inputFile, pages * perPage, perPage)
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._inventService
                    .getSearchResult(
                    self.inputFile,
                    self.searchQuery,
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
                self._inventService
                    .getRecords(self.inputFile, pages * perPage, perPage)
                    .subscribe(
                    (m: string) => {
                        self.reloadTable.apply(self, [m, pages, perPage]);
                    },
                    (err: Response) => {
                        self.handleError(err);
                    }
                    );
            } else {
                self._inventService
                    .getSearchResult(
                    self.inputFile,
                    self.searchQuery,
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
                $("#inventProduct")
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
                this._inventService
                    .getSearchResultCount(this.inputFile, this.searchQuery)
                    .subscribe(
                    (res: number) => {
                        let numb: number = res;
                        this.searchResults = new Array<InventProduct>();
                        for (var n = 0; n < numb; n++) {
                            let prod: InventProduct = new InventProduct();
                            this.searchResults.push(prod);
                        }

                        var pages = 0;
                        var perPage = this.dataTable.getPageSize();

                        this._inventService
                            .getSearchResult(
                            this.inputFile,
                            this.searchQuery,
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
                this.dataTable.jsonData = this.inventProduct; // Object.assign({},this.inProduct);
                this.dataTable.fullJsonData = this.inventProduct;
                let pagi: any = this.dataTable.getDataSourceParam("pagination");
                pagi.page = 1;
                pagi.perpage = 10;
                this.dataTable.setDataSourceParam("pagination", pagi);
                this.dataTable.load();
            }
        }
    }


    public fillViewModel(proObj: Object): InventProduct {

        let catVM = new InventProduct();
        catVM.prodId = proObj["@product-id"];
        catVM.allocation = proObj["allocation"];
        catVM.allocationTime = new Date(
            proObj["allocation-timestamp"]
        ).toLocaleDateString();
        catVM.ats = proObj["ats"];
        catVM.onOrder = proObj["on-order"];
        catVM.perpetual = proObj["perpetual"];
        catVM.preOrder = proObj["preorder-backorder-handling"];
        catVM.turnOver = proObj["turnover"];
        // catVM.ProdId = proObj["@product-id"];
        // catVM.primaryFlag = proObj["primary-flag"];

        return catVM;
    }

    private reloadTable(products: string, page: number, perPage: number): void {
        let prodArray: Array<InventProduct> = new Array<InventProduct>();
        let json: Array<Object> = JSON.parse(products).map(m => m["Value"]);

        json.forEach(pro => {
            prodArray.push(this.fillViewModel(pro));
        });
        if (this.searchQuery == "") {
            this.inventProduct.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.inventProduct;
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
}
