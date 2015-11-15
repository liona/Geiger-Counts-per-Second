# Geiger-Counts-per-Second

This application takes an input of 1s and 0s from a port, outputs the start time, and then the number of counts per second for the duration specified. It was originally written to interact with Sparkfun's geiger counter (https://www.sparkfun.com/products/11345). The executable can be accesssed directly by navigating to Geiger-Counts-per-Second/GeigerCounts/bin/Debug/GeigerCounts.exe.  

Sparkfun also has a v13 firmware which you can load onto the microprocessor if you have an external programmer.  Mine came with v12 which outputs random 1s and 0s.

The program looks for existing comports (therefore the device must be plugged in before you start the program, otherwise you have to open and close the program after you plug and power up your device) and looks for 0's and 1's at which point it increments a counter.

That counter value is output both to a specified file (Save To) and to the output window each second.  This is controlled by a timer tick whose interval is 1s.  At each tick the "count" is output to the two locations and then set to 0.  

If the file name you specify does not already exist, then the file will be created.  If the file does exist, the data will be appended to what exists, ie whatever is already in the file will not be overwritten.

Please improve. 
