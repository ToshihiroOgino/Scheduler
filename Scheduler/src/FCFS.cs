namespace Scheduler.src
{
  sealed class FCFS : core.SchedulerBase
  {
    /// <summary>
    /// 到着順
    /// </summary>
    public override void Run()
    {
      System.Console.WriteLine("到着順でプロセスを消化します");

      // キューを順番に実行
      while (true)
      {
        // キューを更新
        queue.Update(System_Time);

        // 待機キューも未完了キューも空の場合終了する
        if (queue.IsEmpty)
          break;

        // まだプロセスが到着していない場合は到着するまで時間を進める
        if (queue.Incomplete.Count == 0)
        {
          UpdateSystemTime();
          continue;
        }

        // キューの先頭を実行
        while (true)
        {
          UpdateSystemTime();
          if (queue.ExecuteFront(System_Time))
            break;
        }
      }
      System.Console.WriteLine("キューの消化が完了しました。");
    }
  }
}