using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Helpers.IdGenerator;
using WebApplication.Helpers;
using System.ComponentModel;
using WebApplication.Entities.OperatorManage;

namespace WebApplication.Entities
{
    public class BaseEntity
    {
        /// <summary>
        /// 所有表的主键
        /// long返回到前端js的时候，会丢失精度，所以转成字符串
        /// </summary>
        [JsonConverter(typeof(StringJsonConverter))]
        public long? Id { get; set; }

        /// <summary>
        /// WebApi没有Cookie和Session，所以需要传入Token来标识用户身份
        /// </summary>
        [NotMapped]
        public string Token { get; set; }

        public virtual void Create()
        {
            Id = IdGeneratorHelper.Instance.GetId();
        }
    }

    public class BaseCreateEntity : BaseEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonConverter(typeof(DateTimeJsonConverter))]
        [Description("创建时间")]
        public DateTime? BaseCreateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long? BaseCreatorId { get; set; }

        public new async Task Create()
        {
            base.Create();

            if (this.BaseCreateTime == null)
            {
                this.BaseCreateTime = DateTime.Now;
            }

            if (this.BaseCreatorId == null)
            {
                OperatorInfos user = await Operator.Instance.Current(Token);
                if (user != null)
                {
                    this.BaseCreatorId = user.UserId;
                }
                else
                {
                    if (this.BaseCreatorId == null)
                    {
                        this.BaseCreatorId = 0;
                    }
                }
            }
        }
    }
}
