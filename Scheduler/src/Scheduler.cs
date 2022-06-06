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
    private List<PseudoProcess> _standby_queue;
    private List<PseudoProcess> _incomplete_queue;
    private List<PseudoProcess> _completed_queue;
    private int _num_of_processes;
    private uint _system_time;

    private const uint CONSTANT_TIME = 2;

    public Scheduler(AlgorithmType algorithm)
    {
      Processes = new();
      _standby_queue = new();
      _incomplete_queue = new();
      _completed_queue = new();
      GetInput();
      Run(algorithm);
    }

    public void Run(AlgorithmType algorithm)
    {
      // キューを初期化
      _standby_queue = new(Processes);
      _incomplete_queue = new();
      _completed_queue = new();
      // 時刻を初期化
      _system_time = 0;
      // 到着時刻順にソート
      _standby_queue.Sort((a, b) => a.CompareArrivalTimeTo(b));

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

      // キューを順番に実行
      while (true)
      {
        // 待機キューも未完了キューも空の場合終了する
        if ((_incomplete_queue.Count() + _standby_queue.Count() == 0))
          return;

        // 未完了キューを更新
        MoveArrivalProcess();
        _system_time++;

        // まだプロセスが到着していない場合は到着するまで時間を進める
        if (_incomplete_queue.Count() == 0)
          continue;

        // キューの先頭を実行
        _incomplete_queue[0].Execute(_system_time);

        // プロセスが完了したら完了済みキューに追加し、未完了キューから削除する
        if (_incomplete_queue[0].isDone)
        {
          _completed_queue.Add(_incomplete_queue[0]);
          _incomplete_queue.RemoveAt(0);
        }
      }
    }

    /// <summary>
    /// 処理時間順
    /// </summary>
    private void SPT()
    {
      System.Console.WriteLine("処理時間順でプロセスを消化します");

      // 初めのプロセスを読み込む
      _incomplete_queue.Add(_standby_queue[0]);
      _standby_queue.RemoveAt(0);
      _system_time = _incomplete_queue[0].Arrival_Time;

      // キューを処理時間順に実行
      while (true)
      {
        // 待機キューも未完了キューも空の場合終了する
        if ((_incomplete_queue.Count() + _standby_queue.Count() == 0))
          break;

        // まだプロセスが到着していない場合は到着するまで時間を進める
        if (_incomplete_queue.Count() == 0)
        {
          // 未完了キューを更新
          MoveArrivalProcess();
          _system_time++;
        }

        // 未完了キューを実行時間順にソート
        _incomplete_queue.Sort((a, b) => a.CompareTimeToProcessTo(b));

        // プロセスが完了するまで実行し続ける
        while (true)
        {
          _system_time++;
          // 未完了キューを更新
          MoveArrivalProcess();
          // 先頭のプロセスを実行
          _incomplete_queue[0].Execute(_system_time);
          if (_incomplete_queue[0].isDone)
          {
            // 完了済みキューに追加し、未完了キューから削除する
            _completed_queue.Add(_incomplete_queue[0]);
            _incomplete_queue.RemoveAt(0);
            break;
          }
        }
      }
    }

    /// <summary>
    /// ラウンドロビン
    /// </summary>
    /// <param name="const_time">定時間</param>
    private void RR(uint const_time)
    {
      System.Console.WriteLine("ラウンドロビン式でプロセスを消化します");

      // 初めのプロセスを読み込む
      _incomplete_queue.Add(_standby_queue[0]);
      _standby_queue.RemoveAt(0);
      _system_time = _incomplete_queue[0].Arrival_Time;

      int count = 0;
      while (true)
      {
        // 待機キューも未完了キューも空の場合終了する
        if ((_incomplete_queue.Count() + _standby_queue.Count() == 0))
          return;

        _system_time++;

        // 未完了キューを更新
        MoveArrivalProcess();

        // まだプロセスが到着していない場合は到着するまで時間を進める
        if (_incomplete_queue.Count() == 0)
          continue;

        count++;
        // 先頭のプロセスを実行
        _incomplete_queue[0].Execute(_system_time);

        if (_incomplete_queue[0].isDone)
        {
          // 処理を完了した場合、完了済みキューに追加し、未完了キューから削除する
          count = 0;
          _completed_queue.Add(_incomplete_queue[0]);
          _incomplete_queue.RemoveAt(0);
        }
        else if (count == const_time)
        {
          // 定時間が経過したら実行中のプロセスを一番後ろに回す
          count = 0;
          _incomplete_queue.Add(_incomplete_queue[0]);
          _incomplete_queue.RemoveAt(0);
        }
      }
    }

    /// <summary>
    /// _system_timeに到着したキューを探索して未完了キューに移動させる関数
    /// </summary>
    private void MoveArrivalProcess()
    {
      if (_standby_queue.Count() == 0)
        return;

      List<int> list = new();
      for (int i = 0; i < _standby_queue.Count(); i++)
      {
        if (_standby_queue[i].Arrival_Time == _system_time)
        {
          // 到着したキューを未完了キューに移動させる
          _incomplete_queue.Add(_standby_queue[i]);
          list.Add(i);
        }
        else if (_standby_queue[i].Arrival_Time > _system_time)
          // これ以上先に到着したプロセスがない場合に探索を終了する
          break;
      }

      // 移動済みのプロセスを待機列から削除
      foreach (int idx in list)
        _standby_queue.RemoveAt(idx);
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
