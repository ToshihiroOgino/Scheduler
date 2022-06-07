Scheduler
====

## Description

入力された疑似プロセスの集合をいくつかのアルゴリズムで実行するプログラム

## Demo

```planetext
\Scheduler> dotnet run
使用するアルゴリズムを選択してください (FCFS = 0, SPT = 1, RR = 2)
0
以下の形式で入力してください (N:プロセスの数、Sn:名前、An:到着時刻、Bn:処理時間)
N
S1 A1 B1
...
Sn An Bn
4
A 0 2
B 1 10
C 3 7
D 5 3
到着順でプロセスを消化します
t=1| プロセス:A (1/2) | 
︙
t=22| プロセス:D (3/3) | 完了 | 応答時間=17
キューの消化が完了しました。
平均応答時間:11.5
```

## Requirement

おそらく .NET 6.0

## Usage

##### 1. Scheduler.csprojがあるディレクトリで以下のコマンドを実行する

コマンド

```planetext
>dotnet run
```

##### 2. 指示に従って入力を行う

入力例

```planetext
0
4
A 0 2
B 1 10
C 3 7
D 5 3
```

## Licence

[MIT](https://github.com/tcnksm/tool/blob/master/LICENCE)

## Author

[Toshihiro Ogino](https://github.com/ToshihiroOgino)
