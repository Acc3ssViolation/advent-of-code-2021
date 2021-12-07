#include "arduino/mega.h"
#include "arduino/gpio.h"
#include "arduino/lcd1602.h"
#include "arduino/servo.h"
#include "arduino/button.h"
#include "arduino/dht11.h"
#include "arduino/vs1838b.h"
#include "arduino/serial.h"
#include "sysb/log.h"
#include "sysb/timer.h"
#include "sysb/buffers.h"
#include "sysb/commands.h"
#include "sysb/serial_console.h"
#include "util/delay.h"
#include "sysb/events.h"
#include "application.h"
#include <string.h>
#include <stdio.h>
#include <ctype.h>
#include <avr/interrupt.h>
#include <avr/io.h>

static volatile uint16_t m_ticks;

int main(void)
{
  arduino_board_init();
  commands_initialize();
  log_initialize();
  timer_initialize();
  serial_console_initialize();
  
  // Set up timer 2 as a systick timer of 1 ms
  // No outputs, mode 2 (CTC) to clear on capture compare, we get an OC2A interrupt every time the counter gets to OCR2A
  TCCR2A = (1 << WGM21);
  TIMSK2 = (1 << OCIE2A);
  OCR2A = 250;
  TCCR2B = 4;   // Timer 2 uses different scaling values, so can't use the macro

  application_initialize();

  uint16_t previousTicks = m_ticks;
  sei();

  while (1)
  {
    event_flags_t flags = events_get_and_clear_flags();
    if (flags & EVENT_FLAG_TICK)
    {
      uint16_t currentTicks = m_ticks;
      timer_tick(currentTicks - previousTicks);
      previousTicks = currentTicks;
    }

    message_t message;

    if (event_get_message(&message))
    {
      switch (message.id)
      {
        case MESSAGE_ID_TEST:
        {
          arduino_toggle_led();
          break;
        }
      }
    }

    serial_console_poll();
  }
}

ISR(TIMER2_COMPA_vect)
{
  ++m_ticks;
  events_set_flags(EVENT_FLAG_TICK);
}