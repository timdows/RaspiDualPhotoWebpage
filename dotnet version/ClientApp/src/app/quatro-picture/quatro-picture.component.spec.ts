import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuatroPictureComponent } from './quatro-picture.component';

describe('QuatroPictureComponent', () => {
  let component: QuatroPictureComponent;
  let fixture: ComponentFixture<QuatroPictureComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuatroPictureComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuatroPictureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
