using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hinoki.Core.Db;

namespace Hinoki.Core
{
    /// <summary>
    /// データベースから記事を取得するクラス
    /// </summary>
    public class ArticleFetcher
    {
        /// <summary>
        /// 公開中の記事を取得します
        /// </summary>
        /// <param name="dbConnectionString">データベース接続文字列</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="pageNumber">ページ番号</param>
        /// <param name="totalHitCount">ヒットした記事の総数</param>
        /// <param name="publicStatus">Publicだけの場合は指定しないでよい。Public, Privateの両方をとる場合はPublicStatus.Privateを指定する</param>
        /// <returns></returns>
        static public List<Article> GetArticles(string dbConnectionString, int pageSize, int pageNumber, out int totalHitCount, PublicStatus publicStatus = PublicStatus.Public)
        {
            totalHitCount = 0;
            List<Article> articles = new List<Article> { };
            using (HNKArticleResource resource = new HNKArticleResource(dbConnectionString))
            {
                articles = resource.GetArticles(pageSize, pageNumber, out totalHitCount, publicStatus);
            }

            return articles;
        }

        /// <summary>
        /// 公開中の記事を取得します
        /// </summary>
        /// <param name="dbConnectionString">データベース接続文字列</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="pageNumber">ページ番号</param>
        /// <param name="totalHitCount">ヒットした記事の総数</param>
        /// <param name="publicStatus">Publicだけの場合は指定しないでよい。Public, Privateの両方をとる場合はPublicStatus.Privateを指定する</param>
        /// <returns></returns>
        static public List<Article> GetArticles(string dbConnectionString, int pageSize, int pageNumber, out int totalHitCount, string keyword, PublicStatus publicStatus = PublicStatus.Public)
        {
            totalHitCount = 0;
            List<Article> articles = new List<Article> { };
            using (HNKArticleResource resource = new HNKArticleResource(dbConnectionString))
            {
                string[] words = keyword.Split(' ', '　');
                List<string> keywords = words.ToList();
                articles = resource.GetArticles(pageSize, pageNumber, out totalHitCount, keywords, publicStatus);
            }

            return articles;
        }

        /// <summary>
        /// 公開中の記事を取得します
        /// </summary>
        /// <param name="dbConnectionString">データベース接続文字列</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="pageNumber">ページ番号</param>
        /// <param name="totalHitCount">ヒットした記事の総数</param>
        /// <param name="publicStatus">Publicだけの場合は指定しないでよい。Public, Privateの両方をとる場合はPublicStatus.Privateを指定する</param>
        /// <returns></returns>
        static public List<Article> GetArticlesWithDate(string dbConnectionString, int pageSize, int pageNumber, out int totalHitCount, int year, int month, PublicStatus publicStatus = PublicStatus.Public)
        {
            totalHitCount = 0;

            List<Article> articles = new List<Article> { };
            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(dbConnectionString))
                {
                    articles = resource.GetPublicArticlesByDate(year, month, pageSize, pageNumber, out totalHitCount);
                }
            }
            catch (Exception e)
            {
            }

            return articles;
        }

        /// <summary>
        /// 記事を追加します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <param name="article">更新後のカテゴリ</param>
        /// <returns>成功した場合に真を返す</returns>
        static public bool InsertArticle(string dbConnectionString, Article article)
        {
            if (article == null || article.Id > 0)
            {
                return false;
            }

            bool result = false;
            using (HNKArticleResource resource = new HNKArticleResource(dbConnectionString))
            {
                result = resource.InsertArticle(article);
            }

            return result;
        }

        /// <summary>
        /// 記事を更新します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <param name="article">更新後のカテゴリ</param>
        /// <returns>成功した場合に真を返す</returns>
        static public bool UpdateArticle(string dbConnectionString, Article article)
        {
            if (article == null || article.Id < 1)
            {
                return false;
            }

            bool result = false;
            using (HNKArticleResource resource = new HNKArticleResource(dbConnectionString))
            {
                result = resource.UpdateArticle(article);
            }

            return result;
        }
    }
}