using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace StuntJumpsFiveM
{
    public class StuntJumps : BaseScript
    {
        public StuntJumps()
        {

            // Setting up stats to make sure if the resource is restarted we DON'T re-add the stunt jumps, because the pool size is only 64, 
            // and we add 50 stunt jumps, restarting the resource would break the client side stunt jumps pool size.
            bool jumpsSetup = false;
            StatGetBool((uint)GetHashKey("MP1_DEFAULT_STATS_SET"), ref jumpsSetup, -1);

            foreach (var stuntJump in JsonConvert.DeserializeObject<Dictionary<string, List<dynamic>>>(Properties.Resources.stuntjumps))
            {
                if (stuntJump.Key == "angledStuntJumps")
                {
                    foreach (var jump in stuntJump.Value)
                    {
                        // don't add them if they're already previously setup
                        if (!jumpsSetup)
                        {
                            AddStuntJumpAngled(
                                (float)jump["jumpPos"]["start"]["X"],
                                (float)jump["jumpPos"]["start"]["Y"],
                                (float)jump["jumpPos"]["start"]["Z"],
                                (float)jump["jumpPos"]["end"]["X"],
                                (float)jump["jumpPos"]["end"]["Y"],
                                (float)jump["jumpPos"]["end"]["Z"],
                                (float)jump["jumpPos"]["radius"],
                                (float)jump["landingPos"]["start"]["X"],
                                (float)jump["landingPos"]["start"]["Y"],
                                (float)jump["landingPos"]["start"]["Z"],
                                (float)jump["landingPos"]["end"]["X"],
                                (float)jump["landingPos"]["end"]["Y"],
                                (float)jump["landingPos"]["end"]["Z"],
                                (float)jump["landingPos"]["radius"],
                                (float)jump["camPos"]["X"],
                                (float)jump["camPos"]["Y"],
                                (float)jump["camPos"]["Z"],
                                150,
                                0,
                                0
                            );
                        }
                        // add blip if enabled
                        if (GetConvar("enable_stunt_jump_blips", "false").ToLower() == "true")
                        {
                            int blip = AddBlipForCoord((float)jump["jumpPos"]["start"]["X"], (float)jump["jumpPos"]["start"]["Y"], (float)jump["jumpPos"]["start"]["Z"]);
                            SetBlipColour(blip, 0);
                            BeginTextCommandSetBlipName("STRING");
                            AddTextComponentSubstringPlayerName("Stunt Jump");
                            EndTextCommandSetBlipName(blip);
                            SetBlipAsShortRange(blip, true);
                        }

                    }
                }
                else
                {
                    foreach (var jump in stuntJump.Value)
                    {
                        // don't add them if they're already previously setup
                        if (!jumpsSetup)
                        {
                            AddStuntJump(
                                (float)jump["jumpPos"]["start"]["X"],
                                (float)jump["jumpPos"]["start"]["Y"],
                                (float)jump["jumpPos"]["start"]["Z"],
                                (float)jump["jumpPos"]["end"]["X"],
                                (float)jump["jumpPos"]["end"]["Y"],
                                (float)jump["jumpPos"]["end"]["Z"],
                                (float)jump["landingPos"]["start"]["X"],
                                (float)jump["landingPos"]["start"]["Y"],
                                (float)jump["landingPos"]["start"]["Z"],
                                (float)jump["landingPos"]["end"]["X"],
                                (float)jump["landingPos"]["end"]["Y"],
                                (float)jump["landingPos"]["end"]["Z"],
                                (float)jump["camPos"]["X"],
                                (float)jump["camPos"]["Y"],
                                (float)jump["camPos"]["Z"],
                                150,
                                0,
                                0
                            );
                        }
                        // add blip if enabled
                        if (GetConvar("enable_stunt_jump_blips", "false").ToLower() == "true")
                        {
                            int blip = AddBlipForCoord((float)jump["jumpPos"]["start"]["X"], (float)jump["jumpPos"]["start"]["Y"], (float)jump["jumpPos"]["start"]["Z"]);
                            SetBlipColour(blip, 0);
                            BeginTextCommandSetBlipName("STRING");
                            AddTextComponentSubstringPlayerName("Stunt Jump");
                            EndTextCommandSetBlipName(blip);
                            SetBlipAsShortRange(blip, true);
                        }
                    }
                }
            }

            StatSetBool((uint)GetHashKey("MP1_DEFAULT_STATS_SET"), true, true);

            EventHandlers.Add("onResourceStop", new Action<string>((resname) =>
            {
                if (resname == GetCurrentResourceName())
                {
                    // reset the stat if the resource is being stopped because the player disconnected from the server
                    // otherwise do nothing because the stunt jumps probably still exist.
                    // This is a very hacky method, and does cause for some problems, but none that are too critical.
                    // Just some 'visual' issues to do with keeping track of completed jumps. 
                    // (things like 99 remaining stunt jumps etc will appear in banner message when using a jump after re-joining a server without game reboot)
                    if (!NetworkIsSessionActive())
                    {
                        StatSetBool((uint)GetHashKey("MP1_DEFAULT_STATS_SET"), false, true);
                        Debug.WriteLine($"[{GetCurrentResourceName()}] Resetting stunt jumps stat to allow them to be recreated on the next server join.");
                    }
                }
            }));
        }
    }
}
