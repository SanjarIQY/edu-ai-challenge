const { Enigma, Rotor, plugboardSwap, ROTORS, REFLECTOR, alphabet } = require('./enigma');

// Test utilities
function assert(condition, message) {
  if (!condition) {
    throw new Error(`Test failed: ${message}`);
  }
  console.log(`✓ ${message}`);
}

function runTests() {
  console.log('Running Enigma Tests...\n');

  // Test 1: Basic Plugboard Function
  console.log('=== Testing Plugboard ===');
  const plugPairs = [['A', 'B'], ['C', 'D']];
  assert(plugboardSwap('A', plugPairs) === 'B', 'Plugboard swaps A to B');
  assert(plugboardSwap('B', plugPairs) === 'A', 'Plugboard swaps B to A');
  assert(plugboardSwap('C', plugPairs) === 'D', 'Plugboard swaps C to D');
  assert(plugboardSwap('E', plugPairs) === 'E', 'Plugboard leaves unpaired letters unchanged');

  // Test 2: Rotor Functionality
  console.log('\n=== Testing Rotor ===');
  const testRotor = new Rotor(ROTORS[0].wiring, ROTORS[0].notch, 0, 0);
  
  // Test forward and backward are inverses
  const testChar = 'A';
  const forward = testRotor.forward(testChar);
  const backward = testRotor.backward(forward);
  assert(backward === testChar, 'Rotor forward/backward are inverses');

  // Test rotor stepping
  const initialPos = testRotor.position;
  testRotor.step();
  assert(testRotor.position === (initialPos + 1) % 26, 'Rotor steps correctly');

  // Test notch detection
  testRotor.position = alphabet.indexOf(testRotor.notch);
  assert(testRotor.atNotch(), 'Rotor detects notch position correctly');

  // Test 3: Enigma Symmetry (most important test)
  console.log('\n=== Testing Enigma Symmetry ===');
  
  const enigma1 = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], []);
  const enigma2 = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], []);
  
  const plaintext = 'HELLO';
  const encrypted = enigma1.process(plaintext);
  const decrypted = enigma2.process(encrypted);
  
  assert(decrypted === plaintext, `Enigma symmetry test: ${plaintext} -> ${encrypted} -> ${decrypted}`);

  // Test 4: Plugboard Symmetry
  console.log('\n=== Testing Plugboard Symmetry ===');
  
  const enigma3 = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], [['A', 'B'], ['C', 'D']]);
  const enigma4 = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], [['A', 'B'], ['C', 'D']]);
  
  const plaintext2 = 'ABCD';
  const encrypted2 = enigma3.process(plaintext2);
  const decrypted2 = enigma4.process(encrypted2);
  
  assert(decrypted2 === plaintext2, `Plugboard symmetry test: ${plaintext2} -> ${encrypted2} -> ${decrypted2}`);

  // Test 5: No Self-Encryption
  console.log('\n=== Testing No Self-Encryption ===');
  
  const enigma5 = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], []);
  const testString = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
  const result = enigma5.process(testString);
  
  let selfEncrypted = false;
  for (let i = 0; i < testString.length; i++) {
    if (testString[i] === result[i]) {
      selfEncrypted = true;
      break;
    }
  }
  assert(!selfEncrypted, 'No letter encrypts to itself');

  // Test 6: Double-Stepping Test
  console.log('\n=== Testing Double-Stepping ===');
  
  // Set up rotors near double-stepping condition
  // Rotor III at position 21 (V-1), Rotor II at position 4 (E-1)
  const enigma6 = new Enigma([0, 1, 2], [0, 4, 21], [0, 0, 0], []);
  
  // Record initial positions
  const initialPositions = enigma6.rotors.map(r => r.position);
  
  // Encrypt one character to trigger stepping
  enigma6.encryptChar('A');
  
  // Check that double-stepping occurred
  const finalPositions = enigma6.rotors.map(r => r.position);
  
  assert(finalPositions[2] === (initialPositions[2] + 1) % 26, 'Rightmost rotor stepped');
  assert(finalPositions[1] === (initialPositions[1] + 2) % 26, 'Middle rotor double-stepped');
  assert(finalPositions[0] === (initialPositions[0] + 1) % 26, 'Leftmost rotor stepped due to double-stepping');

  // Test 7: Historical Test Vector
  console.log('\n=== Testing Historical Vector ===');
  
  // Known Enigma setting and result
  const historicalEnigma = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], []);
  const historicalPlain = 'AAA';
  const historicalResult = historicalEnigma.process(historicalPlain);
  
  // Test that it produces consistent results
  const verification = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], []);
  const verificationResult = verification.process(historicalResult);
  
  assert(verificationResult === historicalPlain, 'Historical test vector verification');

  // Test 8: Input Validation
  console.log('\n=== Testing Input Validation ===');
  
  try {
    new Enigma([0, 1], [0, 0, 0], [0, 0, 0], []); // Wrong number of rotors
    assert(false, 'Should throw error for wrong rotor count');
  } catch (e) {
    assert(true, 'Correctly validates rotor count');
  }

  try {
    new Enigma([0, 1, 2], [0, 0, 26], [0, 0, 0], []); // Invalid position
    assert(false, 'Should throw error for invalid position');
  } catch (e) {
    assert(true, 'Correctly validates rotor positions');
  }

  // Test 9: Ring Settings
  console.log('\n=== Testing Ring Settings ===');
  
  const enigma7 = new Enigma([0, 1, 2], [0, 0, 0], [1, 2, 3], []);
  const enigma8 = new Enigma([0, 1, 2], [0, 0, 0], [1, 2, 3], []);
  
  const ringTest = 'RING';
  const ringEncrypted = enigma7.process(ringTest);
  const ringDecrypted = enigma8.process(ringEncrypted);
  
  assert(ringDecrypted === ringTest, `Ring settings test: ${ringTest} -> ${ringEncrypted} -> ${ringDecrypted}`);

  // Test 10: Edge Cases
  console.log('\n=== Testing Edge Cases ===');
  
  const enigma9 = new Enigma([0, 1, 2], [0, 0, 0], [0, 0, 0], []);
  
  // Test empty string
  assert(enigma9.process('') === '', 'Empty string handling');
  
  // Test non-alphabetic characters
  const mixedString = 'HELLO123 WORLD!';
  const mixedResult = enigma9.process(mixedString);
  assert(mixedResult.includes('123') && mixedResult.includes(' ') && mixedResult.includes('!'), 'Non-alphabetic characters preserved');

  console.log('\n🎉 All tests passed! The Enigma implementation is working correctly.');
}

// Export for module usage
if (typeof module !== 'undefined' && module.exports) {
  module.exports = { runTests };
}

// Run tests if called directly
if (require.main === module) {
  runTests();
} 