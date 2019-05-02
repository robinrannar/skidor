using Skidor4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skidor4.Data
{
    public class DbTrackOperation
    {
        db4Entities dbcalls = new db4Entities();
        Methods methods = new Methods();

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta alla spår i databasen
        public List<tracks> GetTracks()
        {
            var list = dbcalls.tracks.ToList();
            List<tracks> trackList = new List<tracks>();

            foreach (var t in list)
            {
                if (t.length != null)
                {
                    t.length = Math.Round((Convert.ToDouble(t.length) / 1000), 1).ToString();

                    if(!(t.name == "Stadionområdet" || t.name == "Friåk"))
                    {
                        trackList.Add(t);
                    }
                }
            }

            return trackList;
        }
        //metod för att hämta ett spår i databasen
        public tracks GetTrackById(int id)
        {
            return dbcalls.tracks.FirstOrDefault(x => x.pk_id == id);
        }
        //metod för att knyta samman spår med åkare
        public void setTrackTimes(List<covers> list, DateTime date)
        {
            var tracklist = GetTracks();
            var newList = methods.SetCoverListNewDates(list, date);


            //List <covers> newList = new List<covers>();
            //string newDate = date.ToShortDateString();
            //int i = 0;



            //foreach (covers c in list)
            //{
            //    var eq = dbcalls.equipment.FirstOrDefault(x => x.fk_chip_id == c.fk_chip_id);

            //    if (eq != null)
            //    {
            //        c.isEquipment = true;
            //    }
            //    else
            //    {
            //        c.isEquipment = false;
            //    }

            //    c.timeTest = (DateTime)c.time;
            //    c.onlyDate = c.timeTest.ToShortDateString();
            //    newList.Add(c);
            //}

            int i = 0;
            string newDate = date.ToShortDateString();
            var coverList = newList.Where(x => x.onlyDate == newDate);
            coverList.OrderByDescending(d => d.timeTest);
            int numberOfStations = coverList.Count();
            covers[] stationCover = new covers[numberOfStations];
            string[] stationName = new string[numberOfStations];


            foreach (covers c in coverList)
            {
                var station = dbcalls.covers.FirstOrDefault(x => x.fk_station_id == c.fk_station_id && x.fk_chip_id == c.fk_chip_id);
                var s = dbcalls.stations.First(x => x.pk_id == station.fk_station_id);

                stationName[i] = s.name.Trim();
                stationCover[i] = c;
                i++;
            }

            string tracker = "";
            scoreboard trackNames = new scoreboard();
            List<scoreboard> scoreList = new List<scoreboard>();

            for (int e = 0; e < stationName.Length; e++)
            {
                tracker += stationName[e];
            }

            var firstelements = list.First();

            foreach (var item in tracklist)
            {

                if (tracker.Contains(item.code))
                {
                    scoreboard score = new scoreboard();

                    for (int s = 0; s < stationCover.Length; s++)
                    {

                        if (stationName[s] == "A")
                        {
                            if (score.start_time == DateTime.MinValue)
                            {
                                score.start_time = stationCover[s].time;
                            }
                            else if (score.end_time == DateTime.MinValue)
                            {
                                score.end_time = stationCover[s].time;

                                if (stationCover[s].isEquipment == true)
                                {
                                    var t = dbcalls.tracks.First(x => x.pk_id == item.pk_id);
                                    t.pist = score.end_time;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    score.fk_track_id = item.pk_id;
                    score.fk_chip_id = (int)firstelements.fk_chip_id;

                    var result = dbcalls.scoreboard.FirstOrDefault(x => x.start_time == score.start_time && x.end_time == score.end_time && x.fk_chip_id == score.fk_chip_id && x.fk_track_id == score.fk_track_id);

                    if (result == null)
                    {
                        dbcalls.scoreboard.Add(score);
                        dbcalls.SaveChanges();
                    }
                }
            }
        }
        #endregion
        // <----------------------------------------------------->
    }
}