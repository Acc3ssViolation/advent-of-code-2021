#ifndef COMMANDS_H
#define COMMANDS_H

#include <stdio.h>
#include <stdbool.h>

typedef void (*CommandHandler_t)(const char *commandString, size_t commandStringLength);

typedef struct
{
  const char* command;
  CommandHandler_t handler;
} CommandEntry_t;

bool Commands_Add(const CommandEntry_t* command);

#endif