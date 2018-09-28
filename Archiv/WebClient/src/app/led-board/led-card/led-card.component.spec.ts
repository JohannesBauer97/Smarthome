import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LedCardComponent } from './led-card.component';

describe('LedCardComponent', () => {
  let component: LedCardComponent;
  let fixture: ComponentFixture<LedCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LedCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LedCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
