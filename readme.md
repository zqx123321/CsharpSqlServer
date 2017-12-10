## <center>C#����SQL Server</center>

#### 1.���������ַ���
>Ϊʲô��Ҫ�����ַ�����  

C#�е�ADO.NET���Ϊ��ͬ���ⲿ����Դ�ṩ��һ�µķ��ʷ�������Щ����Դ�����Ǳ��ص������ļ�����excel��txt��access��������SQLite����Ҳ������Զ�̵����ݿ����������SQL Server��MySQL��DB2��Oracle�ȣ�������Դ�ƺ�������Ŀ���������ӡ�������һ�£�ADO.NET����ܹ�׼ȷ���ָ�Ч�ķ��ʵ���ͬ����Դ�أ������ַ�������������һ�鱻��ʽ���ļ�ֵ�ԣ�������ADO.NET����Դ�������Ҫʲô�������ݸ�ʽ���ṩʲô���ķ������μ����Լ������κΰ������ӵ������Ϣ��  
>�����ַ�����һ��Ԫ����ɣ�һ��Ԫ�ذ���һ����ֵ�ԣ�Ԫ��֮���ɡ�;���ֿ����﷨���£�  

```
key1=value1;key2=value2;key3=value3...
```

>SQL Server���������ַ�������˵��

| ���� | ˵��     |
| :------------- | :------------- |
|  Server �� Data Source      | Ҫ���ӵ����ݿ�ʵ�������ƻ������ַ,Ҳ���ǵ�¼SQL Serverʱ�ķ�������ַ��ָ������ʵ������(Local)���߱���IP��ַ�����߸ɴ�һ��. |
| Initial Catalog �� Database   | ���ݿ�����ƣ�Ҳ����ʹ���ĸ����ݿ� |
| Integrated Security �� Trusted_Connection   | ָ����Windows�����֤������SQL Server�����֤��ֵΪtrueʱ����ʾWindows�����֤��false��ʾSQL Server�����֤��Ĭ��false |
|User ID �� UID   |   ʹ��SQL Server�����֤ʱ�ĵ�¼�û���|
|Password �� Pwd   | ʹ��SQL Server�����֤ʱ�ĵ�¼����  |

>SQL Server���������ַ�������

```
windows�����֤
"Data Source=.;Initial Catalog=���ݿ�;Integrated Security=True"

SQL Server�����֤(sa)��     
"Data Source=.;Initial Catalog=���ݿ�;User ID=sa,pwd=;"
```
>PSʹ��SQL Server�����֤��Ҫ����sa��¼���룬������sa�˺�Ȩ�ޣ�����Զ�̷������е�SQL Serverʱ����ʹ��SQL Server�����֤���ҿ���TCP����


#### 2.ʹ��C#����SQL Server


>SQL Server��C#��һ���ˣ���ν��ˮ¥̨�ȵ��£����C#����SQL Server�ǳ����ף�ֻ��Ҫ���д��룺

```C#
//SqlConnection �����ʾ�� SQL Server ����Դ��һ��Ψһ�ĻỰ�����ڿͻ���/���������ݿ�ϵͳ������Ч�ڵ�����������������
public static string ConnStr = @"server=.;Integrated Security=SSPI;database=restaurant;";//�����ַ���
SqlConnection conn = new SqlConnection(ConnStr);//�������ݿ�
conn.Open();//�����ݿ�
//doSomething
conn.Close();//�ر����ݿ⣬�ͷ���Դ
```

#### 3.���ݿ������SQLHelper��װ

>�ܽ����ǵ������޷Ǿ��Ƕ����ݿ����ɾ�Ĳ飬��ʵ�����ֲ������Էֳ����࣬��һ���ǲ�ѯ������ϣ���õ���ѯ�����ݣ��ڶ�������ɾ�ģ�����ϣ���õ�������ִ���������ˣ����ǵ�ͨ�����ݿ������ֻ��Ҫ��װ���������͹���

1����ѯ����

>���ǰѲ�ѯ�Ľ������C#��������DataTable�У�SqlDataAdapter�� DataTable�� SQL Server֮�����������������DataTable���������

