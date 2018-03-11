+++
title = "macOSで定刻にプログラムを実行させる"
date = "2015-06-13T20:30:00+09:00"
tags = [ "macOS", "launchd"]
+++

macOSで決まった時刻にプログラムを実行させたいので、UNIXやLinuxのcron、Windowsのタスクスケジューラに相当する機能がないか調べた。Mac Developer Libraryを覗くと、Scheduling Timed Jobsにジョブに関する記述があり、まとめると以下のようになる。

- プログラムを定時実行させる場合は、launchdジョブとcronジョブが使える
- 好ましいのはlaunchdのジョブを使うことである。cronは非推奨だ

## launchdジョブの定義

launchdジョブはプロパティリストファイル(.plist)で定義する。記述方法はA launchd Tutorialをはじめとするリンクを参考資料にまとめたので、そちらをみてほしい。

## launchdジョブの配置場所

launchdジョブは用途によって設置場所が異なる。

<p style="text-align:center;">launchdジョブ定義ファイルの配置場所</p>

配置場所                      | 用途
-------------                 | -------------
~/Library/LaunchAgents        | 各ユーザが管理するエージェントを設定
/Library/LaunchAgents	      | 管理者が管理するエージェントを設定
/Library/LaunchDaemons	      | 管理者が管理するデーモンを設定
/System/Library/LaunchAgents  | OSが管理するエージェントを設定
/System/Library/LaunchDaemons | OSが管理するデーモンを設定

デーモンはシステムレベルで実行されるジョブのことを指す。従ってroot権限を必要とする。そして、LaunchDaemonsディレクトリはユーザディレクトリには存在しない。エージェントはログインしているユーザ毎に実行されるジョブを指す。

例えば、特定のユーザ用のジョブなら、下記のようにコマンドを実行する。

<pre><code class="language-bash">$ launchctl load ~/Library/LaunchAgents/com.hiroakit.hoge.plist # 登録
$ launchctl list com.hiroakit.hoge # 確認
$ launchctl unload ~/Library/LaunchAgents/com.hiroakit.hoge.plist # 登録解除
</code></pre>  

## 実行したlaunchdジョブのログ

実行結果は、原則として/var/log/system.logに出力される。

ジョブを定義したプロパティリストで変更ができる。

## 参考資料

- [Daemons and Services Programming Guide - Scheduling Timed Jobs](https://developer.apple.com/library/mac/documentation/MacOSX/Conceptual/BPSystemStartup/Chapters/ScheduledJobs.html)
- [A launchd Tutorial](http://launchd.info/)
- [Mac OSにおける、initや/etc/rcやcronの代わりであるlaunchdの使い方 - kanonjiの日記](http://d.hatena.ne.jp/kanonji/20100621/1277075926)
- [Mac（OS X）ではcronじゃなくてlaunchdでやる - Furudateのブログ](http://furudate.hatenablog.com/entry/2014/11/09/155017)
- [launchd.plistの書き方 | Drowsy Dog's Diary](http://ka-zoo.net/2013/04/launchd-plist/)
