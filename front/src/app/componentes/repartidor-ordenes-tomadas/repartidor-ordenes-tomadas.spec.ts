import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepartidorOrdenesTomadas } from './repartidor-ordenes-tomadas';

describe('RepartidorOrdenesTomadas', () => {
  let component: RepartidorOrdenesTomadas;
  let fixture: ComponentFixture<RepartidorOrdenesTomadas>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RepartidorOrdenesTomadas]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RepartidorOrdenesTomadas);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
