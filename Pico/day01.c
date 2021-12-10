#include <stdint.h>
#include "day01.h"
#include "commands.h"

static void Day01Command(const char *commandString, size_t commandStringLength);
static void Day02Command(const char *commandString, size_t commandStringLength);

extern uint32_t day01_imp_run(const uint32_t *input, uint32_t length);
extern uint32_t day01_part2_imp_run(const uint32_t *input, uint32_t length);

extern uint32_t day02_part1(const char **input, uint32_t length);
extern uint32_t day02_part2(const char **input, uint32_t length);


extern uint32_t day02_get_number(const char* input);

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

    // const char* a = "forward 20";
    // const char* b = "up 9212";
    // const char* c = "down 2";

    // uint32_t na = day02_get_number(a);
    // uint32_t nb = day02_get_number(b);
    // uint32_t nc = day02_get_number(c);

    // printf("'%s' -> %u\n", a, na);
    // printf("'%s' -> %u\n", b, nb);
    // printf("'%s' -> %u\n", c, nc);
    uint32_t inputLength;
    const char **input = Day02_GetInput(&inputLength);
    uint32_t result = day02_part1(input, inputLength);
    printf("%u commands -> %u\n", inputLength, result);

    result = day02_part2(input, inputLength);
    printf("%u commands -> %u\n", inputLength, result);

    printf("Finished day02...\n");
}