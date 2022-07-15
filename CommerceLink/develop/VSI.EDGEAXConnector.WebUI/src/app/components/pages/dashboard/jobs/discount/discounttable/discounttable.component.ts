import { DiscountService } from "./../../../../../../_services/discount.service";
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

export class PriceEntity {
    public prodId: string = "";
    public quantity: string = "";
    public amount: string = "";
}

@Component({
    selector: "discount-table",
    templateUrl: "discounttable.component.html"
})
export class DiscountTableComponent implements OnInit, AfterViewInit {
    @Input() public inputFile: string = "";
    public total: number = null;
    private discountBookOb: Observable<Object> = null;
    public entityList: Array<PriceEntity> = null;
    public dataTable: any = null;
    public searchQuery: string = "";
    public searchResults: Array<PriceEntity> = null;

    constructor(private _dService: DiscountService) { }

    ngOnInit() { }
    ngAfterViewInit() {

        this._dService.getRecordCount(this.inputFile).subscribe(
            (number: number) => {
                this.total = number;
                this.setupProductTable();
            },
            (error: Response) => this.handleError(error)
        );
    }

    private setupProductTable(): void {

        this.discountBookOb = this._dService.getRecords(this.inputFile, 0 * 10, 10);
        this.entityList = new Array<PriceEntity>();
        for (var i = 0; i < this.total; i++) {
            var element = new PriceEntity();
            this.entityList.push(element);
        }

        this.dataTable = $("#discountTable").mDatatable({
            data: {
                type: "local",
                source: this.entityList,
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
                    field: "quantity",
                    title: "Quantity",
                    width: "150",
                    selector: !1,
                    textAlign: "center"
                },
                {
                    field: "amount",
                    title: "Amount",
                    width: "150",
                    selector: !1,
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
        //this._dService.getProductByCatalog(this.selectedCatalogFile, pages * perPage, perPage)
        this.discountBookOb.subscribe(
            (m: string) => {
                self.reloadTable.apply(self, [m, pages, perPage]);
            },
            (err: Response) => {
                self.handleError(err);
            }
        );

        let self: DiscountTableComponent = this;
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
                self._dService
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
                self._dService
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
                self._dService
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
                self._dService
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
                $("#discountTable")
                    .find("tbody > tr")
                    .each(function(a, b) {
                        if ($(b).find("td:nth-child(1) span").length > 0) {
                            if (
                                $(b)
                                    .find("td:nth-child(1) span")
                                    .first()
                                    .html() == ""
                            ) {

                                //alert();
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
                this._dService
                    .getSearchResultCount(this.inputFile, this.searchQuery)
                    .subscribe(
                    (res: number) => {
                        let numb: number = res;
                        this.searchResults = new Array<PriceEntity>();
                        for (var n = 0; n < numb; n++) {
                            let prod: PriceEntity = new PriceEntity();
                            this.searchResults.push(prod);
                        }


                        var pages = 0;
                        var perPage = this.dataTable.getPageSize();

                        this._dService
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
                this.dataTable.jsonData = this.entityList; // Object.assign({},this.inProduct);
                this.dataTable.fullJsonData = this.entityList;
                let pagi: any = this.dataTable.getDataSourceParam("pagination");
                pagi.page = 1;
                pagi.perpage = 10;
                this.dataTable.setDataSourceParam("pagination", pagi);
                this.dataTable.load();
            }
        }
    }


    public fillViewModel(proObj: Object): PriceEntity {

        let catVM = new PriceEntity();
        catVM.prodId = proObj["@product-id"];
        catVM.amount = proObj["amount"]["#text"];
        catVM.quantity = proObj["amount"]["@quantity"];
        return catVM;
    }

    private reloadTable(products: string, page: number, perPage: number): void {
        let prodArray: Array<PriceEntity> = new Array<PriceEntity>();
        let json: Array<Object> = JSON.parse(products).map(m => m["Value"]);
        json.forEach(pro => {
            prodArray.push(this.fillViewModel(pro));
        });
        if (this.searchQuery == "") {
            this.entityList.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.entityList;
        } else {
            this.searchResults.splice(page * perPage, perPage, ...prodArray);
            this.dataTable.fullJsonData = this.searchResults;
        }

        this.dataTable.jsonData = Object.assign({}, prodArray); // Object.assign({},this.catAssign);
        this.dataTable.load();
    }

    private handleError(err: Response): void {
        console.error(err);
    }
}
