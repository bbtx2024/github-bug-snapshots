using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Quick.Models.Enum;
using Newtonsoft.Json;

namespace Quick.Models
{
    public class Carrier : PropertyChangedBase
    {
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public DateTime LastUpdated { get; private set; } = DateTime.Now;

        public Dictionary<string, object> InitialSnapshot { get; private set; }
        public List<Dictionary<string, object>> SnapshotHistory { get; } = new List<Dictionary<string, object>>();

        private string _snCode;
        public string SnCode
        {
            get => _snCode;
            set
            {
                if (_snCode != value)
                {
                    _snCode = value;
                    //TrackSnapshot();
                    NotifyOfPropertyChange(() => SnCode);
                }
            }
        }

        private CurrentStation _currentStation;
        public CurrentStation CurrentStation
        {
            get => _currentStation;
            set
            {
                if (_currentStation != value)
                {
                    CarrierUniquenessValidator.AssertUniqueStation(this);
                    _currentStation = value;
                    //TrackSnapshot();
                    UpdateSn();
                    NotifyOfPropertyChange(() => CurrentStation);
                }
            }
        }

        private CurrentStatus _currentStatus = CurrentStatus.WatingOut;
        public CurrentStatus CurrentStatus
        {
            get => _currentStatus;
            set
            {
                if (_currentStatus != value)
                {
                    _currentStatus = value;
                    //TrackSnapshot();
                    UpdateSn();
                    NotifyOfPropertyChange(() => CurrentStatus);
                }
            }
        }

        public BindableCollection<CavityUnit> CavityUnits { get; set; }

        public Carrier()
        {
            CavityUnits = new BindableCollection<CavityUnit>();
            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var unit = new CavityUnit
                    {
                        Row = row,
                        Column = col,
                        IsEnabled = true,
                        IsProcessed = false,
                        ParentCarrier = this 
                    };

                    unit.CavLeft = 10 + col * 35.71;
                    unit.CavTop = 17.5 + row * 45;

                    CavityUnits.Add(unit);
                }
            }

            //InitialSnapshot = TakeSnapshot();
            //SnapshotHistory.Add(new Dictionary<string, object>(InitialSnapshot));

            UpdateSn();
        }


        /// <summary>
        /// 记录当前载具的状态快照，并更新最后修改时间。
        /// 每次调用会生成一份完整的对象快照，并追加到 SnapshotHistory 中。
        /// </summary>
        //private void TrackSnapshot()
        //{
        //    LastUpdated = DateTime.Now;
        //    SnapshotHistory.Add(TakeSnapshot());
        //}


        /// <summary>
        /// 生成当前 Carrier 对象的一份状态快照（字段快照 + 腔体结构）。
        /// 用于追溯对象的状态变化过程，每次属性变更后调用。
        /// </summary>
        /// <returns>
        /// 包含 SnCode、CurrentStation、CurrentStatus、CavitySnapshot、ChangedAt 的字典形式快照。
        /// </returns>
        //private Dictionary<string, object> TakeSnapshot()
        //{
        //    return new Dictionary<string, object>
        //    {
        //        ["SnCode"] = SnCode,
        //        ["CurrentStation"] = CurrentStation.ToString(),
        //        ["CurrentStatus"] = CurrentStatus.ToString(),
        //        ["CavitySnapshot"] = JsonConvert.SerializeObject(
        //            CavityUnits.Select(u => new
        //            {
        //                u.Row,
        //                u.Column,
        //                u.IsEnabled,
        //                u.IsProcessed
        //            }).ToList()),
        //        ["ChangedAt"] = LastUpdated.ToString("yyyy-MM-dd HH:mm:ss")
        //    };
        //}


        private string _displaySn;
        public string DisplaySn
        {
            get => _displaySn;
            private set
            {
                _displaySn = value;
                NotifyOfPropertyChange(() => DisplaySn);
            }
        }

        internal void UpdateSn()
        {
            DisplaySn = $"{SnCode}|{CurrentStation}|{CurrentStatus}";
        }
    }
}