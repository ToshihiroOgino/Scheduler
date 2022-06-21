using System.Collections.Generic;
using Scheduler.src;

namespace Scheduler.src.core
{
    /// <summary>
    /// すべてのプロセスを待機、実行、完了の3つのキューに分けて管理するクラス
    /// </summary>
    class ProsessQueue
    {
        /// <summary>
        /// まだ到着していないプロセスの待機キュー
        /// </summary>
        private List<PseudoProcess> _standby;
        /// <summary>
        ///  実行中の未完了キュー
        /// </summary>
        private List<PseudoProcess> _incomplete;
        /// <summary>
        /// 完了済みプロセスのキュー
        /// </summary>
        private List<PseudoProcess> _completed;

        public ProsessQueue()
        {
            _standby = new();
            _incomplete = new();
            _completed = new();
        }

        /// <summary>
        /// 待機と実行中のキューが空かどうか
        /// </summary>
        /// <returns>待機と実行中のキューがどちらも空ならtruetrue</returns>
        public bool IsEmpty { get { return (_incomplete.Count == 0) && (_standby.Count == 0); } }

        /// <value>未完了キューの要素数</value>
        public int GetIncompleteQueueCount { get { return _incomplete.Count; } }

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
            int Num_Of_Processes = num;
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
            _standby.Add(new PseudoProcess(name, arrival_time, time_to_process));
        }

        /// <summary>
        /// 未完了キューの先頭を実行する関数
        /// </summary>
        /// <param name="system_time">現在の時刻</param>
        /// <returns>完了したときtrueを返す</returns>
        public bool ExecuteFront(uint system_time)
        {
            if (_incomplete[0].IsDone)
                return true;
            _incomplete[0].Execute(system_time);
            return _incomplete[0].IsDone;
        }

        /// <summary>
        /// 先頭プロセスを後ろに回す関数
        /// </summary>
        public void TurnProcess()
        {
            if (_incomplete.Count == 0)
                return;

            if (_incomplete[0].IsDone)
            {
                _completed.Add(_incomplete[0]);
                _incomplete.RemoveAt(0);
            }
            else
            {
                _incomplete.Add(_incomplete[0]);
                _incomplete.RemoveAt(0);
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
            for (int i = 0; i < _standby.Count; i++)
            {
                if (_standby[i].Arrival_Time <= system_time)
                {
                    // 到着したキューを未完了キューに移動させる
                    _incomplete.Add(_standby[i]);
                    _standby.RemoveAt(i);
                    i--;
                }
            }

            // 完了済みのタスクを削除
            for (int i = 0; i < _incomplete.Count; i++)
            {
                if (_incomplete[i].IsDone)
                {
                    _completed.Add(_incomplete[i]);
                    _incomplete.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 待機キューを到着順にソート
        /// </summary>
        public void SortByArrivalTime()
        {
            _standby.Sort((a, b) => a.CompareArrivalTimeTo(b));
        }

        /// <summary>
        /// 未完了キューを処理時間順にソート
        /// </summary>
        public void SortByTimeToProcess()
        {
            _incomplete.Sort((a, b) => a.CompareTimeToProcessTo(b));
        }

        /// <summary>
        /// 完了済みのキューの平均応答時間を計算する
        /// </summary>
        /// <returns>平均応答時間</returns>
        public float AverageTurnAroundTime()
        {
            float ave = 0;
            foreach (PseudoProcess p in _completed)
                ave += p.Turn_Around_Time;
            ave /= _completed.Count;
            return ave;
        }
    }
}
