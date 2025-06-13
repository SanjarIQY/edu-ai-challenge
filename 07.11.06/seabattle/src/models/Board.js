import { Ship } from './Ship.js';

export class Board {
  constructor(size = 10) {
    this.size = size;
    this.grid = Array(size).fill().map(() => Array(size).fill('~'));
    this.ships = [];
    this.guesses = new Set();
  }

  placeShip(ship, locations) {
    // Validate locations
    for (const location of locations) {
      const [row, col] = this.parseLocation(location);
      if (!this.isValidPosition(row, col)) {
        throw new Error('Invalid ship placement location');
      }
      if (this.grid[row][col] !== '~') {
        throw new Error('Ship placement overlaps with existing ship');
      }
    }

    // Place ship
    ship.place(locations);
    this.ships.push(ship);
    
    // Update grid
    for (const location of locations) {
      const [row, col] = this.parseLocation(location);
      this.grid[row][col] = 'S';
    }
  }

  receiveAttack(location) {
    if (this.guesses.has(location)) {
      return { hit: false, message: 'Location already guessed' };
    }

    this.guesses.add(location);
    const [row, col] = this.parseLocation(location);

    if (!this.isValidPosition(row, col)) {
      return { hit: false, message: 'Invalid position' };
    }

    for (const ship of this.ships) {
      if (ship.hit(location)) {
        this.grid[row][col] = 'X';
        const sunk = ship.isSunk();
        return {
          hit: true,
          sunk,
          message: sunk ? 'Ship sunk!' : 'Hit!'
        };
      }
    }

    this.grid[row][col] = 'O';
    return { hit: false, message: 'Miss!' };
  }

  isGameOver() {
    // If there are no ships, game is over
    if (this.ships.length === 0) return true;
    return this.ships.every(ship => ship.isSunk());
  }

  getAdjacentLocations(location) {
    const [row, col] = this.parseLocation(location);
    const adjacent = [
      { r: row - 1, c: col },
      { r: row + 1, c: col },
      { r: row, c: col - 1 },
      { r: row, c: col + 1 }
    ];

    return adjacent
      .filter(({ r, c }) => this.isValidPosition(r, c))
      .map(({ r, c }) => `${r}${c}`);
  }

  parseLocation(location) {
    if (typeof location !== 'string' || !/^[0-9]{2}$/.test(location)) {
      throw new Error('Invalid ship placement location');
    }
    const row = Number(location[0]);
    const col = Number(location[1]);
    if (!this.isValidPosition(row, col)) {
      throw new Error('Invalid ship placement location');
    }
    return [row, col];
  }

  isValidPosition(row, col) {
    return row >= 0 && row < this.size && col >= 0 && col < this.size;
  }

  toString() {
    let result = '  ';
    for (let i = 0; i < this.size; i++) {
      result += `${i} `;
    }
    result += '\n';

    for (let i = 0; i < this.size; i++) {
      result += `${i} `;
      for (let j = 0; j < this.size; j++) {
        result += `${this.grid[i][j]} `;
      }
      result += '\n';
    }
    return result;
  }
} 