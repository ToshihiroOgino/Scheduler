using System.Collections.Generic;
using Scheduler.src;

namespace Scheduler.src.core
{
  static class IOTools
  {
    /// <summary>
    /// AlgorithmTypeを標準入力で受け取る関数
    /// </summary>
    /// <param name="type">アルゴリズムの種類</param>
    public static void GetAlgorithmType(out int type)
    {
      while (true)
      {
        string? input = null;
        while (input == null)
          input = Console.ReadLine();
        if (int.TryParse(input, out type) && 0 <= type && type <= 2)
          return;

        System.Console.WriteLine("必ず0～2の数字を入力してください");
      }
    }

    /// <summary>
    /// プロセスの数を標準入力で受け取る
    /// </summary>
    /// <param name="num_of_processes">プロセスの数</param>
    public static void GetNumOfProcesses(out int num_of_processes)
    {
      while (true)
      {
        string? input = null;
        while (input == null)
          input = Console.ReadLine();
        if (int.TryParse(input, out num_of_processes))
          return;
        Console.WriteLine("必ず数字を入力してください");
      }
    }

    /// <summary>
    /// プロセスのリストを標準入力で受け取る関数
    /// </summary>
    /// <param name="num_of_processes">受け取ったプロセスの数</param>
    /// <param name="processList">受け取ったプロセスのリスト</param>
    public static void GetProcessParameters(out string name, out uint arrival_time, out uint time_to_process)
    {
      string? input = null;
      while (input == null)
        input = Console.ReadLine();
      string[] strArray = input.Split(' ');
      name = strArray[0];
      arrival_time = uint.Parse(strArray[1]);
      time_to_process = uint.Parse(strArray[2]);
    }
  }
}
