cmake_minimum_required(VERSION 3.13)

include(pico_sdk_import.cmake)

project(pico_project_test)

pico_sdk_init()

add_executable(hello_world
    hello_world.c
    day01.c
    day01_data.c
    day01_imp.s
    day02_data.c
    day02_imp.s
)

target_link_libraries(hello_world pico_stdlib)

pico_enable_stdio_usb(hello_world 1)
pico_enable_stdio_uart(hello_world 0)

pico_add_extra_outputs(hello_world)
