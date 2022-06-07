namespace Scheduler.src.core
{
  class PseudoProcess
  {
    /// <summary>
    /// プロセスの名前
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 処理が到着する時刻
    /// </summary>
    public uint Arrival_Time { get; }

    /// <summary>
    /// 処理が終了した時刻
    /// </summary>
    public uint End_Time { get { return _end_time; } }
    private uint _end_time;

    /// <summary>
    /// 応答時間
    /// </summary>
    public uint Turn_Around_Time
    {
      get
      {
        if (IsDone)
          return End_Time - Arrival_Time;
        else
          throw new Exception("処理の完了していないプロセスの応答時間を計算することはできません");
      }
    }

    /// <summary>
    /// 処理にかかる時間
    /// </summary>
    public uint Time_to_Process { get; }

    /// <summary>
    /// 処理が完了しているかどうか
    /// </summary>
    public bool IsDone { get { return _isDone; } }
    private bool _isDone;

    private uint _progress;

    /// <summary>
    /// 処理の進捗
    /// </summary>
    public uint Progress { get { return _progress; } }

    /// <summary>
    /// 疑似プロセスを表現するクラス
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="arrival_time">到着時刻</param>
    /// <param name="time_to_process">処理を完了するために必要な時間</param>
    public PseudoProcess(string name, uint arrival_time, uint time_to_process)
    {
      Name = name;
      Arrival_Time = arrival_time;
      Time_to_Process = time_to_process;
      _progress = 0;
      _isDone = false;
    }

    /// <summary>
    /// プロセスを単位時間実行する
    /// </summary>
    public void Execute(uint system_time)
    {
      // 既に完了済みなら何もしない
      if (IsDone)
        return;

      // 進捗を更新する
      _progress++;

      System.Console.Write("t={0}| プロセス:{1} ({2}/{3}) | ", system_time, Name, Progress, Time_to_Process);

      // プロセスが終了しているかを判定する
      if (Time_to_Process == Progress)
      {
        _isDone = true;
        _end_time = system_time;
        System.Console.WriteLine("完了 | 応答時間={0}", Turn_Around_Time);
        return;
      }

      System.Console.WriteLine("");//改行
    }

    /// <summary>
    /// List型の機能で到着時刻をもとにSortできるようにするための関数
    /// </summary>
    /// <param name="pp">比較対象</param>
    public int CompareArrivalTimeTo(PseudoProcess pp)
    {
      if (Arrival_Time == pp.Arrival_Time)
        return 0;
      else if (Arrival_Time > pp.Arrival_Time)
        return 1;
      else
        return -1;
    }

    /// <summary>
    /// List型の機能で処理時間をもとにSortできるようにするための関数
    /// </summary>
    /// <param name="pp">比較対象</param>
    public int CompareTimeToProcessTo(PseudoProcess pp)
    {
      if (Time_to_Process == pp.Time_to_Process)
        return 0;
      else if (Time_to_Process > pp.Time_to_Process)
        return 1;
      else
        return -1;
    }
  }
}
