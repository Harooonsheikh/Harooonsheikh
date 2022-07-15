import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleOrderTransactionComponent } from './sale-order-transaction.component';

describe('SaleOrderTransactionComponent', () => {
  let component: SaleOrderTransactionComponent;
  let fixture: ComponentFixture<SaleOrderTransactionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleOrderTransactionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleOrderTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
