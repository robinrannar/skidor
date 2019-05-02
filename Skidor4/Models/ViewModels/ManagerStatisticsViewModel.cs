using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skidor4.Models;
using Skidor4.Data;

namespace Skidor4.Models.ViewModels
{
    public class ManagerStatisticsViewModel
    {
        DbEquipmentOperation dbequipment = new DbEquipmentOperation();
        DbChipOperation dbChip = new DbChipOperation();
        DbTrackOperation dbtrack = new DbTrackOperation();
        DbCoverOperation dbCovers = new DbCoverOperation();
        DbPriceOperation dbPrice = new DbPriceOperation();

        public List<covers> StationRegistration { get; set; } = new List<covers>();
        public List<equipment> EquipmentList { get; set; } = new List<equipment>();
        public Tuple<string, int>[] noChipTuple { get; set; } = new Tuple<string, int>[3];
        public Tuple<string, int>[] soldChipTuple { get; set; } = new Tuple<string, int>[3];

        public List<string> TimeNameList { get; set; }

        public ManagerStatisticsViewModel (string date)
        {
            SetTimeFilter();
            SetEquipment();
            SetNoChip();
            SetSoldChip();
        }

        public void SetTimeFilter()
        {
            TimeNameList = new List<string> {
                "Idag",
                "Denna veckan",
                "Denna säsongen"
            };
        }

        public void SetNoChip()
        {        
            var c = dbCovers.GetNoChipRegistrations();

            noChipTuple[0] = new Tuple<string, int>("Idag", c.Item1.Count());
            noChipTuple[1] = new Tuple<string, int>("Senaste månaden", c.Item2.Count());
            noChipTuple[2] = new Tuple<string, int>("Denna säsongen", c.Item3.Count());
        }

        public void SetEquipment()
        {
            var equipments = dbequipment.GetAllEquipment();

            foreach(var eq in equipments)
            {
                eq.isActive = false;
                var stations = dbCovers.GetCoversByChip(eq.fk_chip_id);

                if (stations.Count != 0)
                {
                    var station = stations.OrderByDescending(o => o.time).FirstOrDefault();
                    eq.time = station.time;

                    if (eq.time >= DateTime.Now.AddHours(-1))
                    {
                        eq.isActive = true;
                    }
                }
                EquipmentList.Add(eq);
            }
        }
      
        public void SetSoldChip()
        {
            var c = dbChip.GetSoldChipsByDate();

            soldChipTuple[0] = new Tuple<string, int>("Idag", c.Item1.Count());
            soldChipTuple[1] = new Tuple<string, int>("Senaste månaden", c.Item2.Count());
            soldChipTuple[2] = new Tuple<string, int>("Denna säsongen", c.Item3.Count());
        }

    }
}