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



.global day01_part2_imp_run


// Calculate how many times the sum of 3 values in the array is higher than the previous sum of 3 values
// uint32_t day01_part2_imp_run(const uint32_t *input, uint32_t length);
day01_part2_imp_run:
    array .req r0
    arrayEnd .req r1
    result .req r2
    sumPrev .req r3
    sumNow .req r4
    temp .req r5

    // r0 = array pointer
    // r1 = length
    // Save registers
    push {r3-r7}

    // sanity check to exit if array length <= 3 (not really necessary, but still)
    ldr result, =0
    cmp r1, #3
    ble 3f

    // Increment r1 to be the address one past the end of the array (our stopping point) (minus 2 * 4 = 8 since we have weird positioning)
    ldr r2, =4
    mul arrayEnd, r2, arrayEnd
    add arrayEnd, array, arrayEnd
    sub arrayEnd, arrayEnd, #8
    // Store result in r2 for now
    ldr result, =0

    // Load first 3 values and sum them
    ldr sumPrev, [array, #0]

    ldr temp, [array, #4]
    mov sumNow, temp
    add sumPrev, sumPrev, temp

    ldr temp, [array, #8]
    add sumNow, sumNow, temp
    add sumPrev, sumPrev, temp

    add array, array, #4
    // At this point array points to the 2nd element (index 1), sumPrev is done and sumNow has its first 2 elements
1:
    // Load new value into temp
    ldr temp, [array, #8]
    // Add it to sumNow
    add sumNow, sumNow, temp

    // Check if current sum is higher than the previous one
    cmp sumNow, sumPrev
    ble 2f
    // Add 1 to result
    add result, result, #1
2:
    // Store current sum as previous
    mov sumPrev, sumNow
    // Subtract 'first' value from sum, which is at the array pointer
    ldr temp, [array, #0]
    sub sumPrev, sumPrev, temp

    // Check if we hit the end of the array
    add array, array, #4
    cmp array, arrayEnd
    blt 1b

3:
    // Restore registers
    pop {r3-r7}
    // Move result to r0
    add r0, result, #0

    bx lr
