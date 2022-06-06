namespace Scheduler.src
{
  public enum AlgorithmType : ushort
  {
    FCFS = 0,
    SPT = 1,
    RR = 2
  }

  class Scheduler
  {
    private List<PseudoProcess> Processes;
    private List<PseudoProcess> _incomplete_queue;
    private List<PseudoProcess> _completed_queue;
    private int _num_of_processes;
    private uint _system_time;

    private const uint CONSTANT_TIME = 2;

    public Scheduler(AlgorithmType algorithm)
    {
      Processes = new();
      _incomplete_queue = new();
      _completed_queue = new();
      GetInput();
      Run(algorithm);
    }

    public void Run(AlgorithmType algorithm)
    {
      // キューを初期化
      _incomplete_queue = new(Processes);
      _completed_queue = new();
      // 時刻を初期化
      _system_time = 0;

      switch (algorithm)
      {
        case AlgorithmType.FCFS:
          FCFS();
          break;
        case AlgorithmType.SPT:
          SPT();
          break;
        case AlgorithmType.RR:
          RR(CONSTANT_TIME);
          break;
        default:
          throw new Exception("存在しないアルゴリズムが選択されました");
      }

      // 平均応答時間を計算する
      float Average_Turn_Around_Time = 0;
      foreach (PseudoProcess pp in _completed_queue)
        Average_Turn_Around_Time += pp.Turn_Around_Time;
      Average_Turn_Around_Time /= _completed_queue.Count();
      System.Console.WriteLine("キューの消化が完了しました。");
      System.Console.WriteLine("平均応答時間:" + Average_Turn_Around_Time);
    }

    /// <summary>
    /// 到着順
    /// </summary>
    private void FCFS()
    {
      System.Console.WriteLine("到着順でプロセスを消化します");
      _incomplete_queue.Sort((a, b) => a.CompareArrivalTimeTo(b));
      ExecuteAll();
    }

    /// <summary>
    /// 処理時間順
    /// </summary>
    private void SPT()
    {
      System.Console.WriteLine("処理時間順でプロセスを消化します");
      _incomplete_queue.Sort((a, b) => a.CompareTimeToProcessTo(b));
      ExecuteAll();
    }

    /// <summary>
    /// _incomplete_queueを初めから順番に消化する関数
    /// FCFSとSPTでは、プロセスを並べ替えた後の共通処理
    /// </summary>
    private void ExecuteAll()
    {
      while (_incomplete_queue.Count() != 0)
      {
        // まだプロセスが到着していない場合は到着するまで時間を進める
        if (_incomplete_queue[0].Arrival_Time > _system_time)
          _system_time = _incomplete_queue[0].Arrival_Time;
        // プロセスが完了するまで実行し続ける
        _incomplete_queue[0].Execute(_incomplete_queue[0].Time_to_Process, ref _system_time);
        // 完了済みキューに追加し、未完了キューから削除する
        _completed_queue.Add(_incomplete_queue[0]);
        _incomplete_queue.RemoveAt(0);
      }
    }

    /// <summary>
    /// ラウンドロビン
    /// </summary>
    /// <param name="const_time">定時間</param>
    private void RR(uint const_time)
    {
      System.Console.WriteLine("ラウンドロビン式でプロセスを消化します");
      // 到着時刻順にソート
      _incomplete_queue.Sort((a, b) => a.CompareArrivalTimeTo(b));
      // 初めのキューが来るまで時間を進める
      _system_time = _incomplete_queue[0].Arrival_Time;
      while (_incomplete_queue.Count() != 0)
      {
        // キューの長さが2以上ならラウンドロビンで処理する
        if (_incomplete_queue.Count() > 2)
          for (int i = 0; i < _incomplete_queue.Count(); i++)
          {
            // まだプロセスが到着していない場合は初めに戻る
            if (_incomplete_queue[i].Arrival_Time > _system_time)
              break;
            _incomplete_queue[i].Execute(const_time, ref _system_time);
            if (_incomplete_queue[i].isDone)
            {
              // 処理を完了した場合、完了済みキューに追加し、未完了キューから削除する
              _completed_queue.Add(_incomplete_queue[i]);
              _incomplete_queue.RemoveAt(i);
            }
          }
        // キューが残り一つの場合、ラウンドロビンをやめる
        else
        {
          // まだプロセスが到着していない場合は到着するまで時間を進める
          if (_incomplete_queue[0].Arrival_Time > _system_time)
            _system_time = _incomplete_queue[0].Arrival_Time;
          // プロセスが完了するまで実行し続ける
          _incomplete_queue[0].Execute(_incomplete_queue[0].Time_to_Process, ref _system_time);
          // 完了済みキューに追加し、未完了キューから削除する
          _completed_queue.Add(_incomplete_queue[0]);
          _incomplete_queue.RemoveAt(0);
        }
      }
    }

    /// <summary>
    /// プロセスのリストを標準入力で受け取る
    /// </summary>
    private void GetInput()
    {
      System.Console.WriteLine("以下の形式で入力してください (N:プロセスの数、Sn:名前、An:到着時刻、Bn:処理時間)");
      System.Console.WriteLine("N");
      System.Console.WriteLine("S1 A1 B1");
      System.Console.WriteLine("...");
      System.Console.WriteLine("Sn An Bn");
      while (true)
      {
        string? input = null;
        while (input == null)
          input = Console.ReadLine();
        if (int.TryParse(input, out _num_of_processes))
          break;
        System.Console.WriteLine("必ず数字を入力してください");
      }
      for (int i = 0; i < _num_of_processes; i++)
      {
        string? input = null;
        while (input == null)
          input = Console.ReadLine();
        string[] list = input.Split(' ');
        PseudoProcess pp = new PseudoProcess(list[0], uint.Parse(list[1]), uint.Parse(list[2]));
        Processes.Add(pp);
      }
    }
  }
}
