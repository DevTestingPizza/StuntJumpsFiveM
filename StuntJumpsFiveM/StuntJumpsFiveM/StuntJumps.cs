using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace StuntJumpsFiveM
{
    public class StuntJumps : BaseScript
    {

        public List<int> stuntJumps = new List<int>();
        public List<int> blips = new List<int>();

        public StuntJumps()
        {
            // Blip label text
            AddTextEntry("RADAR_STUNT_JUMP", "Stunt Jump");

            // Enable blips if convar is set.
            if (GetConvar("enable_stunt_jump_blips", "false").ToLower() == "true")
            {
                EnableBlips();
            }

            // Enable stunt jumps by default, but they can be disabled so that they can only be triggered via a command.
            if (GetConvar("enable_stunt_jumps", "true").ToLower() == "true")
            {
                EnableStuntJumps();
            }
        }

        /// <summary>
        /// Automatically disable the stunt jumps if the player disconnects from the server.
        /// </summary>
        /// <param name="resourceName"></param>
        [EventHandler("onResourceStop")]
        public void OnResourceStop(string resourceName)
        {
            if (resourceName == GetCurrentResourceName())
            {
                DisableStuntJumps();
            }
        }

        [EventHandler("stuntjumps:enableJumps")]
        public void EnableStuntJumps()
        {
            // Make sure there are no stunt jumps set beforehand.
            DisableStuntJumps();

            foreach (var stuntJump in JsonConvert.DeserializeObject<Dictionary<string, List<dynamic>>>(Properties.Resources.stuntjumps))
            {
                foreach (var jump in stuntJump.Value)
                {
                    if (stuntJump.Key == "angledStuntJumps")
                    {
                        stuntJumps.Add(AddStuntJumpAngled(
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
                        ));
                    }
                    else
                    {
                        stuntJumps.Add(AddStuntJump(
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
                        ));
                    }
                }
            }
        }

        [EventHandler("stuntjumps:disableJumps")]
        public void DisableStuntJumps()
        {
            foreach (int jump in stuntJumps)
            {
                DeleteStuntJump(jump);
            }
            stuntJumps.Clear();
            DisableBlips();
        }

        [EventHandler("stuntjumps:disableBlips")]
        public void DisableBlips()
        {
            foreach (int blip in blips)
            {
                int b = blip;
                if (DoesBlipExist(b))
                {
                    RemoveBlip(ref b);
                }
            }
            blips.Clear();
        }

        [EventHandler("stuntjumps:enableBlips")]
        public void EnableBlips()
        {
            // Remove existing blips first.
            DisableBlips();

            foreach (var stuntJump in JsonConvert.DeserializeObject<Dictionary<string, List<dynamic>>>(Properties.Resources.stuntjumps))
            {
                foreach (var jump in stuntJump.Value)
                {
                    // Add blip
                    int blip = AddBlipForCoord((float)jump["jumpPos"]["start"]["X"], (float)jump["jumpPos"]["start"]["Y"], (float)jump["jumpPos"]["start"]["Z"]);
                    SetBlipSprite(blip, 488);
                    SetBlipColour(blip, 6);
                    BeginTextCommandSetBlipName("RADAR_STUNT_JUMP");
                    EndTextCommandSetBlipName(blip);
                    SetBlipAsShortRange(blip, true);
                    blips.Add(blip);
                }
            }
        }
    }
}
