
/*
 * shared.s
 *
 * Created: 07/12/2021 20:30:39
 *  Author: Wouter
 */ 


.macro LOAD_16 l, h, value      ; L, H, VALUE
  ldi \l, LOW(\value)
  ldi \h, HIGH(\value)
.endm

.macro ADD_16       ; L1, H1 + L2, H2 -> L1, H1
  add @0, @2
  adc @1, @3
.endm
