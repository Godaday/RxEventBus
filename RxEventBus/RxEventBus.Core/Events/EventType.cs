using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxEventBus.Core.Events
{
    /// <summary>
    /// 系统事件类型
    /// （根据系统业扩展该事件类型，作为EventBus、EventHandler处理Event的过滤）
    /// </summary>
    public enum AppEventType
    {
      
        /// <summary>
        /// 出库
        /// </summary>
        StockOutEvent,
        /// <summary>
        /// 入库
        /// </summary>
        StockInEvent,

        /// <summary>
        /// 根据业务扩展其他事件类型
        /// </summary>
        OtherEvent

    }

}
