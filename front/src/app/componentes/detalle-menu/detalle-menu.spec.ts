import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleMenu } from './detalle-menu';

describe('DetalleMenu', () => {
  let component: DetalleMenu;
  let fixture: ComponentFixture<DetalleMenu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetalleMenu]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetalleMenu);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
