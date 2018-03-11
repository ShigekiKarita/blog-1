+++
title = "CyberduckでAWS S3にファイルをアップロード"
date = "2016-06-19T06:30:00+09:00"
tags = [ "aws", "s3", "cyberduck" ]
+++

[Cyberduck](https://cyberduck.io/index.ja.html)を使えば、フォルダ構造を維持して、AWS S3にファイルをアップロードできる。
ファイルのアップロード自体はAWS S3のフロントエンドのウェブページで出来るが、フォルダ構造は自分で作る必要があり、やや手間がかかる。

<p style="text-align:center;">
<img style="max-width:60%" src="https://c5.staticflickr.com/9/8880/27571840164_47841c9457_z.jpg" alt="AWS S3のフロントエンドのウェブページ" title="AWS S3のフロントエンドのウェブページ">
</p>

実際のところ、今まではAWS S3をお試し程度の利用であったため、アップロード作業時に不便さを感じていなかったが、ウェブサイトをHugoで生成するようになってから、フォルダ構造を維持したまま、一括でアップロードしたくて、アップロードしたくて震えるようになった。

なお、本件に新鮮さはない。すでに別のブログで取り扱われている題材である。下記に参考資料としてまとめた。

- [Amazon S3 再入門 – AWS IAMでアクセスしてみよう！(Cyberduck 編) | cloudpack技術情報サイト](https://blog.cloudpack.jp/2014/08/12/revival-amazon-s3-using-aws-iam-with-cyberduck/)
- [Amazon S3 専用のアカウントを作ってみよう (フェンリル | デベロッパーズブログ)](http://blog.fenrir-inc.com/jp/2013/01/amazon-s3-iam.html)

## CyberduckとIAMユーザー

<p style="text-align:center;">
<img src="https://c4.staticflickr.com/9/8676/28109665331_501ef415ef_z.jpg" alt="CyberduckにAWS S3の接続設定を追加する" title="CyberduckにAWS S3の接続設定を追加する">
</p>

上図はCyberduckでAWS S3に接続する際の設定画面だ。この画面では以下を入力する。

- アクセスキーID
- シークレットアクセスキー

これらのキーを入力すれば、AWS S3に接続できるのだが、そのキーはIAMユーザー作成時に入手できる。IAMユーザーはAWS Identity and Access Managementで作成できる。IAMについては下記を参照されたい。

- [IAM とは - AWS Identity and Access Management](http://docs.aws.amazon.com/ja_jp/IAM/latest/UserGuide/introduction.html)

## IAMユーザーの作成

IAMユーザーにはユーザーとグループの概念があるので、まだ一度も作っていない場合は、下記の手順で進むことになる。

1. グループ作成
2. ユーザー作成
3. IAMユーザーのパスワードを設定する
4. IAMユーザーでログインする

### グループ作成

まずはグループを作る。小規模ならユーザーごとにポリシーを適用するだけで済むが、概念化できないので、確認ミスが起きやすい。それを回避するためにグループの定義が要る。

グループの作成手順は以下の通り。

1. AWSコンソールからIdentity & Access Managementにアクセスする
2. Identity & Access Managementのサイドメニューにある「グループ」をクリックする
3. 「新しいグループの作成」をクリックする。「新しいグループの作成ウィザード」が表示される
 1. グループ名を入力する
 2. ポリシーをアタッチする (今回は「AmazonS3FullAccess」を選択した)
 3. 確認画面で入力内容を見直す

参考までに、以下に図を残す。

AWSコンソールからIdentity & Access Managementにアクセスする。

<p style="text-align:center;">
<img src="https://c3.staticflickr.com/9/8861/28084780962_eea056b133_z.jpg" alt="Identity & Access Management" title="Identity & Access Management">
</p>

Identity & Access Managementのサイドメニューにある「グループ」をクリックする。「新しいグループの作成」をクリックする。「新しいグループの作成ウィザード」が表示される。

<p style="text-align:center;">
<img src="https://c4.staticflickr.com/9/8785/28110034091_c582c4c804_z.jpg" alt="新しいグループの作成" title="新しいグループの作成">
</p>

グループ名を入力する。

<p style="text-align:center;">
<img src="https://c2.staticflickr.com/9/8815/28187962945_2fc31cd1eb_z.jpg" alt="グループ名の入力" title="グループ名の入力">
</p>

アタッチするポリシーを選ぶ。

<p style="text-align:center;">
<img src="https://c3.staticflickr.com/9/8597/28153942546_22db28403d_z.jpg" alt="ポリシーのアタッチ" title="ポリシーのアタッチ">
</p>

最後に、確認画面が表示されるので、入力内容を見直し、グループを作成する。

### ユーザー作成

ユーザーは、以下の手順で作成可能だ。

1. Identity & Access Managementのサイドメニューにある「ユーザー」をクリックする
2. 「新規ユーザーの作成」をクリックする
3. ユーザー名を入力し、「ユーザーごとにアクセスキーを生成」にチェックを入れたままにする
4. 作成したユーザーの認証情報を手元に控える

参考までに、以下に図を残す。

「新規ユーザーの作成」をクリックする。

<p style="text-align:center;">
<img src="https://c2.staticflickr.com/9/8763/28187966025_93a8de84b0_z.jpg" alt="新規ユーザーの作成" title="新規ユーザーの作成">
</p>

ユーザー名を入力する。「ユーザーごとにアクセスキーを生成」のチェックは入れたままにする。こうすることで、前述のアクセスキーIDとシークレットアクセスキーが生成される。

<p style="text-align:center;">
<img src="https://c2.staticflickr.com/9/8767/28110037641_dd3c9375c2_z.jpg" alt="ユーザー名の入力" title="ユーザー名の入力">
</p>

ユーザの認証情報を入手する。画面内の「ユーザーのセキュリティ認証情報を表示」もしくは「認証情報のダウンロード」をクリックすれば、内容を確認できる。

<p style="text-align:center;">
<img src="https://c8.staticflickr.com/9/8635/28187966095_822915bd98_z.jpg" alt="認証情報の取得" title="認証情報の取得">
</p>

### IAMユーザーのパスワードを設定する

パスワードの設定はIdentity & Access Managementのサイドメニューにある「ユーザー」から行える。手順を以下に示す。

1. パスワード設定をしたいユーザーを選ぶ
2. 「認証情報」タブをクリックする
3. 「パスワードの管理」をクリックする

設定するパスワードの強度は、「パスワードポリシー」によって決まる。
このポリシーはIdentity & Access Managementのサイドメニューにある「アカウント設定」で設定できる。
ポリシーは以下をはじめとする項目によって定義される。

- 少なくとも 1 つの大文字が必要
- 少なくとも 1 つの小文字が必要
- 少なくとも 1 つの数字が必要
- 少なくとも 1 つの英数字以外の文字が必要 
 - ! @ # $ % ^ & * ( ) _ + - = [ ] { } | '

パスワードポリシーに関する資料は下記にある。

- [IAM ユーザー用のアカウントパスワードポリシーの設定 - AWS Identity and Access Management](http://docs.aws.amazon.com/ja_jp/IAM/latest/UserGuide/id_credentials_passwords_account-policy.html)

### IAMユーザーでログインする

IAMユーザーは「ユーザーサインインページ」からログインする。このリンクはIdentity & Access Managementのダッシュボードに記載されているので、そちらを確認されたい。サインインページについては下記を参照されたい。

- [IAM コンソールとサインインページ - AWS Identity and Access Management](http://docs.aws.amazon.com/ja_jp/IAM/latest/UserGuide/console.html)

ログインすると、ユーザーのパスワード再設定を要求される可能性がある。その場合は、パスワードポリシーに従って、新しくパスワードを設定する。
