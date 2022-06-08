namespace Scheduler.src
{
  sealed class RR : core.SchedulerBase
  {
    /// <summary>
    /// ラウンドロビンの定時間
    /// </summary>
    private const uint CONSTANT_TIME = 2;

    /// <summary>
    /// ラウンドロビン式
    /// </summary>
    public override void Run()
    {
      Console.WriteLine("ラウンドロビン式でプロセスを消化します");

      while (true)
      {
        // 待機キューも未完了キューも空の場合終了する
        if (queue.IsEmpty)
          break;

        // 更新
        queue.Update(System_Time);

        // まだプロセスが到着していない場合は到着するまで時間を進める
        if (queue.Incomplete.Count == 0)
        {
          UpdateSystemTime();
          continue;
        }

        // キューの先頭を定時間実行
        for (int count = 0; count < CONSTANT_TIME; count++)
        {
          UpdateSystemTime();
          if (queue.ExecuteFront(System_Time))
          {
            queue.Update(System_Time);
          }
        }
        // 更新
        queue.Update(System_Time);
        queue.TurnProcess();
      }

      Console.WriteLine("キューの消化が完了しました。");
    }
  }
}
