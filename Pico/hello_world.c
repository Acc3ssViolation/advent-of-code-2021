#include <stdio.h>
#include <string.h>
#include "pico/stdlib.h"
#include "hardware/exception.h"
#include "hardware/watchdog.h"
#include "pico/platform.h"
#include "commands.h"
#include "day01.h"

#define MAX_COMMAND_ENTRIES   (40)

#define LED_PIN (25)

#define RESET_BY_HARDFAULT    (0xDEADBEEF)

static CommandEntry_t m_commandEntries[MAX_COMMAND_ENTRIES];
static uint8_t m_nrOfCommandEntries;
static bool m_resetByWatchdog;
static bool m_resetByHardFault;

static uint32_t __uninitialized_ram() m_resetByHardFaultCode;

static void HardfaultHandler(void);

static void LedOnCommand(const char *commandString, size_t commandStringLength);
static void LedOffCommand(const char *commandString, size_t commandStringLength);
static void HelpCommand(const char *commandString, size_t commandStringLength);
static void InfoCommand(const char *commandString, size_t commandStringLength);
static void ResetCommand(const char *commandString, size_t commandStringLength);

int main()
{
  stdio_init_all();

  m_resetByHardFault = (m_resetByHardFaultCode == RESET_BY_HARDFAULT);
  m_resetByHardFaultCode = 0;
  m_resetByWatchdog = watchdog_caused_reboot();

  exception_handler_t originalHandler = exception_set_exclusive_handler(HARDFAULT_EXCEPTION, HardfaultHandler);

  watchdog_enable(1000, true);

  gpio_init(LED_PIN);
  gpio_set_dir(LED_PIN, GPIO_OUT);

  const CommandEntry_t ledOnEntry = {
      .command = "led on",
      .handler = LedOnCommand,
  };

  const CommandEntry_t ledOffEntry = {
      .command = "led off",
      .handler = LedOffCommand,
  };

  const CommandEntry_t helpEntry = {
      .command = "help",
      .handler = HelpCommand,
  };

  const CommandEntry_t infoEntry = {
      .command = "info",
      .handler = InfoCommand,
  };

  const CommandEntry_t resetEntry = {
      .command = "reset",
      .handler = ResetCommand,
  };

  Commands_Add(&helpEntry);
  Commands_Add(&infoEntry);
  Commands_Add(&resetEntry);
  Commands_Add(&ledOnEntry);
  Commands_Add(&ledOffEntry);

  Day01_Initialize();

  char readBuffer[50];
  memset(readBuffer, 0, sizeof(readBuffer));
  uint8_t index = 0;

  while(true)
  {
    if (false == stdio_usb_connected())
    {
      gpio_xor_mask(1 << LED_PIN);
      watchdog_update();
      sleep_ms(100);
      continue;
    }

    int chr = getchar_timeout_us(10000ul);

    watchdog_update();

    if (chr != PICO_ERROR_TIMEOUT)
    {
      if (chr == 0x7F || chr == 0x08)
      {
        if (index > 0)
        {
          index--;

          // Move back, print space, move back again
          putchar(0x08);
          putchar(0x20);
          putchar(0x08);
        }
      }
      else if (chr == 0x0D)
      {
        if (index > 0)
        {
          printf("\n");
          readBuffer[index] = 0x00;

          CommandHandler_t handler = NULL;

          for (uint8_t i = 0; i < m_nrOfCommandEntries; i++)
          {
            if (strncmp(m_commandEntries[i].command, readBuffer, sizeof(readBuffer)) == 0)
            {
              handler = m_commandEntries[i].handler;
              break;
            }
          }

          if (handler != NULL)
          {
            handler(readBuffer, index + 1);
          }
          else
          {
            printf("Unknown command %s", readBuffer);
          }

          index = 0;
        }
        printf("\n>");
      }
      else if (index < sizeof(readBuffer) - 1)
      {
        readBuffer[index] = chr;
        putchar(chr);

        index++;
      }

      gpio_xor_mask(1 << LED_PIN);
    }
  }
  return 0;
}

bool Commands_Add(const CommandEntry_t* command)
{
  if (m_nrOfCommandEntries < MAX_COMMAND_ENTRIES)
  {
    m_commandEntries[m_nrOfCommandEntries++] = *command;
    return true;
  }
  return false;
}

static void HardfaultHandler(void)
{
  // Log reason
  m_resetByHardFaultCode = RESET_BY_HARDFAULT;
  // Wait for watchdog to reset us
  while(1) {}
}

static void HelpCommand(const char *commandString, size_t commandStringLength)
{
  for (int i = 0; i < m_nrOfCommandEntries; i++)
  {
    printf("%s\n", m_commandEntries[i].command);
  }
}

static void LedOnCommand(const char *commandString, size_t commandStringLength)
{
  gpio_put(LED_PIN, 1);
  printf("LED on");
}

static void LedOffCommand(const char *commandString, size_t commandStringLength)
{
  gpio_put(LED_PIN, 0);
  printf("LED off");
}

static void InfoCommand(const char *commandString, size_t commandStringLength)
{
  printf("Reset by watchdog: %s\n", m_resetByWatchdog ? "yes" : "no");
  printf("Reset by hard fault: %s", m_resetByHardFault ? "yes" : "no");
}

static void ResetCommand(const char *commandString, size_t commandStringLength)
{
  printf("Resetting device...\n");
  while (1)
  {
    // Wait for watchdog to reset us
  }
}