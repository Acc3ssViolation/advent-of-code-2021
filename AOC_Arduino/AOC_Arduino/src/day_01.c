/*
* day_01.c
*
* Created: 07/12/2021 20:08:07
*  Author: Wouter
*/

#include "day_01.h"
#include "day_01_data.h"
#include "sysb/commands.h"

extern uint16_t day_01_entry(const uint16_t *data, uint16_t dataLength);

static void run_command(const char *arguments, uint8_t length, const command_functions_t* output);

static const command_t m_command = {
  .prefix = "DAY01",
  .summary = "Run this days assignment",
  .handler = run_command
};

void day01_initialize()
{
  commands_register(&m_command);
}

static void run_command(const char *arguments, uint8_t length, const command_functions_t* output)
{
  uint16_t result = day_01_entry(&day_01_input[0], sizeof(day_01_input));
  output->writeln_format("%u", result);
}