<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

    <%--查询数据--%>
    <%
        string sqlSelect = "select * from Student";
        DataTable dt = new DataTable();
        dt = SQLHelper.Select(sqlSelect);
    %>

  

    <div>
        
        <h4>查询数据</h4>
        <hr>
        <table>
            <%--绑定数据--%>
            <%for (int i = 0; i < dt.Rows.Count; i++) { %>
            <tr>
                <td><%Response.Write(dt.Rows[i]["name"]); %></td>
                <td><%Response.Write(dt.Rows[i]["sex"].ToString() == "True" ? "男" : "女"); %></td>
                <td><%Response.Write(dt.Rows[i]["major"]); %></td>
            </tr>
            <%} %>
        </table>
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />

        <h4>插入数据</h4>
        <hr>
        姓名：<input type="text" name="insert_name" value=""  />
        性别：<select name="insert_sex" class="select">
					<option value="1">男</option>
					<option value="0">女</option>
			 </select>
        专业：<input type="text" name="insert_major" value="" />
        <asp:Button ID="btnInsert" runat="server" Text="插入" OnClick="btnInsert_Click"/>
        <h4>删除数据</h4>
        <hr>
        姓名：<input type="text" name="delete_name" value=""  />
         <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click"/>
        <h4>更新数据</h4>
        <hr>
        姓名：<input type="text" name="update_name" value=""  />

        性别：<select name="update_sex" class="select">
					<option value="1">男</option>
					<option value="0">女</option>
			 </select>
        专业：<input type="text" name="update_major" value="" />
        <asp:Button ID="btnUpdate" runat="server" Text="更新" OnClick="btnUpdate_Click"/>
    
    </div>
    </form>
</body>
</html>