```C#
/// <summary>
/// ��ѯ����
/// </summary>
/// <param name="sql">���ڲ�ѯ��SQL���</param>
/// <returns>���ݼ�</returns>
public DataTable Select(string sql)
{
   using (SqlConnection conn = new SqlConnection(ConnStr))
   {
       DataTable dt = new DataTable();
       {
           try
           {
               conn.Open();//��
               SqlDataAdapter da = new SqlDataAdapter(sql, conn);
               da.Fill(dt);//�������
           }
           catch(Exception ex)
           {
               Console.WriteLine(ex);
           }
       }
       return dt;
   }
}

```
2����ɾ�Ĳ���

>sqlcommand�Ƕ�SQL Server�������࣬��ExecuteNonQuery�������Զ�����ִ�� SQL ��䲢������Ӱ���������ִ����ɾ��ʱ������1��ʾ�����ɹ���0��ʾ����ʧ��

```C#
/// <summary>
/// ��ɾ�Ĳ���
/// </summary>
/// <param name="sql">������ɾ�ĵ�SQL���</param>
/// <returns></returns>
static public int SqlExecute(string sql)
{
   using (SqlConnection conn = new SqlConnection(ConnStr))
   {
       try
       {
           conn.Open();
           SqlCommand cmd = new SqlCommand(sql, conn);
           return Convert.ToInt32(cmd.ExecuteNonQuery());//ִ��
       }
       catch(Exception ex)
       {
           Console.WriteLine(ex);
           return 0;
       }
   }
}

```

3.ʹ������

```C#
/// <summary>
/// ִ�ж�������������
/// </summary>
/// <param name="lst">SQL��伯</param>
/// <returns>����ɹ��и�����true,���򷵻�false</returns>
public bool SqlExecuteTrans(List<String> lst)
{
    using (SqlConnection conn = new SqlConnection(ConnStr))
    {
        conn.Open();
        //��������
        SqlTransaction trans = conn.BeginTransaction();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;//������ӹ���
        cmd.Transaction = trans;//�������
        try
        {
            for (int i = 0; i < lst.Count; i++)
            {
                string sql = lst[i].ToString();//��ȡsql���
                cmd.CommandText = sql;//���sql���
                cmd.ExecuteNonQuery();//ִ��
            }
            trans.Commit();//ִ�����֮���ύ
            return true;
        }
        catch (Exception ex)
        {
            //ִ��sql���ʧ�ܣ�����ع�
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

#### 4.SQLHelper.cs��������
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
/// SQLHelper ��ժҪ˵��
/// </summary>
public class SQLHelper
{
    public SQLHelper()
    {
        //
        // TODO: �ڴ˴���ӹ��캯���߼�
        //
    }
    public static string ConnStr = @"server=(localdb)\v11.0;Integrated Security=SSPI;database=restaurant;";
    /// <summary>
    /// ��ѯ����
    /// </summary>
    /// <param name="sql">���ڲ�ѯ��SQL���</param>
    /// <returns>���ݼ�</returns>
    static public DataTable Select(string sql)
    {
        using (SqlConnection conn = new SqlConnection(ConnStr))
        {
            DataTable dt = new DataTable();
            try
            {
                conn.Open();//��
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);//�������
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dt;
        }
    }
    /// <summary>
    /// ��ɾ�Ĳ���
    /// </summary>
    /// <param name="sql">������ɾ�ĵ�SQL���</param>
    /// <returns></returns>
    static public int SqlExecute(string sql)
    {
        using (SqlConnection conn = new SqlConnection(ConnStr))
        {
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                return Convert.ToInt32(cmd.ExecuteNonQuery());//ִ��
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return 0;
            }
        }
    }

    /// <summary>
    /// ִ�ж�������������
    /// </summary>
    /// <param name="lst">SQL��伯</param>
    /// <returns>����ɹ��и�����true,���򷵻�false</returns>
    public bool SqlExecuteTrans(List<String> lst)
    {
        using (SqlConnection conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            //��������
            SqlTransaction trans = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;//������ӹ���
            cmd.Transaction = trans;//�������
            try
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    string sql = lst[i].ToString();//��ȡsql���
                    cmd.CommandText = sql;//���sql���
                    cmd.ExecuteNonQuery();//ִ��
                }
                trans.Commit();//ִ�����֮���ύ
                return true;
            }
            catch (Exception ex)
            {
                //ִ��sql���ʧ�ܣ�����ع�
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
