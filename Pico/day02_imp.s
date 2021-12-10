//extern uint32_t day02_part1(const char **input, uint32_t length);
.global day02_part1

day02_part1:
    // TODO: Save registers
    push {r4-r7, lr}

    // Parameters
    input .req r0
    length .req r1
    string .req r2
    number .req r3
    firstByte .req r4
    depth .req r5
    horizontal .req r6

    // Result init
    ldr depth, =0
    ldr horizontal, =0

    // The ol' array end check setup
    // It is an array of pointers, so the values are 4 bytes in size
    ldr string, =4
    mul length, string, length
    add length, input, length
1:
    // Load string pointer
    ldr string, [input]
    // Find number in this command string
    push {r0-r2}
    mov r0, string
    bl day02_get_number
    mov number, r0
    pop {r0-r2}
    // Determine which operation to perform by looking at the first byte of the string
    ldrb firstByte, [string]
    cmp firstByte, #0x66 // 'f'
    beq 2f
    cmp firstByte, #0x75 // 'u'
    beq 3f
    // Just assume it's down since our input is perfect
    // Increment depth
    add depth, depth, number
    b 4f
2:
    // Increment forward
    add horizontal, horizontal, number
    b 4f
3:
    // Decrement depth
    sub depth, depth, number
4:
    // Increment input to move to the next string
    add input, input, #4
    // Check if we hit the end of the array
    cmp input, length
    blt 1b

    // Done, calculate the result which is horizontal * depth
    mul depth, depth, horizontal
    // Move to result register
    mov r0, depth

    // Pop lr from stack back into pc
    pop {r4-r7, pc}

// Extract a decimal digit after a space
// uint32_t day02_get_number(const char* input)
.global day02_get_number
day02_get_number:
    // Parameters
    input .req r0
    temp .req r1
    result .req r2
    ten .req r3

    // Init result
    ldr result, =0
    ldr ten, =10

    // Find space
1:
    ldrb temp, [input]
    // Check for end of string
    cmp temp, #0
    beq 2f
    // Move up and loop until we hit a space
    add input, input, #1
    cmp temp, #0x20 // ' '
    bne 1b
    // We are now one past the space
1:
    ldrb temp, [input]
    // Check for end of string and character range (0 - 9)
    cmp temp, #0
    beq 2f
    cmp temp, #0x30 // '0'
    blt 2f
    cmp temp, #0x39 // '9'
    bgt 2f
    // Convert char to actual digit value
    sub temp, temp, #0x30
    // Multiply current result by 10, then add the new digit
    mul result, ten, result
    add result, temp, result
    // Move to next char
    add input, input, #1
    b 1b
2:
    mov r0, result
    bx lr
