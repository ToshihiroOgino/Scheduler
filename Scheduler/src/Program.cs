namespace Scheduler.src
{
    class Program
    {
        static void Main()
        {
            System.Console.WriteLine("使用するアルゴリズムを選択してください (FCFS = 0, SPT = 1, RR = 2)");
            // アルゴリズムを指定
            core.IOTools.GetAlgorithmType(out int algorithm_type);

            // 指定したアルゴリズムでスケジューラを作成して実行
            switch (algorithm_type)
            {
                case 0:
                    FCFS fcfs = new();
                    fcfs.Init();
                    fcfs.Run();
                    fcfs.ShowAverageTurnAroundTime();
                    break;
                case 1:
                    SPT spt = new();
                    spt.Init();
                    spt.Run();
                    spt.ShowAverageTurnAroundTime();
                    break;
                case 2:
                    RR rr = new();
                    rr.Init();
                    rr.Run();
                    rr.ShowAverageTurnAroundTime();
                    break;
                default:
                    throw new System.Exception("存在しないアルゴリズムを指定しようとしました");
            }
            return;
        }
    }
}
