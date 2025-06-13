export class CPUStrategy {
  constructor(boardSize) {
    this.boardSize = boardSize;
    this.mode = 'hunt';
    this.targetQueue = [];
    this.lastHit = null;
  }

  generateGuess() {
    if (this.mode === 'target' && this.targetQueue.length > 0) {
      return this.targetQueue.shift();
    }

    this.mode = 'hunt';
    let guess;
    do {
      const row = Math.floor(Math.random() * this.boardSize);
      const col = Math.floor(Math.random() * this.boardSize);
      guess = `${row}${col}`;
    } while (this.targetQueue.includes(guess));

    return guess;
  }

  processResult(guess, result) {
    if (result.hit) {
      this.lastHit = guess;
      this.mode = 'target';
      
      // Add adjacent locations to target queue
      const adjacent = this.getAdjacentLocations(guess);
      this.targetQueue.push(...adjacent.filter(loc => !this.targetQueue.includes(loc)));
    }

    if (result.sunk) {
      this.mode = 'hunt';
      this.targetQueue = [];
      this.lastHit = null;
    }
  }

  getAdjacentLocations(location) {
    const [row, col] = [parseInt(location[0]), parseInt(location[1])];
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

  isValidPosition(row, col) {
    return row >= 0 && row < this.boardSize && col >= 0 && col < this.boardSize;
  }
} 