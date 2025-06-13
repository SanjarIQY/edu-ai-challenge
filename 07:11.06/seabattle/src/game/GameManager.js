import { Board } from '../models/Board.js';
import { Ship } from '../models/Ship.js';
import { CPUStrategy } from './CPUStrategy.js';

export class GameManager {
  constructor(boardSize = 10, numShips = 3, shipLength = 3) {
    this.boardSize = boardSize;
    this.numShips = numShips;
    this.shipLength = shipLength;
    
    this.playerBoard = new Board(boardSize);
    this.cpuBoard = new Board(boardSize);
    this.cpuStrategy = new CPUStrategy(boardSize);
    
    this.initializeBoards();
  }

  initializeBoards() {
    // Place ships for both players
    for (let i = 0; i < this.numShips; i++) {
      this.placeRandomShip(this.playerBoard);
      this.placeRandomShip(this.cpuBoard);
    }
  }

  placeRandomShip(board) {
    const ship = new Ship(this.shipLength);
    let placed = false;

    while (!placed) {
      try {
        const orientation = Math.random() < 0.5 ? 'horizontal' : 'vertical';
        const locations = this.generateRandomShipLocations(orientation);
        board.placeShip(ship, locations);
        placed = true;
      } catch (error) {
        // If placement fails, try again
        continue;
      }
    }
  }

  generateRandomShipLocations(orientation) {
    const locations = [];
    let startRow, startCol;

    if (orientation === 'horizontal') {
      startRow = Math.floor(Math.random() * this.boardSize);
      startCol = Math.floor(Math.random() * (this.boardSize - this.shipLength + 1));
    } else {
      startRow = Math.floor(Math.random() * (this.boardSize - this.shipLength + 1));
      startCol = Math.floor(Math.random() * this.boardSize);
    }

    for (let i = 0; i < this.shipLength; i++) {
      const row = orientation === 'horizontal' ? startRow : startRow + i;
      const col = orientation === 'horizontal' ? startCol + i : startCol;
      locations.push(`${row}${col}`);
    }

    return locations;
  }

  processPlayerGuess(guess) {
    if (!this.isValidGuess(guess)) {
      return { valid: false, message: 'Invalid guess format. Use two digits (e.g., 00, 34)' };
    }

    const result = this.cpuBoard.receiveAttack(guess);
    return { valid: true, ...result };
  }

  processCPUGuess() {
    const guess = this.cpuStrategy.generateGuess();
    const result = this.playerBoard.receiveAttack(guess);
    this.cpuStrategy.processResult(guess, result);
    return { guess, ...result };
  }

  isValidGuess(guess) {
    if (!guess || guess.length !== 2) return false;
    
    const [row, col] = [parseInt(guess[0]), parseInt(guess[1])];
    return !isNaN(row) && !isNaN(col) &&
           row >= 0 && row < this.boardSize &&
           col >= 0 && col < this.boardSize;
  }

  isGameOver() {
    return this.playerBoard.isGameOver() || this.cpuBoard.isGameOver();
  }

  getWinner() {
    if (this.playerBoard.isGameOver()) return 'CPU';
    if (this.cpuBoard.isGameOver()) return 'Player';
    return null;
  }

  getBoardState() {
    return {
      playerBoard: this.playerBoard.toString(),
      cpuBoard: this.cpuBoard.toString()
    };
  }
} 