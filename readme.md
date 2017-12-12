## <center>C#连接SQL Server</center>

#### 1.配置连接字符串
>为什么需要连接字符串？  

C#中的ADO.NET类库为不同的外部数据源提供了一致的访问方法，这些数据源可以是本地的数据文件（如excel、txt、access，甚至是SQLite），也可以是远程的数据库服务器（如SQL Server、MySQL、DB2、Oracle等）。数据源似乎琳琅满目，鱼龙混杂，请试想一下，ADO.NET如何能够准确而又高效的访问到不同数据源呢？连接字符串，就是这样一组被格式化的键值对：它告诉ADO.NET数据源在哪里，需要什么样的数据格式，提供什么样的访问信任级别以及其他任何包括连接的相关信息。  
>连接字符串由一组元素组成，一个元素包含一个键值对，元素之间由“;”分开。语法如下：  

```
key1=value1;key2=value2;key3=value3...
```

>SQL Server常用连接字符串参数说明

| 参数 | 说明     |
| :------------- | :------------- |
|  Server 或 Data Source      | 要连接的数据库实例的名称或网络地址,也就是登录SQL Server时的服务器地址，指定本地实例可用(Local)或者本机IP地址，或者干脆一个. |
| Initial Catalog 或 Database   | 数据库的名称，也就是使用哪个数据库 |
| Integrated Security 或 Trusted_Connection   | 指定是Windows身份验证，还是SQL Server身份验证，值为true时，表示Windows身份验证，false表示SQL Server身份验证，默认false |
|User ID 或 UID   |   使用SQL Server身份验证时的登录用户名|
|Password 或 Pwd   | 使用SQL Server身份验证时的登录密码  |

>SQL Server常用连接字符串举例

```
windows身份验证
"Data Source=.;Initial Catalog=数据库;Integrated Security=True"

SQL Server身份验证(sa)：     
"Data Source=.;Initial Catalog=数据库;User ID=sa,pwd=;"
```
>PS：使用SQL Server身份验证需要设置sa登录密码，并赋予sa账号权限，连接远程服务器中的SQL Server时必须使用SQL Server身份验证，且开启TCP连接


#### 2.使用C#连接SQL Server


>SQL Server和C#是一家人，所谓近水楼台先得月，因此C#连接SQL Server非常容易，只需要几行代码：

```C#
//SqlConnection 对象表示与 SQL Server 数据源的一个唯一的会话。对于客户端/服务器数据库系统，它等效于到服务器的网络连接
public static string ConnStr = @"server=.;Integrated Security=SSPI;database=restaurant;";//连接字符串
SqlConnection conn = new SqlConnection(ConnStr);//连接数据库
conn.Open();//打开数据库
//doSomething
conn.Close();//关闭数据库，释放资源
```

#### 3.数据库操作类SQLHelper封装

>总结我们的需求，无非就是对数据库的增删改查，其实这四种操作可以分成两类：第一类是查询，我们希望得到查询的数据；第二类是增删改，我们希望得到操作的执行情况。因此，我们的通用数据库操作类只需要封装两个函数就够了

1、查询操作

>我们把查询的结果放在C#数据容器DataTable中，SqlDataAdapter是 DataTable和 SQL Server之间的适配器，用于向DataTable中填充数据

```C#
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

```
2、增删改操作

>sqlcommand是对SQL Server操作的类，其ExecuteNonQuery方法可以对连接执行 SQL 语句并返回受影响的行数，执行增删改时，返回大于0的数表示操作成功，0表示操作失败

```C#
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

```

3.使用事务

```C#
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
```

#### 4.SQLHelper.cs完整代码
``` C#
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
    public static string ConnStr = @"server=.;Integrated Security=SSPI;database=demo;";
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
```
