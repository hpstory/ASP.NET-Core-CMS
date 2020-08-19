using System;
using WebApplication.Helpers;

namespace WebApplication.Helpers.IdGenerator
{
    public class Snowflake
    {
        // 起始的时间戳
        private const long TwEpoch = 1577808000000L;//2020-01-01 00:00:00
        // 每一部分占用的位数
        private const int WorkerIdBits = 5; // 机器标识占用的位数
        private const int DatacenterIdBits = 5; // 数据中心占用的位数
        private const int SequenceBits = 12; // 序列号占用的位数
        // 每一部分最大值
        private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits); // 31
        private const long MaxDatacenterId = -1L ^ (-1L << DatacenterIdBits); // 31
        private const long SequenceMask = -1L ^ (-1L << SequenceBits); // 4095
        // 每一部分向左的位移
        private const int WorkerIdShift = SequenceBits; // 12
        private const int DatacenterIdShift = SequenceBits + WorkerIdBits; // 17
        private const int TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits; //23


        private long _sequence = 0L;
        private long _lastTimestamp = -1L;
        /// <summary>
        ///10位的数据机器位中的高位
        /// </summary>
        public long WorkerId { get; protected set; }
        /// <summary>
        /// 10位的数据机器位中的低位
        /// </summary>
        public long DatacenterId { get; protected set; }

        private readonly object _lock = new object();
        public long CurrentId { get; private set; }

        /// <summary>
        /// 基于Twitter的snowflake算法
        /// </summary>
        /// <param name="workerId">10位的数据机器位中的高位，默认不应该超过5位(5byte)</param>
        /// <param name="datacenterId"> 10位的数据机器位中的低位，默认不应该超过5位(5byte)</param>
        /// <param name="sequence">初始序列</param>
        public Snowflake(long workerId, long datacenterId, long sequence = 0L)
        {
            WorkerId = workerId;
            DatacenterId = datacenterId;
            _sequence = sequence;

            if (workerId > MaxWorkerId || workerId < 0)
            {
                throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");
            }

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
            {
                throw new ArgumentException($"datacenter Id can't be greater than {MaxDatacenterId} or less than 0");
            }
        }

        /// <summary>
        /// 获取下一个Id，该方法线程安全
        /// </summary>
        /// <returns></returns>
        public long NextId()
        {
            // 同一时刻只能被一个线程操作
            lock (_lock)
            {
                var timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
                if (timestamp < _lastTimestamp)
                {
                    //TODO 是否可以考虑直接等待？
                    throw new Exception(
                        $"Clock moved backwards or wrapped around. Refusing to generate id for {_lastTimestamp - timestamp} ticks");
                }

                if (_lastTimestamp == timestamp)
                {
                    // 当前调用和上一次调用落在了相同毫秒内，只能通过第三部分，序列号自增来判断为唯一，所以+1.
                    _sequence = (_sequence + 1) & SequenceMask;
                    // 同一毫秒的序列数已经达到最大，只能等待下一个毫秒
                    if (_sequence == 0)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    // 不同毫秒内，序列号置为0
                    _sequence = 0;
                }
                _lastTimestamp = timestamp;
                //就是用相对毫秒数、机器ID和自增序号拼接
                CurrentId = ((timestamp - TwEpoch) << TimestampLeftShift) | // 时间戳部分
                         (DatacenterId << DatacenterIdShift) | // 数据中心部分
                         (WorkerId << WorkerIdShift) | _sequence; // 机器标识和序列号部分

                return CurrentId;
            }
        }

        private long TilNextMillis(long lastTimestamp)
        {
            var timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
            while (timestamp <= lastTimestamp)
            {
                timestamp = DateTimeHelper.GetUnixTimeStamp(DateTime.Now);
            }
            return timestamp;
        }
    }
}

// 但是这里有一个坑，雪花算法产生的长整数的精度可能超过javascript能表达的精度，
// 这会导致js获取的id与雪花算法算出来的id不一致，这会导致对数据的所有操作都失效。
// 解决办法：后端的语言获取到雪花算法的id后将其转换为String类型，这样js也会当做字符串来处理，就不会丢失精度了。
