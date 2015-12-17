using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hinoki.Core
{
    /// <summary>
    /// カテゴリをデータベースから取得するクラス
    /// </summary>
    public class CategoryFetcher
    {
        /// <summary>
        /// カテゴリを取得します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <param name="publicStatus">Publicなものだけをとるか、PublicとPrivateなものをとるか。規定値はPublicなもののみ。PublicStatus.Privateにするとすべてとる</param>
        /// <returns>カテゴリのリスト</returns>
        static public List<Category> GetCategories(string dbConnectionString, PublicStatus publicStatus = PublicStatus.Public)
        {
            List<Category> categories = new List<Category> { };
            using (CategoryResource resource = new CategoryResource(dbConnectionString))
            {
                categories = resource.GetCategories(publicStatus);
            }
            return categories;
        }

        /// <summary>
        /// 指定されたカテゴリを取得します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <param name="id">カテゴリーID</param>
        /// <param name="publicStatus">Publicなものだけをとるか、PublicとPrivateなものをとるか。規定値はPublicなもののみ。PublicStatus.Privateにするとすべてとる</param>
        /// <returns>カテゴリ</returns>
        static public Category GetCategory(string dbConnectionString, int id, PublicStatus publicStatus = PublicStatus.Public)
        {
            Category category = null;
            using (CategoryResource resource = new CategoryResource(dbConnectionString))
            {
                category = resource.GetCategory(id, publicStatus);
            }
            return category;
        }

        /// <summary>
        /// カテゴリを更新します
        /// </summary>
        /// <param name="dbConnectionString">接続文字列</param>
        /// <param name="category">更新後のカテゴリ</param>
        /// <returns>成功した場合に真を返す</returns>
        static public bool UpdateCategory(string dbConnectionString, Category category)
        {
            if (category == null || category.Id < 1)
            {
                return false;
            }

            bool result = false;
            using (CategoryResource resource = new CategoryResource(dbConnectionString))
            {
                 result = resource.UpdateCategory(category);
            }

            return result;
        }
    }
}
