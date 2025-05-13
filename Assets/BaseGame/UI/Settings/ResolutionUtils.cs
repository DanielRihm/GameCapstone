using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCPS.SlipForge.UI
{
    public static class ResolutionUtils
    {
        public static List<string> GetResolutionList()
        {
            List<string> resolutions = Screen.resolutions.Select(resolution => GetResolutionStringNoRefresh(resolution)).ToList();
            CleanResolutionList(resolutions);
            return resolutions;
        }

        private static void CleanResolutionList(List<string> resolutions)
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                // if resolution is duplicate of previous resolution, remove it
                if (i > 0 && resolutions[i] == resolutions[i - 1])
                {
                    resolutions.RemoveAt(i);
                    i--;
                }
            }
        }

        public static string GetResolutionStringNoRefresh(Resolution resolution)
        {
            return resolution.ToString()[..(resolution.ToString().IndexOf('@') - 1)];
        }

        public static int GetResolutionIndex(List<string> resolutionList, Resolution resolution)
        {
            return resolutionList.FindIndex(res => res == GetResolutionStringNoRefresh(resolution));
        }

        public static int GetResolutionIndex(List<string> resolutionList, string resolution)
        {
            return resolutionList.IndexOf(resolution);
        }

        public static (int width, int height) GetResolutionFromString(string resolution)
        {
            string[] split = resolution.Split('x');
            return (int.Parse(split[0]), int.Parse(split[1]));
        }
    }
}
