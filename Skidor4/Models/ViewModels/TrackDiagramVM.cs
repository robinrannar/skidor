using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skidor4.Data;

namespace Skidor4.Models.ViewModels
{
    public class TrackDiagramVM
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public List<TrackDiagramVM> SetTracks(string date)
        {
            var list = new List<TrackDiagramVM>();
            DbScoreboardOperation dbScores = new DbScoreboardOperation();
            DbTrackOperation dbTracks = new DbTrackOperation();

            if (string.IsNullOrEmpty(date))
            {
                date = "Idag";
            }

            var c = dbScores.GetScoreboardByDateCondition(date);

            var count = c.GroupBy(item => item.fk_track_id).Select(grp => new { Number = grp.Key, Count = grp.Count() });

            foreach (var i in count)
            {
                int id = i.Number;
                var track = dbTracks.GetTrackById(id);
                track.count = i.Count;
                list.Add(new TrackDiagramVM { Name = track.name, Count = track.count });
            }

            return list;
        }

        public List<TrackDiagramVM> SetCardList(string date)
        {
            var list = new List<TrackDiagramVM>();
            DbChipOperation dbChips = new DbChipOperation();
            DbPriceOperation dbPrice = new DbPriceOperation();

            if (string.IsNullOrEmpty(date))
            {
                date = "Idag";
            }

            var c = dbChips.GetChipByDateCondition(date);
            var count = c.GroupBy(item => item.price_id).Select(grp => new { Number = grp.Key, Count = grp.Count() });

            foreach (var i in count)
            {
                int id = i.Number;
                var price = dbPrice.GetPriceByPriceID(id);
                price.count = i.Count;
                price.sum = price.price * price.count;
                list.Add(new TrackDiagramVM { Name = price.name, Count = price.count });
            }

            return list;
        }

    }



}