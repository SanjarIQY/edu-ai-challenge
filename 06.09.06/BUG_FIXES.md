# Enigma Machine - Bug Fixes

## Bug #1: Missing Final Plugboard Application
**Location**: `encryptChar()` method  
**Issue**: Plugboard only applied once instead of twice  

**How I Found It**: While analyzing the encryption flow, I noticed the plugboard was only called once at the beginning but not at the end. In a real Enigma machine, the signal passes through the plugboard twice - once going in and once coming out.

**How I Fixed It**: Added a second plugboard call after all rotor processing is complete:
```javascript
// Added this line at the end of encryptChar():
c = plugboardSwap(c, this.plugboardPairs);
```

**Verification**: Tested with plugboard pairs and confirmed encryption/decryption symmetry now works correctly.

## Bug #2: Incorrect Double-Stepping Mechanism
**Location**: `stepRotors()` method  
**Issue**: Middle rotor didn't double-step correctly  

**How I Found It**: Studied the original stepping logic and compared it to historical Enigma documentation. The original code didn't capture the middle rotor's position BEFORE stepping, causing incorrect double-stepping behavior.

**How I Fixed It**: Implemented proper double-stepping by checking the middle rotor's notch position before any stepping occurs:
```javascript
stepRotors() {
  const doubleStep = this.rotors[1].atNotch(); // Check BEFORE stepping
  if (this.rotors[2].atNotch()) this.rotors[1].step();
  if (doubleStep) {
    this.rotors[0].step();
    this.rotors[1].step(); // Middle rotor steps again
  }
  this.rotors[2].step();
}
```

**Verification**: Created test case with rotors positioned near double-stepping condition and confirmed correct behavior.

## Bug #3: Missing Input Validation
**Location**: Constructor  
**Issue**: No validation for invalid positions/settings  

**How I Found It**: While creating test cases, I noticed the constructor would accept invalid inputs (like position 26 or negative numbers) without throwing errors.

**How I Fixed It**: Added comprehensive validation in the constructor to check array lengths and value ranges:
```javascript
// Added validation checks in constructor:
if (rotorIDs.length !== 3 || rotorPositions.length !== 3 || ringSettings.length !== 3) {
  throw new Error('Must specify exactly 3 rotors, positions, and ring settings');
}

for (let i = 0; i < 3; i++) {
  if (rotorPositions[i] < 0 || rotorPositions[i] > 25) {
    throw new Error(`Rotor position ${i} must be between 0 and 25`);
  }
  if (ringSettings[i] < 0 || ringSettings[i] > 25) {
    throw new Error(`Ring setting ${i} must be between 0 and 25`);
  }
}
```

**Verification**: Added test cases that attempt to create Enigma instances with invalid inputs and confirmed proper error handling. 