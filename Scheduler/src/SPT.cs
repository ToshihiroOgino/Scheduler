namespace Scheduler.src
{
    sealed class SPT : core.SchedulerBase
    {
        /// <summary>
        /// 処理時間順
        /// </summary>
        public override void Run()
        {
            System.Console.WriteLine("処理時間順でプロセスを消化します");

            // キューを処理時間順に実行
            while (true)
            {
                // 待機キューも未完了キューも空の場合終了する
                if (queue.IsEmpty)
                    break;

                // キューを更新
                queue.Update(System_Time);
                queue.SortByTimeToProcess();

                // まだプロセスが到着していない場合は到着するまで時間を進める
                if (queue.GetIncompleteQueueCount == 0)
                {
                    UpdateSystemTime();
                    continue;
                }

                // キューの先頭を実行
                while (true)
                {
                    // プロセスが完了するまで実行し続ける
                    UpdateSystemTime();
                    if (queue.ExecuteFront(System_Time))
                        break;
                }
            }

            Console.WriteLine("キューの消化が完了しました。");
        }
    }
}
