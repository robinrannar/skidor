using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skidor4.Data;
using Skidor4.Models;

namespace Skidor4.Models.ViewModels
{
    public class FilterScoreboardVM
    {
        public List<string> TimeNameList { get; set; }
        public List<string> TrackNameList { get; set; } = new List<string>();
        public List<string> FilterList { get; set; }
        DbTrackOperation dbo = new DbTrackOperation();
        public List<scoreboard> scoreList { get; set; }
        public scoreboard personalScore { get; set; }

        public FilterScoreboardVM(string date, string track, string filter, int pId)
        {
            CombineFilters();
            SetScoreboard(date, track, filter, pId);
        }

        public void CombineFilters()
        {
            TimeNameList = new List<string> {
                "Idag",
                "Denna veckan",
                "Denna säsongen"
            };

            FilterList = new List<string> {
                "Snabbast",
                "Längst"
            };

            var t = dbo.GetTracks();
            TrackNameList.Add("Alla spår");

            foreach (var track in t)
            {
                if(!(track.name.Contains("Stadionområdet") || track.name.Contains("Friåk")))
                {
                    TrackNameList.Add(track.name);
                }
            }  
        }

        public void SetScoreboard(string date, string track, string filter, int pId)
        {
            Methods method = new Methods();
            DbScoreboardOperation dbscores = new DbScoreboardOperation();
            DbChipOperation dbChips = new DbChipOperation();
            scoreList = new List<scoreboard>();
            personalScore = new scoreboard();
            int i = 0;

            scoreList = dbscores.GetScoreboardByCondition(date, track);
            
            foreach (var score in scoreList)
            {
                i++;
                var chiplink = dbChips.GetPriceByChipId(score.fk_chip_id);
                score.alias = chiplink.persons.alias;
                score.speed = method.GetSpeed(score.Sum, score.tracks.length);
                score.placement = i;
            }

            personalScore = scoreList.FirstOrDefault(x => x.fk_chip_id == pId);
            scoreList = scoreList.Take(10).ToList();
        }
    }


}