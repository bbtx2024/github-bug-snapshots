using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Quick.Models.Enum;

namespace Quick.Models
{
    public static class CarrierPoolExtensions
    {
        /// <summary>
        /// 判断是否存在指定工位和状态的载具
        /// </summary>
        public static bool HasCarrier(this BindableCollection<Carrier> carriers, CurrentStation station, CurrentStatus status)
        {
            return carriers.Any(c => c.CurrentStation == station && c.CurrentStatus == status);
        }

        /// <summary>
        /// 是否不存在某工位任何载具
        /// </summary>
        public static bool IsStationEmpty(this BindableCollection<Carrier> carriers, CurrentStation station)
        {
            return !carriers.Any(c => c.CurrentStation == station);
        }

        /// <summary>
        /// 更改指定工位上载具的状态
        /// </summary>
        public static void SetCarrierStatusAtStation(this BindableCollection<Carrier> carriers, CurrentStation station, CurrentStatus newStatus)
        {
            foreach (var carrier in carriers.Where(c => c.CurrentStation == station))
            {
                carrier.CurrentStatus = newStatus;
            }
        }

        /// <summary>
        /// 将指定工位上的 Carrier 移动到下一个工位，并设置新状态
        /// </summary>
        public static void MoveCarrierToNextStation(this BindableCollection<Carrier> carriers, CurrentStation fromStation, CurrentStation toStation, CurrentStatus newStatus)
        {
            var target = carriers.FirstOrDefault(c => c.CurrentStation == fromStation);
            if (target != null)
            {
                target.CurrentStation = toStation;
                target.CurrentStatus = newStatus;
            }
            else
            {
                throw new InvalidOperationException($"找不到来自 {fromStation} 的 Carrier，无法移动。");
            }
        }

        /// <summary>
        /// 将扫码结果写入缓存位（Line1Buffer）的载具对象
        /// </summary>
        /// <param name="carriers">Carrier集合</param>
        /// <param name="snCode">扫码结果</param>
        public static void SetSnCodeAtLine1Buffer(this BindableCollection<Carrier> carriers, string snCode)
        {
            var target = carriers.FirstOrDefault(c => c.CurrentStation == CurrentStation.Line1Buffer);
            if (target != null)
            {
                target.SnCode = snCode;
            }
            else
            {
                throw new InvalidOperationException("未找到缓存位（Line1Buffer）上的载具，无法设置 SN。");
            }
        }

        /// <summary>
        /// 将扫码结果写入缓存位（Line2Buffer）的载具对象
        /// </summary>
        /// <param name="carriers">Carrier集合</param>
        /// <param name="snCode">扫码结果</param>
        public static void SetSnCodeAtLine2Buffer(this BindableCollection<Carrier> carriers, string snCode)
        {
            var target = carriers.FirstOrDefault(c => c.CurrentStation == CurrentStation.Line2Buffer);
            if (target != null)
            {
                target.SnCode = snCode;
            }
            else
            {
                throw new InvalidOperationException("未找到缓存位（Line2Buffer）上的载具，无法设置 SN。");
            }
        }

        /// <summary>
        /// 判断指定工位的指定穴位是否启用
        /// </summary>
        /// <param name="carriers">Carrier集合</param>
        /// <param name="station">工位</param>
        /// <param name="row">穴位行号</param>
        /// <param name="column">穴位列号</param>
        /// <returns>如果找到指定穴位，返回其 IsEnabled 状态；否则返回 false</returns>
        public static bool IsCavityEnabled(this BindableCollection<Carrier> carriers, CurrentStation station, int row, int column)
        {
            var carrier = carriers.FirstOrDefault(c => c.CurrentStation == station);
            if (carrier == null)
            {
                System.Windows.MessageBox.Show("当前工位上没有载具！", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            var unit = carrier.CavityUnits.FirstOrDefault(cu => cu.Row == row && cu.Column == column);
            if (unit == null)
            {
                System.Windows.MessageBox.Show("当前行列没有穴位！", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return false;
            }

            return unit.IsEnabled;
        }

        /// <summary>
        /// 设置指定工位的某个穴位为已处理状态（IsProcessed = true）
        /// </summary>
        /// <param name="carriers">载具集合</param>
        /// <param name="station">工位</param>
        /// <param name="row">穴位行</param>
        /// <param name="column">穴位列</param>
        public static void SetCavityProcessed(this BindableCollection<Carrier> carriers, CurrentStation station, int row, int column)
        {
            var carrier = carriers.FirstOrDefault(c => c.CurrentStation == station);
            if (carrier == null)
                throw new InvalidOperationException($"未找到工位 {station} 上的载具！");

            var unit = carrier.CavityUnits.FirstOrDefault(cu => cu.Row == row && cu.Column == column);
            if (unit == null)
                throw new InvalidOperationException($"未找到指定穴位：行 {row}，列 {column}");

            unit.IsProcessed = true;
        }

        /// <summary>
        /// 从集合中移除所有状态为 Killed 的载具
        /// </summary>
        /// <param name="carriers">载具集合</param>
        public static void RemoveKilled(this BindableCollection<Carrier> carriers)
        {
            // 使用 ToList 防止在迭代时修改集合导致异常
            var toRemove = carriers.Where(c => c.CurrentStatus == CurrentStatus.Killed).ToList();

            foreach (var carrier in toRemove)
                carriers.Remove(carrier);
        }

    }
}
