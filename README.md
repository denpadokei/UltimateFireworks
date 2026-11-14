# UltimateFireworks

[![Build and Release](https://github.com/denpadokei/UltimateFireworks/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/UltimateFireworks/actions/workflows/build-and-release.yml)

アニバーサリーステージの花火を大きく派手に置き換える MOD です。花火の規模、色、飛び方、効果音を設定画面から調整できます。

## 機能

- ゲーム標準のアニバーサリー花火の開始/停止処理を止め、花火の生成範囲・生成間隔を上書き
- 花火のサイズ、重力、放射 (radial)、床での跳ね返り、火花、トレイルを設定可能
- 爆発位置をプレイヤーの周囲 (InSide) と上空 (Normal) から選択
- 花火の色を毎回ランダムに生成
- `UserData` に置いた WAV ファイルを効果音セットとして読み込み、爆発音を差し替え
- 設定画面の Preview ボタンで花火をその場でプレビュー

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.39.1 |
| 依存 MOD | BSIPA 4.3.5 / BeatSaberMarkupLanguage 1.12.3 / SiraUtil 3.1.14 |

## インストール

1. Beat Saber を 1.39.1 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/UltimateFireworks/releases) から `UltimateFireworks-4.0.0-bs1.39.1.zip` をダウンロードし、`Plugins` フォルダをゲームフォルダへ上書きコピーする

## 使い方

Mod Settings の「UltimateFireworks」から設定を開きます。`Preview` ボタンを押すと花火の再生/停止を切り替えられるので、設定を変えながら確認できます。

効果音を差し替える場合は、`UserData/UltimateFireworks/Sound/<セット名>/` フォルダを作り、その中に `.wav` ファイルを置きます。フォルダ名がそのまま効果音セット名になり、設定画面の `Sound set` の一覧に表示されます。`Default` はゲーム標準の爆発音です。ファイルはゲーム起動時に読み込まれるため、追加した場合は Beat Saber を再起動してください。

## 設定

| 項目 | 型 | 既定値 | 説明 |
| --- | --- | --- | --- |
| FireEnable | bool | false | 火花 (パーティクル) を表示するか |
| TraileEnable | bool | true | 花火の軌跡 (トレイル) を表示するか |
| Refrect | bool | true | パーティクルを床やオブジェクトで跳ね返らせるか |
| Mode | FireWorksMode | InSide | 爆発位置。InSide はプレイヤーの周囲、Normal は上空 |
| Scale | int | 10 | 花火の大きさ (倍率) |
| GravityModifierMultiplier | float | 20 | 重力の倍率 |
| Radial | float | 0 | 放射状に広がる強さ。0 で無効 |
| SoundSet | string | Default | 使用する効果音セット。`UserData/UltimateFireworks/Sound` のフォルダ名、または `Default` |

設定は `UserData/UltimateFireworks.json` に保存されます。

## ビルド

```powershell
git clone https://github.com/denpadokei/UltimateFireworks.git
cd UltimateFireworks
dotnet build .\UltimateFireworks\UltimateFireworks.csproj -c Release
```

`BeatSaberDir` に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

MIT
