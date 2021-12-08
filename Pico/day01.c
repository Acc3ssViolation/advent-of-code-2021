#include <stdint.h>
#include "day01.h"
#include "commands.h"

static void Day01Command(const char *commandString, size_t commandStringLength);

extern uint32_t day01_imp_run(uint32_t a, uint32_t b);

void day01_initialize(void)
{
    const CommandEntry_t command = {
        .command = "day01",
        .handler = Day01Command,
    };

    Commands_Add(&command);
}

static void Day01Command(const char *commandString, size_t commandStringLength)
{
    printf("Beginning day01...\n");

    uint32_t a = 2012;
    uint32_t b = 900000;
    uint32_t result = day01_imp_run(a, b);

    printf("%u + %u = %u\n", a, b, result);

    printf("Finished day01...\n");
}