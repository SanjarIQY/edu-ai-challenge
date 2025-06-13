import { Board } from '../src/models/Board.js';
import { Ship } from '../src/models/Ship.js';

describe('Board', () => {
  let board;
  let ship;

  beforeEach(() => {
    board = new Board(10);
    ship = new Ship(3);
  });

  test('should create board with correct size', () => {
    expect(board.size).toBe(10);
    expect(board.grid).toHaveLength(10);
    expect(board.grid[0]).toHaveLength(10);
    expect(board.grid[0][0]).toBe('~');
  });

  test('should place ship correctly', () => {
    const locations = ['00', '01', '02'];
    board.placeShip(ship, locations);
    expect(board.ships).toContain(ship);
    expect(board.grid[0][0]).toBe('S');
    expect(board.grid[0][1]).toBe('S');
    expect(board.grid[0][2]).toBe('S');
  });

  test('should throw error when placing ship in invalid location', () => {
    // Test with out-of-bounds coordinates on a 2x2 board
    const smallBoard = new Board(2);
    const smallShip = new Ship(3);
    expect(() => smallBoard.placeShip(smallShip, ['22', '23', '24'])).toThrow('Invalid ship placement location');
    // Test with non-numeric coordinates
    expect(() => board.placeShip(ship, ['a0', 'b1', 'c2'])).toThrow('Invalid ship placement location');
    // Test with invalid format
    expect(() => board.placeShip(ship, ['0', '1', '2'])).toThrow('Invalid ship placement location');
  });

  test('should throw error when placing ship in occupied location', () => {
    board.placeShip(ship, ['00', '01', '02']);
    const newShip = new Ship(3);
    expect(() => board.placeShip(newShip, ['00', '10', '20'])).toThrow('Ship placement overlaps with existing ship');
  });

  test('should process attack correctly', () => {
    board.placeShip(ship, ['00', '01', '02']);
    
    // Hit
    const hitResult = board.receiveAttack('00');
    expect(hitResult.hit).toBe(true);
    expect(hitResult.message).toBe('Hit!');
    expect(board.grid[0][0]).toBe('X');
    
    // Miss
    const missResult = board.receiveAttack('99');
    expect(missResult.hit).toBe(false);
    expect(missResult.message).toBe('Miss!');
    
    // Already guessed
    const repeatResult = board.receiveAttack('00');
    expect(repeatResult.hit).toBe(false);
    expect(repeatResult.message).toBe('Location already guessed');
  });

  test('should detect sunk ship', () => {
    board.placeShip(ship, ['00', '01', '02']);
    board.receiveAttack('00');
    board.receiveAttack('01');
    const result = board.receiveAttack('02');
    expect(result.sunk).toBe(true);
    expect(result.message).toBe('Ship sunk!');
  });

  test('should get adjacent locations', () => {
    const adjacent = board.getAdjacentLocations('55');
    expect(adjacent).toContain('45');
    expect(adjacent).toContain('65');
    expect(adjacent).toContain('54');
    expect(adjacent).toContain('56');
    expect(adjacent).not.toContain('44');
  });

  test('should check game over state', () => {
    expect(board.isGameOver()).toBe(true); // No ships
    
    board.placeShip(ship, ['00', '01', '02']);
    expect(board.isGameOver()).toBe(false);
    
    board.receiveAttack('00');
    board.receiveAttack('01');
    board.receiveAttack('02');
    expect(board.isGameOver()).toBe(true);
  });
}); 