import { CPUStrategy } from '../src/game/CPUStrategy.js';

describe('CPUStrategy', () => {
  let strategy;

  beforeEach(() => {
    strategy = new CPUStrategy(10);
  });

  test('should initialize with correct state', () => {
    expect(strategy.boardSize).toBe(10);
    expect(strategy.mode).toBe('hunt');
    expect(strategy.targetQueue).toHaveLength(0);
    expect(strategy.lastHit).toBeNull();
  });

  test('should generate valid hunt mode guess', () => {
    const guess = strategy.generateGuess();
    expect(guess).toMatch(/^[0-9]{2}$/);
    const [row, col] = [parseInt(guess[0]), parseInt(guess[1])];
    expect(row).toBeGreaterThanOrEqual(0);
    expect(row).toBeLessThan(10);
    expect(col).toBeGreaterThanOrEqual(0);
    expect(col).toBeLessThan(10);
  });

  test('should switch to target mode after hit', () => {
    strategy.processResult('55', { hit: true, sunk: false });
    expect(strategy.mode).toBe('target');
    expect(strategy.lastHit).toBe('55');
    expect(strategy.targetQueue.length).toBeGreaterThan(0);
  });

  test('should add adjacent locations to target queue after hit', () => {
    strategy.processResult('55', { hit: true, sunk: false });
    const adjacent = strategy.getAdjacentLocations('55');
    adjacent.forEach(loc => {
      expect(strategy.targetQueue).toContain(loc);
    });
  });

  test('should return to hunt mode after ship sunk', () => {
    strategy.processResult('55', { hit: true, sunk: false });
    expect(strategy.mode).toBe('target');
    
    strategy.processResult('56', { hit: true, sunk: true });
    expect(strategy.mode).toBe('hunt');
    expect(strategy.targetQueue).toHaveLength(0);
    expect(strategy.lastHit).toBeNull();
  });

  test('should generate valid adjacent locations', () => {
    const adjacent = strategy.getAdjacentLocations('55');
    expect(adjacent).toContain('45');
    expect(adjacent).toContain('65');
    expect(adjacent).toContain('54');
    expect(adjacent).toContain('56');
    expect(adjacent).not.toContain('44');
  });

  test('should not include invalid positions in adjacent locations', () => {
    const adjacent = strategy.getAdjacentLocations('00');
    expect(adjacent).not.toContain('-10');
    expect(adjacent).not.toContain('0-1');
    expect(adjacent).toContain('01');
    expect(adjacent).toContain('10');
  });
}); 