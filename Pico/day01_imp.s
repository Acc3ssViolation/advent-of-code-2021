.global day01_imp_run

// Calculate how many times a value in the array is higher than the previous value (n > n - 1)
// uint32_t day01_imp_run(const uint32_t *input, uint32_t length);
day01_imp_run:
    // r0 = array pointer
    // r1 = length
    // Save registers
    push {r3, r4}

    // Increment r1 to be the address one past the end of the array (our stopping point)
    ldr r2, =4
    mul r1, r2, r1
    add r1, r0, r1
    // Store result in r2 for now
    ldr r2, =0

    // Store 'previous value' in r3
    ldr r3, [r0]
    // Increment r0 so we start at element index 1
    add r0, r0, #4
1:
    // Load current into r4
    ldr r4, [r0]
    // Check if r4 is higher than r3 or not
    cmp r4, r3
    ble 2f
    // Add 1 to result
    add r2, r2, #1
2:
    // Move current to previous register
    add r3, r4, #0
    // Increment r0 to move to the next one
    add r0, r0, #4
    // Check if we hit the end of the array
    cmp r0, r1
    blt 1b

    // Restore registers
    pop {r3, r4}
    // Move result to r0
    add r0, r2, #0

    bx lr
