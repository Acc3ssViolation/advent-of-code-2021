/*
 * application.c
 *
 * Created: 07/12/2021 19:59:03
 *  Author: Wouter
 */ 

#include "application.h"
#include "sysb/commands.h"
#include "day_01.h"

#define APPLICATION_VERSION_STRING    "0.1"

static void version_command(const char *arguments, uint8_t length, const command_functions_t* output);

static const command_t m_versionCommand = {
  .prefix = "VERSION",
  .summary = "Shows application version info",
  .handler = version_command
};

void application_initialize()
{
  commands_register(&m_versionCommand);

  day01_initialize();
}

static void version_command(const char *arguments, uint8_t length, const command_functions_t* output)
{
  output->writeln("VERSION: " APPLICATION_VERSION_STRING);
  output->writeln("DATE: " __DATE__);
  output->writeln("TIME: " __TIME__);
}