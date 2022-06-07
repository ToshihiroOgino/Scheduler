using System.Collections.Generic;
using Scheduler.src;

namespace Scheduler.src.core
{
  /// <summary>
  /// すべてのプロセスを待機、実行、完了の3つのキューに分けて管理するクラス
  /// </summary>
  class ProsessQueue
  {
    public int Num_Of_Processes { get; set; }
    public List<PseudoProcess> Standby;
    public List<PseudoProcess> Incomplete;
    public List<PseudoProcess> Completed;

    public ProsessQueue()
    {
      Standby = new();
      Incomplete = new();
      Completed = new();
    }

    /// <summary>
    /// 待機と実行中のキューが空かどうか
    /// </summary>
    /// <returns>キューが空ならtrue</returns>
    public bool IsEmpty { get { return (Incomplete.Count == 0) && (Standby.Count == 0); } }

    /// <summary>
    /// キューの初期化
    /// </summary>
    public void Init()
    {
      GetInput();
      SortByArrivalTime();
    }

    /// <summary>
    /// プロセス集合を標準入力で受け取る
    /// </summary>
    private void GetInput()
    {
      System.Console.WriteLine("以下の形式で入力してください (N:プロセスの数、Sn:名前、An:到着時刻、Bn:処理時間)");
      System.Console.WriteLine("N");
      System.Console.WriteLine("S1 A1 B1");
      System.Console.WriteLine("...");
      System.Console.WriteLine("Sn An Bn");
      IOTools.GetNumOfProcesses(out int num);
      Num_Of_Processes = num;
      for (int i = 0; i < num; i++)
      {
        IOTools.GetProcessParameters(out string name, out uint at, out uint ttp);
        CreateProcess(name, at, ttp);
      }
    }

    /// <summary>
    /// 新しいプロセスを作成して待機列に追加する関数
    /// </summary>
    /// <param name="p">追加するプロセス</param>
    public void CreateProcess(string name, uint arrival_time, uint time_to_process)
    {
      Standby.Add(new PseudoProcess(name, arrival_time, time_to_process));
    }

    /// <summary>
    /// 未完了キューの先頭を実行する関数
    /// </summary>
    /// <param name="system_time">現在の時刻</param>
    /// <returns>完了したときtrueを返す</returns>
    public bool ExecuteFront(uint system_time)
    {
      if (Incomplete[0].IsDone)
        return true;
      Incomplete[0].Execute(system_time);
      return Incomplete[0].IsDone;
    }

    /// <summary>
    /// 先頭プロセスを後ろに回す関数
    /// </summary>
    public void TurnProcess()
    {
      if (Incomplete.Count == 0)
        return;

      if (Incomplete[0].IsDone)
      {
        Completed.Add(Incomplete[0]);
        Incomplete.RemoveAt(0);
      }
      else
      {
        Incomplete.Add(Incomplete[0]);
        Incomplete.RemoveAt(0);
      }
    }

    /// <summary>
    /// 到着または完了したプロセスを移動させる関数
    /// </summary>
    /// <param name="system_time"></param>
    public void Update(uint system_time)
    {
      if (IsEmpty)
        return;

      // 到着したプロセスを未完了キューに移動
      for (int i = 0; i < Standby.Count; i++)
      {
        if (Standby[i].Arrival_Time <= system_time)
        {
          // 到着したキューを未完了キューに移動させる
          Incomplete.Add(Standby[i]);
          Standby.RemoveAt(i);
          i--;
        }
      }

      // 完了済みのタスクを削除
      for (int i = 0; i < Incomplete.Count; i++)
      {
        if (Incomplete[i].IsDone)
        {
          Completed.Add(Incomplete[i]);
          Incomplete.RemoveAt(i);
          i--;
        }
      }
    }

    /// <summary>
    /// 待機キューを到着順にソート
    /// </summary>
    public void SortByArrivalTime()
    {
      Standby.Sort((a, b) => a.CompareArrivalTimeTo(b));
    }

    /// <summary>
    /// 未完了キューを処理時間順にソート
    /// </summary>
    public void SortByTimeToProcess()
    {
      Incomplete.Sort((a, b) => a.CompareTimeToProcessTo(b));
    }

    /// <summary>
    /// 完了済みのキューの平均応答時間を計算する
    /// </summary>
    /// <returns>平均応答時間</returns>
    public float AverageTurnAroundTime()
    {
      float ave = 0;
      foreach (PseudoProcess p in Completed)
        ave += p.Turn_Around_Time;
      ave /= Completed.Count;
      return ave;
    }
  }
}