#include <stdio.h>
#include <string.h>
#include "pico/stdlib.h"
#include "commands.h"
#include "day01.h"

#define MAX_COMMAND_ENTRIES   (40)

#define LED_PIN (25)

static CommandEntry_t m_commandEntries[MAX_COMMAND_ENTRIES];
static uint8_t m_nrOfCommandEntries;

static void LedOnCommand(const char *commandString, size_t commandStringLength);
static void LedOffCommand(const char *commandString, size_t commandStringLength);
static void HelpCommand(const char *commandString, size_t commandStringLength);

int main()
{
  stdio_init_all();

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

  Commands_Add(&helpEntry);
  Commands_Add(&ledOnEntry);
  Commands_Add(&ledOffEntry);

  day01_initialize();

  char readBuffer[50];
  memset(readBuffer, 0, sizeof(readBuffer));
  uint8_t index = 0;

  while(true)
  {
    int chr = getchar_timeout_us(100000ul);
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
    }

    gpio_xor_mask(1 << LED_PIN);
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
