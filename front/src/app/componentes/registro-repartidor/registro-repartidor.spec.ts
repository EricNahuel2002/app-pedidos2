import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistroRepartidor } from './registro-repartidor';

describe('RegistroRepartidor', () => {
  let component: RegistroRepartidor;
  let fixture: ComponentFixture<RegistroRepartidor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegistroRepartidor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegistroRepartidor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
