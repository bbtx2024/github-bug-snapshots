using System.Windows;
using System.Linq;

namespace Quick.Models
{
    /// <summary>
    /// 提供对 Carrier 集合中工位唯一性的校验工具。
    /// 确保任何时候，同一个工位（CurrentStation）最多只能存在一个 Carrier 实例。
    /// 如果检测到冲突，将弹出提示框提醒开发者或操作人员。
    /// </summary>
    public static class CarrierUniquenessValidator
    {
        /// <summary>
        /// 在设置 Carrier 的 CurrentStation 属性时调用此方法。
        /// 检查是否有其他 Carrier 实例已经占用了相同工位。
        /// 如果存在冲突，将弹出 MessageBox 警告。
        /// </summary>
        /// <param name="changedCarrier">当前正在更改工位的 Carrier 实例</param>
        public static void AssertUniqueStation(Carrier changedCarrier)
        {
            var conflict = CarrierPool.Carriers
                .Where(c => c != changedCarrier && c.CurrentStation == changedCarrier.CurrentStation)
                .ToList();

            if (conflict.Any())
            {
                MessageBox.Show($"工位 {changedCarrier.CurrentStation} 上已有 Carrier，请检查任务逻辑！");
            }
        }
    }
}
