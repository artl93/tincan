tincan
======

IMPORTANT NOTE: The engine is not currently in a working state. It is a port from XNA and not ready for general use - yet. 

Experimental audio graph in C#. It's for my own learning as much as anything else. 

This is a C#, originally XNA-based audio graph. It was designed to solve timing issues in building a simple music application for Windows Phone 7 (hence using XNA). Each AudioNode gets to write to an input / output buffer. The same buffer is used for both input and output to minimize the need for allocations and copies in the audio graph, which would negatively impact performance and GC pressure. 

The core of the audio engine uses XNA-style polled updates to the audio output buffer. 
