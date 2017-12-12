using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        //获取值
        string name = Request.Form["insert_name"];
        string sex = Request.Form["insert_sex"];
        string major = Request.Form["insert_major"];
        //拼接SQL语句
        string sql = "insert into Student values('" + name + "','" + sex + "','" + major + "')";
        //执行SQL语句
        int res = SQLHelper.SqlExecute(sql);
        if (res > 0)
        {
            Response.Write("<script>alert('插入成功');location='Default.aspx';</script>");
        }
        else
        {
            Response.Write("<script>alert('插入失败')</script>");
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //获取值
        string name = Request.Form["update_name"];
        string sex = Request.Form["update_sex"];
        string major = Request.Form["update_major"];
        //拼接SQL语句
        string sql = "update Student set sex='" + sex + "',major='" + major + "'where name='" + name + "'";
        //执行SQL语句
        int res = SQLHelper.SqlExecute(sql);
        if (res > 0)
        {
            Response.Write("<script>alert('更新成功');location='Default.aspx';</script>");
        }
        else
        {
            Response.Write("<script>alert('更新失败')</script>");
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //获取值
        string name = Request.Form["delete_name"];
        //拼接SQL语句
        string sql = "delete from Student where name='" + name + "'";
        //执行SQL语句
        int res = SQLHelper.SqlExecute(sql);
        if (res > 0)
        {
            Response.Write("<script>alert('删除成功');location='Default.aspx';</script>");
        }
        else
        {
            Response.Write("<script>alert('删除失败')</script>");
        }
    }
}