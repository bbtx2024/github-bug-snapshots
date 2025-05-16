using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Quick.Models.Enum;

namespace Quick.Models
{
    /// <summary>
    /// 全局载具对象池，维护系统中所有活跃的 Carrier 实例。
    /// 提供对当前所有载具的访问、管理与生命周期追踪能力。
    /// 每个工位最多应有一个载具实例，载具在流程中进出、状态变更。
    /// </summary>
    public static class CarrierPool
    {
        /// <summary>
        /// 当前系统中所有活跃的 Carrier 集合。
        /// 可通过 LINQ 查询指定工位、状态的载具。
        /// 注意：此集合应仅包含生命周期内仍在使用的对象。
        /// </summary>
        public static BindableCollection<Carrier> Carriers { get; } = new BindableCollection<Carrier>();
    }
}
