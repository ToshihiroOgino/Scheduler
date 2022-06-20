using Scheduler.src.core;

namespace Scheduler.src.core
{
    abstract class SchedulerBase
    {
        /// <summary>
        /// システムの現在時刻
        /// </summary>
        public uint System_Time { get { return system_time; } }
        private uint system_time;

        /// <summary>
        /// プロセス集合
        /// </summary>
        public ProsessQueue queue;

        public SchedulerBase()
        {
            queue = new();
            system_time = 0;
        }

        /// <summary>
        /// スケジューラを初期化する
        /// </summary>
        public void Init()
        {
            // キューを初期化
            queue.Init();
            // 時刻を初期化
            system_time = 0;
        }

        /// <summary>
        /// ここに各アルゴリズムを実装する
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// システム時間を更新
        /// </summary>
        public void UpdateSystemTime()
        {
            system_time++;
        }

        /// <summary>
        /// ターンアラウンドタイムの平均を計算して表示する
        /// </summary>
        public void ShowAverageTurnAroundTime()
        {
            // 平均応答時間を計算する
            float Average_Turn_Around_Time = queue.AverageTurnAroundTime();
            System.Console.WriteLine("平均応答時間:" + Average_Turn_Around_Time);
        }
    }
}
