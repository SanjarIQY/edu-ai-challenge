import { Ship } from '../src/models/Ship.js';

describe('Ship', () => {
  let ship;

  beforeEach(() => {
    ship = new Ship(3);
  });

  test('should create a ship with correct length', () => {
    expect(ship.length).toBe(3);
    expect(ship.locations).toHaveLength(0);
    expect(ship.hits).toHaveLength(3);
  });

  test('should place ship correctly', () => {
    const locations = ['00', '01', '02'];
    ship.place(locations);
    expect(ship.locations).toEqual(locations);
  });

  test('should throw error when placing ship with wrong number of locations', () => {
    expect(() => ship.place(['00', '01'])).toThrow('Invalid number of locations for ship placement');
  });

  test('should register hits correctly', () => {
    ship.place(['00', '01', '02']);
    expect(ship.hit('00')).toBe(true);
    expect(ship.hit('00')).toBe(false); // Already hit
    expect(ship.hit('99')).toBe(false); // Invalid location
  });

  test('should detect sunk ship', () => {
    ship.place(['00', '01', '02']);
    expect(ship.isSunk()).toBe(false);
    ship.hit('00');
    ship.hit('01');
    ship.hit('02');
    expect(ship.isSunk()).toBe(true);
  });

  test('should get hit and missed locations', () => {
    ship.place(['00', '01', '02']);
    ship.hit('00');
    ship.hit('02');
    expect(ship.getHitLocations()).toEqual(['00', '02']);
    expect(ship.getMissedLocations()).toEqual(['01']);
  });
}); 