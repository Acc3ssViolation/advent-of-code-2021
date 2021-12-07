
/*
 * day_01_asm.s
 *
 * Created: 07/12/2021 20:13:36
 *  Author: Wouter
 */ 

// The assembler decided that include paths are not a thing, so we'll just hardcode them
.include "C:/Program Files (x86)/Atmel/Studio/7.0/Packs/atmel/ATmega_DFP/1.3.300/avrasm/inc/m2560def.inc"
//.include "../include/shared.s"

// extern uint16_t day_01_entry(const uint16_t *data, uint16_t dataLength);
.global day_01_entry
day_01_entry:
; Push result 666 on stack
;LOAD_16(r24, r25, 666)
ldi r24, LOW(666)
ldi r25, HIGH(666)
ret