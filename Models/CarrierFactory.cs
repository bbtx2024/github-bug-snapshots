using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quick.Models.Enum;

namespace Quick.Models
{
    /// <summary>
    /// 提供创建 Carrier 实例的统一工厂方法。
    /// 用于初始化载具对象并按需注册到全局对象池中。
    /// </summary>
    public static class CarrierFactory
    {

        private static int _snCounter = 1;

        /// <summary>
        /// 创建一个新的 Carrier 实例，并设置初始工位与状态。
        /// </summary>
        /// <param name="station">初始化的工位</param>
        /// <param name="status">初始化的载具状态</param>
        /// <returns>新建的 Carrier 对象</returns>
        private static Carrier CreateNew(CurrentStation station, CurrentStatus status)
        {
            var sn = $"C{_snCounter++.ToString("D3")}"; // 输出格式：C001, C002...

            return new Carrier
            {
                SnCode = sn,

                CurrentStation = station,
                CurrentStatus = status
            };
        }


        /// <summary>
        /// 创建一个新的 Carrier，并自动注册到全局载具集合中（CarrierPool.Carriers）。
        /// 推荐在“正许入料 IN”时使用，确保其生命周期可全局追踪。
        /// </summary>
        /// <param name="station">初始化的工位</param>
        /// <param name="status">初始化的状态</param>
        /// <returns>已注册的 Carrier 实例</returns>
        public static Carrier CreateAndRegister(CurrentStation station, CurrentStatus status)
        {
            var carrier = CreateNew(station, status);
            CarrierPool.Carriers.Add(carrier);
            return carrier;
        }
    }
}
