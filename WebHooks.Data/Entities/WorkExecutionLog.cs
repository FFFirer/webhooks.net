using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHooks.Data.Constants;

namespace WebHooks.Data.Entities
{
    public class WorkExecutionLog : Entity<long>
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

        /// <summary>
        /// 捕获异常
        /// </summary>
        public string? Exception { get; set; }

        /// <summary>
        /// 执行的脚本
        /// </summary>
        public List<string>? Script { get; set; }

        /// <summary>
        /// 脚本执行的结果
        /// </summary>
        public List<ShellExecutedResultLine>? Results { get; set; }
    }
}
