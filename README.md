# Stunt Jumps FiveM
*All stock stunt jumps from GTA V in FiveM.*

----------

This simple resource adds all stock GTA V stunt jumps into your server. Stats are tracked by the game itself, they _will_ reset once you close your game and rejoin.

## Install
1. Download the latest release from the releases page.
2. Copy the `StuntJumpsFiveM.net.dll`, `Newtonsoft.Json.dll` and `__resource.lua` files from the download into a new folder in your resources folder, you can call it whatever you like.
3. Add `start <name>` to the server.cfg, making sure to replace `<name>` with the name you gave the new folder you just made.
4. If you want to enable blips for all the stunt jumps, add `setr enable_stunt_jump_blips true` _ABOVE_ the `start <name>` line from step 3.


## Example
https://streamable.com/dhyc3
*(in case embed is messed up: https://streamable.com/dhyc3)*

## Dev notes
You should not try to add lots more, the game can only handle up to 64 stunt jumps before shitting itself. This resource adds 50 stunt jumps and sets the `MP1_DEFAULT_STATS_SET` stat to `true` whenever it has added them. This is used to make sure they are not added again in the same game session in case the resource is rebooted for whatever reason. So please don't touch that stat value cause you'll most likely end up breaking people's games. If you want to add your own stunt jumps, just create a fork of this project and edit the `stuntjumps.json` file embedded in the source files.