#include <stdint.h>
#include "day01.h"
#include "commands.h"

static void Day01Command(const char *commandString, size_t commandStringLength);
static void Day02Command(const char *commandString, size_t commandStringLength);

extern uint32_t day01_imp_run(const uint32_t *input, uint32_t length);
extern uint32_t day01_part2_imp_run(const uint32_t *input, uint32_t length);

void Day01_Initialize(void)
{
    const CommandEntry_t command1 = {
        .command = "day01",
        .handler = Day01Command,
    };

    const CommandEntry_t command2 = {
        .command = "day02",
        .handler = Day02Command,
    };

    Commands_Add(&command1);
    Commands_Add(&command2);
}

static void Day01Command(const char *commandString, size_t commandStringLength)
{
    printf("Beginning day01...\n");

    uint32_t inputLength;
    uint32_t *input = Day01_GetInput(&inputLength);
    uint32_t result = day01_imp_run(input, inputLength);

    printf("%u samples -> %u\n", inputLength, result);

    result = day01_part2_imp_run(input, inputLength);

    printf("%u samples -> %u\n", inputLength, result);

    printf("Finished day01...\n");
}

static void Day02Command(const char *commandString, size_t commandStringLength)
{
    printf("Beginning day02...\n");


    printf("Finished day02...\n");
}