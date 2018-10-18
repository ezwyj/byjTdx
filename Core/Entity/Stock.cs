using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entity
{
    /// <summary>
    /// 股票
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string symbol { get; set; }
        /// <summary>
        /// 股票名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 一档委买价
        /// </summary>
        public decimal bidPrice { get; set; }
        /// <summary>
        /// 一档委买量
        /// </summary>
        public Decimal bidVolume { get; set; }
    }
}
