using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hinoki.Core;
using Hinoki.Core.Db;

namespace Hinoki.Core.Tests
{
    [TestClass]
    public class ブログ記事を取得する
    {
        #region メンバ変数

        private TestContext _testContext;
        private string _dbName = "blog";
        private string _dbServerName = @"""HIROAKI-PC\SQLEXPRESS""";

        #endregion

        #region プロパティ

        public TestContext TestContext
        {
            get
            {
                return _testContext;
            }
            set
            {
                _testContext = value;
            }
        }

        #endregion

        [TestInitialize()]
        public void TestInitialize()
        {
            this.TestContext.WriteLine("テストで使用するデータを準備します...");
            try
            {
                this.TestContext.WriteLine("テーブルarticlesを作成します");
                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    string batFilePath = Path.Combine(TestConfig.SqlDir, "exe");
                    string sqlFilePath = Path.Combine(TestConfig.SqlDir, "GetPublicArticles", "SetupTable.sql");

                    process.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
                    process.StartInfo.Arguments = string.Format(@"/c {0} {1} {2} {3} {4} {5}", batFilePath, "sa", "hiroaki", _dbServerName, _dbName, sqlFilePath);
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    output.Replace("\r\r\n", "\n");
                    this.TestContext.WriteLine(output);
                }
            }
            catch (Exception e)
            {
                this.TestContext.WriteLine(e.ToString());
            }
        }

        [TestMethod]
        public void ID指定で特定のブログ記事を取得する()
        {
            try
            {
                this.TestContext.WriteLine("データベースにテストデータをインサートします");
                using (DataResource resource = new DataResource(TestConfig.GetDbConnectionString()))
                {
                    string filePath = Path.Combine(TestConfig.SqlDir, "GetPublicArticles", "insert1.sql");

                    string sql = string.Empty;
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        sql = reader.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(sql))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, resource.SqlConnection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.TestContext.WriteLine(e.ToString());
                Assert.Fail(e.ToString());
            }

            int id = 1;
            Article article = null;
            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(TestConfig.GetDbConnectionString()))
                {
                    article = resource.GetArticle(id);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }

            Assert.IsNotNull(article, "データベースから記事を取得できていない");
            Assert.IsTrue(article.PublicStatus == PublicStatus.Public, "未公開記事を取得している");
        }

        [TestMethod]
        public void 上位10件の最新ブログ記事を取得する()
        {
            try
            {
                this.TestContext.WriteLine("データベースにテストデータをインサートします");
                using (DataResource resource = new DataResource(TestConfig.GetDbConnectionString()))
                {
                    string filePath = Path.Combine(TestConfig.SqlDir, "GetPublicArticles", "insert2.sql");

                    string sql = string.Empty;
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        sql = reader.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(sql))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, resource.SqlConnection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.TestContext.WriteLine(e.ToString());
                Assert.Fail(e.ToString());
            }

            List<Article> articles = null;
            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(TestConfig.GetDbConnectionString()))
                {
                    //articles = resource.GetPublicArticles(10, 1);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }

            Assert.IsNotNull(articles, "データベースから記事を取得できていない");

            Article article1 = articles[0];
            Assert.IsTrue(article1.PublicStatus == PublicStatus.Public, "未公開記事を取得している");

            DateTime time = new DateTime(2014, 8, 23, 0, 0, 0);
            Assert.IsTrue(article1.OpenDate == time, "公開日が昇順になっている");
        }

        [TestMethod]
        public void 日付範囲2014年8月1日から2014年8月末までのブログ記事を取得する()
        {
            try
            {
                this.TestContext.WriteLine("データベースにテストデータをインサートします");
                using (DataResource resource = new DataResource(TestConfig.GetDbConnectionString()))
                {
                    string filePath = Path.Combine(TestConfig.SqlDir, "GetPublicArticles", "insert3.sql");

                    string sql = string.Empty;
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        sql = reader.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(sql))
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, resource.SqlConnection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.TestContext.WriteLine(e.ToString());
                Assert.Fail(e.ToString());
            }

            List<Article> articles = null;
            try
            {
                using (HNKArticleResource resource = new HNKArticleResource(TestConfig.GetDbConnectionString()))
                {
                    //articles = resource.GetPublicArticlesByDate(2014, 8);
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }

            Assert.IsNotNull(articles, "データベースから記事を取得できていない");
            Assert.IsTrue(IsEqulPublicArticles(articles), "未公開記事を取得している");

            Article article1 = articles[0];
            Assert.IsTrue(article1.OpenDate == new DateTime(2014, 8, 31, 0, 0, 0), "公開日が昇順になっている");
            Article article8 = articles[8];
            Assert.IsTrue(article8.OpenDate == new DateTime(2014, 8, 1, 0, 0, 0), "公開日が昇順になっている");
        }

        /// <summary>
        /// すべての記事が公開状態のものであれば真を返します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool IsEqulPublicArticles(IEnumerable<Article> value)
        {
            return value.All(article => article.PublicStatus == PublicStatus.Public);
        }
    }
}
