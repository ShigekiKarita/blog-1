var urlDef = { root: 'http://localhost:44300', update: '/admin/article' }
// https://localhost:44300/admin/article/18

function UpdateArticle(id) {
    var url = _getArticleUpdateUrl();
    var data = { id: id };

    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        data: data,
        success: function (data, textStatus) {
            console.log('success')
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('error')
        },
        complete: function () {
        }
    });
}

function _getUrl(path) {
    /// <summary>URLを返します。</summary>
    /// <param name="path" type="String">URLのパス部分。先頭にはスラッシュを付け、末尾には付けないこと。</param>
    /// <returns type="String">base 引数pathがNULLもしくは未指定の場合はベースのURLのみを返します。</returns>

    var root = urlDef.root;

    var base = '';
    if (root.length > 0) {
        if (root.slice(-1) == '/') {
            base = root.substring(0, (root.length - 1));
        } else {
            base = root;
        }
    }

    if (path != null && path.length > 0) {
        return base + path;
    } else {
        if (base.length == 0) {
            base = '/';
        }
    }
    return base;
}

function _getArticleUpdateUrl() {
    /// <summary>記事更新用URLを返します。</summary>
    /// <returns type="String">記事更新用URLの文字列</returns>
    return _getUrl(urlDef.update)
}