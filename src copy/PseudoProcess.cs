namespace Scheduler.src
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
    /// 処理を開始した時刻
    /// </summary>
    public uint Start_Time { get { return _start_time; } }
    private uint _start_time;

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
        if (isDone)
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
    public bool isDone { get { return _isDone; } }
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
    /// プロセスを実行する
    /// </summary>
    /// <param name="system_time">実行時の時間</param>
    /// <param name="given_time">実行可能時間</param>
    public void Execute(uint given_time, ref uint system_time)
    {
      // すでに実行が完了されているか、まだプロセスが到着していない場合は何もしない
      if (_isDone || system_time < Arrival_Time || given_time == 0)
        return;

      // 初めてプロセスを実行する時間を記録する
      if (Progress == 0)
        _start_time = system_time;

      System.Console.Write("プロセス:{0}を実行 (Time={1}) | ", Name, system_time);

      // 完了に必要な時間が実行可能時間以上なら実行可能時間、未満なら必要な時間だけ進める
      uint useTime = 0, timeLeft = Time_to_Process - Progress;
      if (given_time <= timeLeft)
        useTime = given_time;
      else
        useTime = timeLeft;
      _progress += useTime;
      system_time += useTime;
      if (Time_to_Process == Progress)
      {
        _isDone = true;
        _end_time = system_time;
        System.Console.WriteLine("完了 (Time={0}) | 応答時間:{1}", system_time, Turn_Around_Time);
        return;
      }
      System.Console.WriteLine("中断:{0}/{1}", Progress, Time_to_Process);
      return;
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
