using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Constants;
using WebHooks.Data.Entities;
using WebHooks.Service.Dtos;

namespace WebHooks.Service.Models
{
    public class WorkExecutionLogSummary : Dto<long>
    {
        /// <summary>
        /// 对应的工作项编号
        /// </summary>
        public Guid WorkId { get; set; }

        /// <summary>
        /// 开始执行的时间
        /// </summary>
        public DateTime? ExecuteStartAt { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime? ExecuteEndAt { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public TimeSpan? ElapsedTime { get; set; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public WebExecutionStatus Status { get; set; }

        /// <summary>
        /// 执行成功
        /// </summary>
        public bool? Success { get; set; }
    }
}
