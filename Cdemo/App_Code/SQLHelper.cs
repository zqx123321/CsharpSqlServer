using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// SQLHelper 的摘要说明
/// </summary>
public class SQLHelper
{
    public SQLHelper()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    public static string ConnStr = @"server=.;Integrated Security=True;database=demo;";
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="sql">用于查询的SQL语句</param>
    /// <returns>数据集</returns>
    static public DataTable Select(string sql)
    {
        //创建DataTable对象
        DataTable dt = new DataTable();
        //创建SqlConnection对象，在其构造函数里传入连接字符串
        SqlConnection conn = new SqlConnection(ConnStr);
        try
        {
            conn.Open();//打开
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);//进行填充
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            conn.Close();//关闭连接，释放资源
        }
        return dt;
    }
    /// <summary>
    /// 增删改操作
    /// </summary>
    /// <param name="sql">用于增删改的SQL语句</param>
    /// <returns></returns>
    static public int SqlExecute(string sql)
    {
        SqlConnection conn = new SqlConnection(ConnStr);
        try
        {
            conn.Open();
            //创建SqlCommand对象，在其构造函数里传入SQL语句和连接字符串
            SqlCommand cmd = new SqlCommand(sql, conn);
            return Convert.ToInt32(cmd.ExecuteNonQuery());//执行
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return 0;
        }
        finally
        {
            conn.Close();//关闭连接，释放资源
        }
    }

    /// <summary>
    /// 执行多条操作的事务
    /// </summary>
    /// <param name="lst">SQL语句集</param>
    /// <returns>事务成功托付返回true,否则返回false</returns>
    public bool SqlExecuteTrans(List<String> lst)
    {
        using (SqlConnection conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            //开启事务
            SqlTransaction trans = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;//添加链接工具
            cmd.Transaction = trans;//添加事务
            try
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    string sql = lst[i].ToString();//获取sql语句
                    cmd.CommandText = sql;//添加sql语句
                    cmd.ExecuteNonQuery();//执行
                }
                trans.Commit();//执行完成之后提交
                return true;
            }
            catch (Exception ex)
            {
                //执行sql语句失败，事务回滚
                trans.Rollback();
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}