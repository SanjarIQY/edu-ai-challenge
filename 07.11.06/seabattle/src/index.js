import readline from 'readline';
import { GameManager } from './game/GameManager.js';

const rl = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

class Game {
  constructor() {
    this.gameManager = new GameManager();
    this.setupGame();
  }

  setupGame() {
    console.log('Welcome to Sea Battle!');
    console.log('The game is set up with a 10x10 grid.');
    console.log('Enter coordinates as two digits (e.g., 00, 34) to make your guess.');
    this.printBoards();
    this.playerTurn();
  }

  printBoards() {
    const playerGrid = this.gameManager.playerBoard.grid;
    const cpuGrid = this.gameManager.cpuBoard.grid;
    const size = this.gameManager.boardSize;

    // Calculate padding for centering
    const colLabel = Array.from({ length: size }, (_, i) => i).map(n => n.toString()).join(' ');
    const boardWidth = 3 + (size * 2 - 1); // 3 for row label and space, then columns
    const gap = '   ';
    const cpuLabel = 'OPPONENT BOARD';
    const playerLabel = 'YOUR BOARD';
    const cpuLabelPad = ' '.repeat(Math.max(0, boardWidth / 2 - cpuLabel.length / 2));
    const playerLabelPad = ' '.repeat(Math.max(0, boardWidth / 2 - playerLabel.length / 2));

    // Print labels
    console.log();
    console.log(cpuLabelPad + cpuLabel + gap + playerLabelPad + playerLabel);
    // Print column numbers
    console.log('   ' + colLabel + gap + '   ' + colLabel);

    // Print rows side by side
    for (let i = 0; i < size; i++) {
      let cpuRow = (i < 10 ? ' ' : '') + i + ' ';
      let playerRow = (i < 10 ? ' ' : '') + i + ' ';
      for (let j = 0; j < size; j++) {
        // Hide CPU ships
        cpuRow += (cpuGrid[i][j] === 'S' ? '~' : cpuGrid[i][j]) + ' ';
        playerRow += playerGrid[i][j] + ' ';
      }
      // Remove trailing space
      cpuRow = cpuRow.trimEnd();
      playerRow = playerRow.trimEnd();
      console.log(cpuRow + gap + playerRow);
    }
    console.log();
  }

  playerTurn() {
    rl.question('\nEnter your guess (e.g., 00): ', (guess) => {
      const result = this.gameManager.processPlayerGuess(guess);
      
      if (!result.valid) {
        console.log(result.message);
        this.playerTurn();
        return;
      }

      console.log(`\nYour guess: ${guess}`);
      console.log(`Result: ${result.message}`);

      if (this.gameManager.isGameOver()) {
        this.endGame();
        return;
      }

      this.cpuTurn();
    });
  }

  cpuTurn() {
    console.log("\n--- CPU's Turn ---");
    const result = this.gameManager.processCPUGuess();
    console.log(`CPU guesses: ${result.guess}`);
    console.log(`Result: ${result.message}`);

    this.printBoards();

    if (this.gameManager.isGameOver()) {
      this.endGame();
      return;
    }

    this.playerTurn();
  }

  endGame() {
    const winner = this.gameManager.getWinner();
    console.log(`\nGame Over! ${winner} wins!`);
    rl.close();
  }
}

// Start the game
new Game(); 