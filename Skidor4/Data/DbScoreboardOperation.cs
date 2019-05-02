using Skidor4.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Skidor4.Data
{
    public class DbScoreboardOperation
    {

        db4Entities dbcalls = new db4Entities();
        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta scoreboard
        public List<scoreboard> GetScoreboard()
        {
            return dbcalls.scoreboard.ToList();
        }
        //metod för att hämta scoreboard detaljer
        public List<covers> ScoreboardDetails(int id)
        {
            var result = dbcalls.scoreboard.FirstOrDefault(x => x.pk_id == id);

            var score = dbcalls.tracks.FirstOrDefault(x => x.name == result.tracks.name);
            var covers = dbcalls.covers.Where(x => x.fk_chip_id == result.fk_chip_id && x.time >= result.start_time && x.time <= result.end_time).ToList();

            return covers;
        }
        //metod för att hämta scores
        public List<scoreboard> GetPersonalScoreboard(int id)
        {
            var score = dbcalls.scoreboard.Where(x => x.fk_chip_id == id).ToList();

            foreach (var s in score)
            {
                TimeSpan time = TimeSpan.FromSeconds(s.Sum);
                s.time = time.ToString(@"hh\:mm\:ss");
            }
            return score;
        }
        //metod för att hämta statistik
        public List<scoreboard> SkiTracksStatistics()
        {
            var scoreTracks = dbcalls.scoreboard.Where(x => x.fk_track_id == x.tracks.pk_id).OrderBy(z => z.end_time);

            List<scoreboard> highScores = new List<scoreboard>();

            foreach (var s in scoreTracks)
            {
                var eq = dbcalls.equipment.FirstOrDefault(x => x.fk_chip_id == s.fk_chip_id);

                if (eq == null)
                {
                    var chipPerson = dbcalls.chiplink.FirstOrDefault(x => x.chip_id == s.fk_chip_id);
                    var person = dbcalls.persons.FirstOrDefault(x => x.pk_id == chipPerson.person_id);

                    if ((bool)person.@public)
                    {
                        var track = dbcalls.tracks.FirstOrDefault(x => x.pk_id == s.fk_track_id);
                        double howFast = Math.Round((Convert.ToDouble(track.length) / (s.Sum)), 1);
                        s.speed = (howFast * 3.6).ToString() + " km/h";

                        TimeSpan time = TimeSpan.FromSeconds(s.Sum);
                        s.time = time.ToString(@"hh\:mm\:ss");
                        s.alias = person.alias;
                        s.km = Math.Round(Convert.ToDouble(track.length) / 1000, 1).ToString() + " km";

                        highScores.Add(s);
                    }
                }
            }

            return highScores;
        }

        //metod för att hämta scoreboard beroende på dag
        public List<scoreboard> GetScoreboardByCondition(string date, string track)
        {
            DateTime d = DateTime.Today;
            DateTime seasonStart = new DateTime(2017, 9, 1, 0, 0, 0);
            List<scoreboard> sList = new List<scoreboard>();
            scoreboard s = new scoreboard();

            var t = dbcalls.tracks.FirstOrDefault(x => x.name == track);
            var tracks = dbcalls.tracks.ToList();

            if (date == "Idag" || date == null)
            {
                if (track == "Alla spår")
                {
                    foreach(var p in tracks)
                    {
                        List<scoreboard> list = dbcalls.scoreboard.Where(x => x.fk_track_id == p.pk_id && x.end_time >= d).ToList();
                        list.Sort((x, y) => x.Sum.CompareTo(y.Sum));

                        s = list.FirstOrDefault();

                        if (s != null)
                        {
                            sList.Add(s);
                        }       
                    }
                }
                else
                {
                    sList = dbcalls.scoreboard.Where(x => x.end_time >= d && x.fk_track_id == t.pk_id).ToList();
                }
            }

            else if (date == "Denna veckan")
            {
                d = d.AddDays(-7);

                if (track == "Alla spår")
                {
                    foreach (var p in tracks)
                    {
                        List<scoreboard> list = dbcalls.scoreboard.Where(x => x.fk_track_id == p.pk_id && x.end_time >= d).ToList();
                        list.Sort((x, y) => x.Sum.CompareTo(y.Sum));

                        s = list.FirstOrDefault();

                        if (s != null)
                        {
                            sList.Add(s);
                        }
                    }
                }
                else
                { 
                    sList = dbcalls.scoreboard.Where(x => x.end_time >= d && x.fk_track_id == t.pk_id).ToList();
                }

            }
            else if (date == "Denna säsongen")
            {
                if (track == "Alla spår")
                {
                    foreach (var p in tracks)
                    {
                        List<scoreboard> list = dbcalls.scoreboard.Where(x => x.fk_track_id == p.pk_id && x.end_time >= seasonStart).ToList();
                        list.Sort((x, y) => x.Sum.CompareTo(y.Sum));

                        s = list.FirstOrDefault();

                        if (s != null)
                        {
                            sList.Add(s);
                        }
                    }
                }
                else
                {
                    sList = dbcalls.scoreboard.Where(x => x.end_time >= seasonStart && x.fk_track_id == t.pk_id).ToList();
                }
            }

            if(sList.Count >= 2)
            {
                sList.Sort((x, y) => y.Sum.CompareTo(x.Sum));

            }
            

            List<int> idList = new List<int>();
            List<scoreboard> scoreList = new List<scoreboard>();

            foreach (var p in sList)
            {
                if (!idList.Contains(p.pk_id))
                {
                    scoreList.Add(p);
                    idList.Add(p.pk_id);
                }
            }

            scoreList.Sort((x, y) => x.Sum.CompareTo(y.Sum));

            return scoreList;
        }

        //metod för att hämta scoreboard beroende på dag
        public List<scoreboard> GetScoreboardByDateCondition(string date)
        {
            DateTime d = DateTime.Today;
            DateTime seasonStart = new DateTime(2017, 9, 1, 0, 0, 0);
            List<scoreboard> sList = new List<scoreboard>();
            scoreboard s = new scoreboard();


            if (date == "Idag" || date == null)
            {
               sList = dbcalls.scoreboard.Where(x => x.end_time >= d).ToList();
            }

            else if (date == "Denna veckan")
            {
                d = d.AddDays(-7);
                sList = dbcalls.scoreboard.Where(x => x.end_time >= d).ToList();
            }
            else if (date == "Denna säsongen")
            {
                sList = dbcalls.scoreboard.Where(x => x.end_time >= seasonStart).ToList();
            }

            return sList;
        }



        #endregion
        // <----------------------------------------------------->
    }
}