namespace Scheduler.src
{
  class Program
  {
    static void Main(string[] args)
    {
      AlgorithmType algorithm = GetAlgorithmType();
      // 指定したアルゴリズムでスケジューラを作成
      Scheduler scheduler = new(algorithm);
    }

    /// <summary>
    /// AlgorithmTypeを標準入力で受け取る
    /// </summary>
    /// <returns>AlgorithmTypeのどれか</returns>
    private static AlgorithmType GetAlgorithmType()
    {
      System.Console.WriteLine("使用するアルゴリズムを選択してください (FCFS = 0, SPT = 1, RR = 2)");
      while (true)
      {
        string? input = null;
        ushort num = 100;
        while (input == null)
          input = Console.ReadLine();
        if (ushort.TryParse(input, out num) && (0 <= num && num <= 2))
          return (AlgorithmType)num;

        System.Console.WriteLine("必ず0～2の数字を入力してください");
      }
    }
  }
}
