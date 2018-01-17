import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LedcardComponent } from './ledcard.component';

describe('LedcardComponent', () => {
  let component: LedcardComponent;
  let fixture: ComponentFixture<LedcardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LedcardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LedcardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
