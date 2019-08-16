#pragma once
#include <vector>
#include <string>

extern signed int selection;
extern int scene;

void MainMenuLoop();
void printItems(const std::vector<std::string> &items, std::string menuTitle);