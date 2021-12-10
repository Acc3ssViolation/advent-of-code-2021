.cpu cortex-m0

//extern uint32_t day03_part1(const char **input, uint32_t length);
.global day03_part1

day03_part1:
    // Save registers
    push {r4-r7, lr}

    // Parameters
    input .req r0
    length .req r1
    string .req r2

    // Calculate and store threshold on stack for later
    lsr r3, length, #1
    push {r3}

    // The ol' array end check setup
    // It is an array of pointers, so the values are 4 bytes in size
    lsl length, length, #2
    add length, input, length

    // Clear array to 0
    push {r0-r2}
    ldr r0, =day03_part1_counts
    ldr r1, =12
    lsl r1, r1, #2
    add r1, r1, r0
    eor r2, r2, r2
1:
    str r2, [r0]
    add r0, r0, #4
    cmp r0, r1
    blt 1b
    pop {r0-r2}
1:
    // Load string pointer
    ldr string, [input]
    // Find number in this command string
    push {r0-r2}

    mov r0, string
    bl day03_get_number
    // number is now in r0
    // Set up parameters
    ldr r1, =day03_part1_counts
    ldr r2, =12
    // Process this number
    bl day03_process_number

    pop {r0-r2}

    // Increment input to move to the next string
    add input, input, #4
    // Check if we hit the end of the array
    cmp input, length
    blt 1b

    // Done!
    // Set up parameters
    ldr r0, =day03_part1_counts
    ldr r1, =12
    // This pops the initial push {r3} into r2
    pop {r2}
    // Calculate result
    bl day03_get_result

    // Pop lr from stack back into pc
    pop {r4-r7, pc}

    .unreq input
    .unreq length
    .unreq string

.data 
// Arrays for day03_part1
.global day03_part1_counts
day03_part1_counts:
    .word 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
.text


// uint32_t day03_get_result(uint32_t *counts, uint32_t length, uint32_t threshold)
.global day03_get_result
day03_get_result:
    counts .req r0
    length .req r1
    threshold .req r2
    result .req r3
    temp .req r4

    push {r4}

    // counts an array of words, so the values are 4 bytes in size
    // We want to run from the back to the front, so we use length
    // as the array pointer. Put it at the last entry:
    lsl length, length, #2
    add length, counts, length
    sub length, length, #4

    // Init result
    ldr result, =0

    // index 0: lsb, 11: msb
1:
    // if counts[i] >= threshold
    ldr temp, [length]
    cmp temp, threshold
    blt 2f
    b 3f
2:
    // counts[i] < threshold, temp = 0
    ldr temp, =0
    b 4f    
3:
    // counts[i] >= threshold, temp = 0
    ldr temp, =1
4:
    // result <<= 1
    lsl result, result, #1
    // result |= temp
    orr result, result, temp
    // length--
    sub length, length, #4
    // Are we done?
    cmp length, counts
    bge 1b

    // We got gamma, so now we need epsilon
    ldr r0, =#0xFFF
    eor r0, result, r0
    mul r0, result, r0

    // Restore and exit
    pop {r4}
    bx lr

    .unreq counts
    .unreq length
    .unreq threshold
    .unreq result
    .unreq temp

// void day03_process_number(uint32_t number, uint32_t *counts, uint32_t length)
.global day03_process_number
day03_process_number:
    number .req r0
    counts .req r1
    length .req r2
    temp .req r3

    // counts is an array of words, so the values are 4 bytes in size
    // store the first word address past the array in length
    ldr r3, =4
    mul length, r3, length
    add length, counts, length

    // index 0: lsb, 11: msb
1:
    ldr temp, =1
    // Is LSB set?
    and temp, number, temp
    // Jump if Zero flag is set (e.g. bit was not set)
    beq 2f
    // Increment count
    ldr temp, [counts]
    add temp, temp, #1
    str temp, [counts]
2:
    // number >>= 1
    lsr number, number, #1
    // counts++
    add counts, counts, #4
    // Are we done?
    cmp counts, length
    blt 1b
    // Yes
    bx lr

    .unreq counts
    .unreq length
    .unreq number
    .unreq temp

// Binary string parsing
// uint32_t day03_get_number(const char* input)
.global day03_get_number
day03_get_number:
    // Parameters
    input .req r0
    temp .req r1
    result .req r2

    // Init result
    ldr result, =0

1:
    ldrb temp, [input]
    // Check for end of string
    cmp temp, #0
    beq 2f
    // Convert char to actual digit value. We only have '0' and '1', so just subtract 0x30
    sub temp, temp, #0x30
    // Shift up existing result, then add the new digit
    lsl result, result, #1
    orr result, result, temp
    // Move to next char
    add input, input, #1
    b 1b
2:
    mov r0, result
    bx lr
