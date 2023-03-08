# Brie 🧀
A lil console program for viewing + killing listening ports inspired by [gruyere](https://github.com/savannahostrowski/gruyere).

Brie uses [Spectre.Console](https://github.com/spectreconsole/spectre.console) to look purty and [dein ToolBox](https://github.com/deinsoftware/toolbox) to run shell commands. 

Similar to gruyere, Brie wraps `lsof -i -P -n -sTCP:LISTEN` to list all the various processes on your machine. 

## Requirements to build:
dotnet 6.0

## Tested on:
Currently only tested on macOS 13.2.1 M1
