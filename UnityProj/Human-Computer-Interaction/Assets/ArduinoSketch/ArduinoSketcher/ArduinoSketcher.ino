#include "Ardunity.h"
#include "AnalogInput.h"
#include "DigitalInput.h"

AnalogInput aInput1(1, A0);
DigitalInput dInput0(0, 0, true);

void setup()
{
  ArdunityApp.attachController((ArdunityController*)&aInput1);
  ArdunityApp.attachController((ArdunityController*)&dInput0);
  ArdunityApp.resolution(256, 1024);
  ArdunityApp.timeout(5000);
  ArdunityApp.begin(9600);
}

void loop()
{
  ArdunityApp.process();
}
