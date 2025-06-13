# Sea Battle Game

A modern implementation of the classic Battleship game using JavaScript ES6+ features.

## Features

- 10x10 game board
- Turn-based gameplay
- CPU opponent with intelligent targeting strategy
- Modern ES6+ code structure
- Comprehensive test coverage
- Clean separation of concerns

## Project Structure

```
seabattle/
├── src/
│   ├── models/
│   │   ├── Board.js    # Game board implementation
│   │   ├── Ship.js     # Ship class implementation
│   │   └── Player.js   # Player class implementation
│   ├── game/
│   │   ├── GameManager.js    # Main game logic
│   │   └── CPUStrategy.js    # CPU opponent strategy
│   ├── utils/
│   │   ├── constants.js      # Game constants
│   │   └── validators.js     # Input validation
│   └── index.js             # Main game entry point
├── tests/
│   ├── Board.test.js
│   ├── Ship.test.js
│   ├── GameManager.test.js
│   └── CPUStrategy.test.js
├── package.json
└── README.md
```

## Setup

1. Install dependencies:
```bash
npm install
```

2. Run the game:
```bash
npm start
```

3. Run tests:
```bash
npm test
```

4. Check test coverage:
```bash
npm run test:coverage
```

## How to Play

1. The game is played on a 10x10 grid
2. Each player has 3 ships of length 3
3. Ships are placed randomly at the start of the game
4. Take turns guessing coordinates (e.g., "00", "34")
5. 'X' marks a hit, 'O' marks a miss
6. First player to sink all opponent's ships wins

## Game Rules

- Enter coordinates as two digits (e.g., "00", "34")
- First digit is the row (0-9)
- Second digit is the column (0-9)
- '~' represents unknown/empty space
- 'S' represents your ship (only visible on your board)
- 'X' represents a hit
- 'O' represents a miss

## Development

The codebase is structured using modern JavaScript features:

- ES6+ syntax
- Classes and modules
- Arrow functions
- Modern array methods
- Proper encapsulation
- Comprehensive error handling

## Testing

The project uses Jest for testing. Tests cover:

- Ship placement and hit detection
- Board state management
- CPU strategy logic
- Game flow and win conditions
- Input validation

Run tests with:
```bash
npm test
```

## License

MIT 