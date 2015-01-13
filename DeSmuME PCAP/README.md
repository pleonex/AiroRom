Fork of DeSmuME to dump the wireless communication packets
into PCAP files.

Each time the game authenticate (activate Wi-Fi) a new file is created and
closed when it ends.

It also includes a disabled RC4 dumper in the *debug.cpp* file. It needs to work
the RC4 start and end address.