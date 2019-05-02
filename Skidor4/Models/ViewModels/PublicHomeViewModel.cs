using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skidor4.Data;
using System.ComponentModel.DataAnnotations;

namespace Skidor4.Models.ViewModels
{
    public class PublicHomeViewModel : CreateUserViewModel
    {
        public List<tracks> Tracks { get; set; }
        public List<persons> Persons { get; set; }
        public new persons Person { get; set; }
        [Display(Name = "Användarnamn")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ett användarnamn")]
        public string alias { get; set; }

        [Display(Name = "Lösenord")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Du måste ange ditt lösenord")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public permissions Role { get; set; } = new permissions();

        //FilterScoreboardVM
        public List<string> TimeNameList { get; set; }
        public List<string> TrackNameList { get; set; } = new List<string>();
        public List<string> FilterList { get; set; }
        DbTrackOperation dbo = new DbTrackOperation();
        DbPersonOperation dbPerson = new DbPersonOperation();
        public List<scoreboard> scoreList { get; set; }
        public scoreboard personalScore { get; set; }
        public string moreSpeed { get; set; }
        public string moreKm { get; set; }
        public string mapImgUrl { get; set; }

        public PublicHomeViewModel(string date, string track, int pId)
        {
            CombineFilters();
            SetScoreboard(date, track, pId);
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
                if (!(track.name.Contains("Stadionområdet") || track.name.Contains("Friåk")))
                {
                    TrackNameList.Add(track.name);
                }
            }
        }

        public void SetScoreboard(string date, string track, int pId)
        {
            Methods method = new Methods();
            DbScoreboardOperation dbscores = new DbScoreboardOperation();
            DbChipOperation dbChips = new DbChipOperation();
            scoreList = new List<scoreboard>();
            personalScore = new scoreboard();
            int i = 0;
            int id = 0;

            scoreList = dbscores.GetScoreboardByCondition(date, track);

            foreach (var score in scoreList)
            {
                i++;
                var chiplink = dbChips.GetPriceByChipId(score.fk_chip_id);
                score.alias = chiplink.persons.alias;
                score.speed = method.GetSpeed(score.Sum, score.tracks.length);
                score.placement = i;
                id = chiplink.chip_id;
                score.time = TimeSpan.FromSeconds(score.Sum).ToString();
            }

            if (scoreList.Count()!= 0)
            {
                var person = dbChips.GetChipLink(pId);
                
                if (person != null)
                {
                    personalScore = scoreList.FirstOrDefault(x => x.fk_chip_id == person.chip_id);
                    if (personalScore != null)
                    {
                        var lastScore = scoreList.Last();
                        moreSpeed = TimeSpan.FromSeconds(((personalScore.Sum) - (lastScore.Sum))).ToString();

                        double result = Math.Round((Convert.ToDouble(method.GetSpeed(lastScore.Sum, lastScore.tracks.length))) - (Convert.ToDouble(method.GetSpeed(personalScore.Sum, personalScore.tracks.length))), 1);
                        moreKm = result.ToString();
                    }
                }
            }

            scoreList = scoreList.Take(10).ToList();
        }
        public string GetMap(string mapName)
        {
            var tracks = dbo.GetTracks();

            foreach (var track in tracks)
            {
                if(track.name == mapName)
                {
                    if (mapName.Contains("blå"))
                    {
                        mapImgUrl = "../content/img/static/blå.png";
                    }
                    else if (mapName.Contains("röd"))
                    {
                        mapImgUrl = "../content/img/static/röd.png";
                    }
                    else if (mapName.Contains("grön"))
                    {
                        mapImgUrl = "../content/img/static/grön.png";
                    }
                }

                if(mapName == "Alla")
                {
                    mapImgUrl = "../content/img/static/all.png";

                    break;
                }
            }
            return mapImgUrl;
        }

        public void GetPerson()
        {
            persons Person = new persons();
            Person = dbPerson.GetPerson();
        }
    }
}