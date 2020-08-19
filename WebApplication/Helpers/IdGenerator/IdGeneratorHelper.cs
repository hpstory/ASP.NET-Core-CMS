using WebApplication.Utils;

namespace WebApplication.Helpers.IdGenerator
{
    /// <summary>
    /// 生成数据库主键Id
    /// </summary>
    public class IdGeneratorHelper
    {
        private readonly int SnowFlakeWorkerId = GlobalContext.SystemConfig.SnowFlakeWorkerId;
        private Snowflake snowflake;

        public IdGeneratorHelper()
        {
            snowflake = new Snowflake(SnowFlakeWorkerId, 0, 0);
        }
        public static IdGeneratorHelper Instance { get; } = new IdGeneratorHelper();
        public long GetId()
        {
            return snowflake.NextId();
        }
    }
}
