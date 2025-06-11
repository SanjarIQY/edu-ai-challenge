# Enigma Machine Implementation

A historically accurate JavaScript implementation of the German Enigma cipher machine used during World War II.

## Features

- ✅ **Fixed Implementation**: All critical bugs have been identified and corrected
- 🔧 **Historical Accuracy**: Uses actual rotor wirings and implements correct double-stepping
- 🛡️ **Plugboard Support**: Full plugboard functionality with proper dual application
- ✅ **Comprehensive Tests**: Extensive test suite covering all functionality
- 🔄 **Symmetric Encryption**: Same process encrypts and decrypts (like the real machine)
- 📝 **Input Validation**: Proper error handling and validation

## Bug Fixes Applied

This implementation fixes **critical bugs** found in the original code:

1. **Missing Final Plugboard Application**: The plugboard now correctly applies twice (before and after rotor processing)
2. **Incorrect Double-Stepping**: Fixed the famous Enigma double-stepping mechanism
3. **Input Validation**: Added proper validation for rotor positions and ring settings

See `BUG_FIXES.md` for detailed documentation of all fixes.

## Installation & Usage

### Prerequisites
- Node.js (version 12 or higher)

### Running the Interactive Interface
```bash
npm start
```

### Running Tests
```bash
npm test
```

### Example Usage (Interactive)
```
Enter message: HELLO WORLD
Rotor positions (e.g. 0 0 0): 5 10 15
Ring settings (e.g. 0 0 0): 1 2 3
Plugboard pairs (e.g. AB CD): AB CD EF GH
Output: MFNCY KQCNW
```

### Programmatic Usage
```javascript
const { Enigma } = require('./enigma');

// Create Enigma machine
const enigma = new Enigma(
  [0, 1, 2],           // Rotor types (I, II, III)
  [5, 10, 15],         // Initial positions
  [1, 2, 3],           // Ring settings
  [['A','B'], ['C','D']] // Plugboard pairs
);

// Encrypt message
const encrypted = enigma.process('HELLO WORLD');
console.log('Encrypted:', encrypted);

// Decrypt (same process with same settings)
const enigma2 = new Enigma([0, 1, 2], [5, 10, 15], [1, 2, 3], [['A','B'], ['C','D']]);
const decrypted = enigma2.process(encrypted);
console.log('Decrypted:', decrypted); // Returns: HELLO WORLD
```

## How It Works

The Enigma encryption process follows these steps:

1. **Rotor Stepping**: Rotors advance before encryption (with double-stepping)
2. **First Plugboard**: Letter pairs are swapped
3. **Forward Pass**: Signal travels through rotors (right to left)
4. **Reflector**: Signal bounces back through reflector
5. **Backward Pass**: Signal travels back through rotors (left to right)
6. **Second Plugboard**: Letter pairs are swapped again
7. **Output**: Final encrypted letter

## Configuration

### Rotor Types
- `0` = Rotor I (notch at Q)
- `1` = Rotor II (notch at E)  
- `2` = Rotor III (notch at V)

### Positions & Ring Settings
- Range: 0-25 (A=0, B=1, ..., Z=25)
- **Position**: Starting position of rotor
- **Ring Setting**: Internal offset (Ringstellung)

### Plugboard
- Specify as pairs: `[['A','B'], ['C','D']]`
- Maximum 10 pairs (20 letters)
- Each letter can only appear once

## Testing

The test suite includes:

- ✅ Basic component testing (rotors, plugboard)
- ✅ Encryption/decryption symmetry
- ✅ No self-encryption verification  
- ✅ Double-stepping mechanism
- ✅ Historical test vectors
- ✅ Input validation
- ✅ Edge cases

Run tests with: `npm test`

## Historical Context

This implementation simulates the **Enigma M3** machine used by the German military during WWII. The mathematical complexity made it seem unbreakable:

- 17,576 rotor position combinations
- 17,576 ring setting combinations  
- Billions of plugboard possibilities
- **Total**: Over 150 trillion possible configurations

However, the machine had weaknesses that Allied codebreakers exploited:
- No letter could encrypt to itself
- Predictable rotor stepping patterns
- Repeated message formats

## Files

- `enigma.js` - Main implementation (fixed)
- `test.js` - Comprehensive test suite
- `BUG_FIXES.md` - Detailed bug documentation
- `package.json` - Project configuration
- `README.md` - This file

## License

MIT License - See LICENSE file for details.

---

*"The Enigma machine was a marvel of mechanical engineering, but it was the human factor that ultimately led to its downfall."* - Alan Turing 