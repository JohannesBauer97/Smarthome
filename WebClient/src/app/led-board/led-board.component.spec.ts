import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LedBoardComponent } from './led-board.component';

describe('LedBoardComponent', () => {
  let component: LedBoardComponent;
  let fixture: ComponentFixture<LedBoardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LedBoardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LedBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
