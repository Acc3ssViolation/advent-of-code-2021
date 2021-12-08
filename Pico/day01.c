#include <stdint.h>
#include "day01.h"
#include "commands.h"

static void Day01Command(const char *commandString, size_t commandStringLength);

extern uint32_t day01_imp_run(const uint32_t *input, uint32_t length);

void Day01_Initialize(void)
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

    uint32_t inputLength;
    uint32_t *input = Day01_GetInput(&inputLength);
    uint32_t result = day01_imp_run(input, inputLength);

    printf("%u samples -> %u\n", inputLength, result);

    printf("Finished day01...\n");
}