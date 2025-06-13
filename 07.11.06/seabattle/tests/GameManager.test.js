import { GameManager } from '../src/game/GameManager.js';

describe('GameManager', () => {
  let gameManager;

  beforeEach(() => {
    gameManager = new GameManager(10, 3, 3);
  });

  test('should initialize game with correct settings', () => {
    expect(gameManager.boardSize).toBe(10);
    expect(gameManager.numShips).toBe(3);
    expect(gameManager.shipLength).toBe(3);
    expect(gameManager.playerBoard).toBeDefined();
    expect(gameManager.cpuBoard).toBeDefined();
    expect(gameManager.cpuStrategy).toBeDefined();
  });

  test('should validate player guesses', () => {
    expect(gameManager.isValidGuess('00')).toBe(true);
    expect(gameManager.isValidGuess('99')).toBe(true);
    expect(gameManager.isValidGuess('9')).toBe(false);
    expect(gameManager.isValidGuess('999')).toBe(false);
    expect(gameManager.isValidGuess('ab')).toBe(false);
  });

  test('should process valid player guess', () => {
    const result = gameManager.processPlayerGuess('00');
    expect(result.valid).toBe(true);
    expect(['Hit!', 'Miss!']).toContain(result.message);
  });

  test('should reject invalid player guess', () => {
    const result = gameManager.processPlayerGuess('999');
    expect(result.valid).toBe(false);
    expect(result.message).toBe('Invalid guess format. Use two digits (e.g., 00, 34)');
  });

  test('should process CPU guess', () => {
    const result = gameManager.processCPUGuess();
    expect(result.guess).toMatch(/^[0-9]{2}$/);
    expect(['Hit!', 'Miss!']).toContain(result.message);
  });

  test('should detect game over state', () => {
    expect(gameManager.isGameOver()).toBe(false);
    
    // Simulate sinking all CPU ships
    gameManager.cpuBoard.ships.forEach(ship => {
      ship.locations.forEach(loc => {
        ship.hit(loc);
      });
    });
    
    expect(gameManager.isGameOver()).toBe(true);
    expect(gameManager.getWinner()).toBe('Player');
  });

  test('should get board state', () => {
    const state = gameManager.getBoardState();
    expect(state.playerBoard).toBeDefined();
    expect(state.cpuBoard).toBeDefined();
    expect(typeof state.playerBoard).toBe('string');
    expect(typeof state.cpuBoard).toBe('string');
  });

  test('should generate valid random ship locations', () => {
    const locations = gameManager.generateRandomShipLocations('horizontal');
    expect(locations).toHaveLength(3);
    locations.forEach(loc => {
      expect(loc).toMatch(/^[0-9]{2}$/);
      const [row, col] = [parseInt(loc[0]), parseInt(loc[1])];
      expect(row).toBeGreaterThanOrEqual(0);
      expect(row).toBeLessThan(10);
      expect(col).toBeGreaterThanOrEqual(0);
      expect(col).toBeLessThan(10);
    });
  });
}); 