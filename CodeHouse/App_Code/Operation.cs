using System;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using Bmob_space;
using cn.bmob.api;
using cn.bmob.io;
/// <summary>
/// Operation 的摘要说明
/// </summary>
namespace Operate
{
 
    //操作类
    public class Operation: BmobDatabaseHand
    { 
        
        /// <summary>
        /// Json序列化所有分类
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string ToKindJson(DataTable table,DataTable tablecode)
        {
            List<Kind_Model> list = new List<Kind_Model>();
            foreach(DataRow row in table.Rows)
            {
                string Color= "blue";
                string Icon = "glyphicon glyphicon-folder-open";
                if (row["ParentId"].ToString() == "无")
                {
                    Color = "brown";Icon = "glyphicon glyphicon-folder-open";
                }
                else
                {
                    foreach (DataRow row_ in tablecode.Rows)
                    {
                        if (row_["Title"].ToString() == row["Name"].ToString()) { Color = "black";Icon = "glyphicon glyphicon-pencil"; break; }
                    }
                    foreach(DataRow row_ in table.Rows)
                    {
                        if (row_["ParentId"].ToString() == row["ID"].ToString()) { Color = "blue"; Icon = "glyphicon glyphicon-folder-open"; break; }
                    }
                }

                list.Add(new Kind_Model()
                {
                    Id = row["ID"].ToString(), ParentId = row["ParentId"].ToString(), text = row["Name"].ToString(),
                    color = Color, icon = Icon
                });
            }
            string JsonData = JsonConvert.SerializeObject(list);
            return JsonData;
        }
        /// <summary>
        /// 检测账户
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public string TestUser(string User,string Pwd)
        {
            if (User.Equals("shajun") && Pwd.Equals("sj76606729"))
            {
                string UserName = "沙俊";
                return UserName;
            }else if(User.Equals("shajie") && Pwd.Equals("a295574220"))
            {
                string UserName = "沙杰";
                return UserName;
            }
            else if (User.Equals("chenyu") && Pwd.Equals("CHENYU"))
            {
                string UserName = "陈煜";
                return UserName;
            }
            return null;
        }
    }
    /*//数据库处理类
    public class DatabaseHand
    {
        public SqlConnection conn;
        public DatabaseHand()
        {
            conn = new SqlConnection("Server=LAPTOP-03G3SDKH\\SQLEXPRESS;DataBase=CodeHouse; uid=sa;pwd=sj76606729");
            if (System.Environment.MachineName== "SC-201706152014")
            {
                conn = new SqlConnection("Server=SC-201706152014;DataBase=CodeHouse; uid=sa;pwd=sj76606729");
            }
        }
        public SqlCommand comm;
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Kind"></param>
        /// <param name="Code"></param>
        /// <param name="Author"></param>
        /// <returns></returns>
        public bool TestExit(string Title,int Kind,string Code,string Author)
        {
            conn.Open();
            comm = new SqlCommand("select Title from Code_tb where Title='" + Title + "'", conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            if (read.HasRows) { CloseDataBase(conn,comm,read); return false; }
            read.Close();
            comm = new SqlCommand("insert into Kind_tb(ParentId,Name) values(" + Kind + ",'" + Title + "')", conn);
            comm.ExecuteNonQuery();
            string time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            comm = new SqlCommand("insert into Code_tb(Title,Code,Author,Time) values('" + Title + "','" + Code + "','" + Author + "','" + time + "')", conn);
            comm.ExecuteNonQuery();
            CloseDataBase(conn,comm);
            return true;

        }
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool AddKind(string ParentId,string Name)
        {
            conn.Open();
            comm = new SqlCommand("select * from Kind_tb where ParentId=" + int.Parse(ParentId) + " and Name='" + Name + "'", conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            if (!read.HasRows)
            {
                read.Close();
                comm = new SqlCommand("insert into Kind_tb(ParentId,Name) values(" + int.Parse(ParentId) + ",'" + Name + "')", conn);
                comm.ExecuteNonQuery();
                CloseDataBase(conn, comm);
                return true;
            }
            else { CloseDataBase(conn, comm, read);return false; }
        }
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllKind()
        {
            conn.Open();
            comm = new SqlCommand("select * from Kind_tb", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            CloseDataBase(conn, comm,adapt);
            return set.Tables[0];
        }
        /// <summary>
        /// 获取所有代码数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCode()
        {
            conn.Open();
            comm = new SqlCommand("select * from Code_tb", conn);
            SqlDataAdapter adapt = new SqlDataAdapter(comm);
            DataSet set = new DataSet();
            adapt.Fill(set);
            CloseDataBase(conn, comm, adapt);
            return set.Tables[0];
        }
        /// <summary>
        /// 获得代码数据
        /// </summary>
        /// <param name="Name"></param>
        public string GetCode(string Name)
        {
            conn.Open();
            List<CodeModel> list = new List<CodeModel>();
            comm = new SqlCommand("select * from Code_tb where Title='" + Name + "'", conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            if (read.HasRows)
            {
                list.Add(new CodeModel()
                {
                    Title = read["Title"].ToString(),
                    Code = read["Code"].ToString(),
                    Author = read["Author"].ToString(),
                    Time = Convert.ToDateTime(read["Time"]).ToString("yyyy-MM-dd hh:mm:ss")
                });
                string JsonData = JsonConvert.SerializeObject(list);
                return JsonData;
            }
            else { return null; }
            
        }
        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool ModifyKind(string Name,string Id)
        {
            conn.Open();
            comm = new SqlCommand("update Kind_tb set Name='" + Name + "' where ID=" + Id, conn);
            comm.ExecuteNonQuery();
            CloseDataBase(conn, comm);
            return true;
        }
        /// <summary>
        /// 修改代码数据
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Code"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool ModifyCode(string Title,string Code,string Id)
        {
            conn.Open();
            comm = new SqlCommand("select * from Kind_tb where ID=" + Id, conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            string OldTitle = read["Name"].ToString();
            read.Close();
            comm = new SqlCommand("update Code_tb set Title='" + Title + "',Code='" + Code + "' where Title='" + OldTitle + "'", conn);
            comm.ExecuteNonQuery();
            comm = new SqlCommand("update Kind_tb set Name='" + Title + "' where ID=" + Id, conn);
            comm.ExecuteNonQuery();
            CloseDataBase(conn, comm);
            return true;
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteKind(string Id)
        {
            conn.Open();
            comm = new SqlCommand("select * from Kind_tb where ParentId=" + Id, conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            if (read.HasRows)
            {
                CloseDataBase(conn, comm, read);return false;
            }
            else
            {
                read.Close();
                comm = new SqlCommand("delete from Kind_tb where ID=" + Id, conn);
                comm.ExecuteNonQuery();
                CloseDataBase(conn, comm);
                return true;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteCode(string Id,string Title)
        {
            conn.Open();
            comm = new SqlCommand("select * from Code_tb where Title='" + Title + "'", conn);
            SqlDataReader read = comm.ExecuteReader();
            read.Read();
            if (read.HasRows)
            {
                read.Close();
                comm = new SqlCommand("delete from Code_tb where Title='" + Title + "'", conn);
                comm.ExecuteNonQuery();
            }
            read.Close();
            comm = new SqlCommand("delete from Kind_tb where ID=" + Id, conn);
            comm.ExecuteNonQuery();
            CloseDataBase(conn, comm);
            return true;
        }
        /// <summary>
        /// 关闭数据库相关链接重载
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="comm"></param>
        public void CloseDataBase(SqlConnection conn,SqlCommand comm)
        {
            conn.Close();comm.Dispose();
        }
        public void CloseDataBase(SqlConnection conn, SqlCommand comm, SqlDataReader read)
        {
            conn.Close(); comm.Dispose();read.Close();
        }
        public void CloseDataBase(SqlConnection conn, SqlCommand comm, SqlDataAdapter adapter)
        {
            conn.Close(); comm.Dispose(); adapter.Dispose();
        }
    }*/
    //比目数据库处理类
    public class BmobDatabaseHand
    {
        BmobWindows Bmob= new BmobWindows();
        Bmob_Initial initial = Bmob_Initial.Initial();
        /// <summary>
        /// 添加代码
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Kind"></param>
        /// <param name="Code"></param>
        /// <param name="Author"></param>
        /// <returns></returns>
        public bool AddData(string Title, string Kind, string Code, string Author)
        {
            try
            {
                CodeModel codeModel = new CodeModel("Code_tb");
                codeModel.Title = Title;
                codeModel.Code = Code;
                codeModel.Author = Author;
                var future = Bmob.CreateTaskAsync(codeModel);
                if (future.Result is IBmobWritable)
                {
                    KindModel kindModel = new KindModel("Kind_tb");
                    kindModel.ParentId = Kind;
                    kindModel.Name = Title;
                    var future1 = Bmob.CreateTaskAsync(kindModel);
                    if (future1.Result is IBmobWritable) { return true;
                    } else return false;
                }
                else return false;
            }catch(Exception e)
            {
                return false;
            }
            

            

        }
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public bool AddKind(string ParentId, string Name)
        {
            var query = new BmobQuery();
            query.WhereContainedIn<string>("ParentId", ParentId);
            query.WhereContainedIn<string>("Name", Name);
            var future = Bmob.FindTaskAsync<KindModel>("Kind_tb", query);
            if (future.Result.results.Count == 0)
            {
                KindModel kindModel = new KindModel("Kind_tb");
                kindModel.ParentId = ParentId;
                kindModel.Name = Name;
                var future1 = Bmob.CreateTaskAsync(kindModel);
                if(future1.Result.objectId.Length>0) { return true; }
            }
            return false;
        }
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllKind()
        {
            var query = new BmobQuery();
            query.Limit(500);
            var future = Bmob.FindTaskAsync<KindModel>("Kind_tb", query);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("ID",typeof(string)));
            table.Columns.Add(new DataColumn("ParentId", typeof(string)));
            table.Columns.Add(new DataColumn("Name", typeof(string)));
            if(future.Result is IBmobWritable)
            {
                int i = 0;
                foreach(object data in future.Result.results)
                {
                    DataRow row = table.NewRow();
                    row["ID"] = future.Result.results[i].objectId;
                    row["ParentId"] = future.Result.results[i].ParentId;
                    row["Name"] = future.Result.results[i].Name;
                    table.Rows.Add(row);i++;
                }
            }
            return table; ;
        }
        /// <summary>
        /// 获取所有代码数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCode()
        {
            var query = new BmobQuery();
            query.Limit(500);
            var future = Bmob.FindTaskAsync<CodeModel>("Code_tb", query);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Title", typeof(string)));
            table.Columns.Add(new DataColumn("Code", typeof(string)));
            table.Columns.Add(new DataColumn("Author", typeof(string)));
            if(future.Result is IBmobWritable)
            {
                int i = 0;
                foreach(object data in future.Result.results)
                {
                    DataRow row = table.NewRow();
                    row["Title"] = future.Result.results[i].Title;
                    row["Code"] = future.Result.results[i].Code;
                    row["Author"] = future.Result.results[i].Author;
                    table.Rows.Add(row);i++;
                }
            }
            return table;
        }
        /// <summary>
        /// 获得代码数据
        /// </summary>
        /// <param name="Name"></param>
        public string GetCode(string Name)
        {
            BmobQuery query = new BmobQuery();
            query.WhereEqualTo("Title", Name);
            var future = Bmob.FindTaskAsync<CodeModel>("Code_tb", query);
            List<Code_Model> list = new List<Code_Model>();
            if (future.Result is IBmobWritable)
            {
                list.Add(new Code_Model()
                {
                    Title = future.Result.results[0].Title,
                    Code = future.Result.results[0].Code,
                    Author = future.Result.results[0].Author
                });
                string JsonData = JsonConvert.SerializeObject(list);
                return JsonData;
            }
            else { return null; }

        }
        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool ModifyKind(string Name, string Id)
        {
            var future1 = Bmob.GetTaskAsync<KindModel>("Kind_tb", Id);
            string ParentId="";
            if(future1.Result is IBmobWritable)
            {
                ParentId = future1.Result.ParentId;
            }
            var query = new BmobQuery();
            query.WhereContainedIn<string>("ParentId", ParentId);
            query.WhereContainedIn<string>("Name", Name);
            var future2 = Bmob.FindTaskAsync<KindModel>("Kind_tb", query);
            if(future2.Result is IBmobWritable)
            {
                if (future2.Result.results.Count > 0) { return false; }
            }
            KindModel kindModel = new KindModel("Kind_tb");
            kindModel.objectId = Id;
            kindModel.Name = Name;
            var future = Bmob.UpdateTaskAsync<KindModel>(kindModel);
            if (future.Result is IBmobWritable)
            {
                return true;
            }
            else return false;
            
        }
        /// <summary>
        /// 修改代码数据
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Code"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool ModifyCode(string Title, string Code, string Id,string OldTitle,string User)
        {
            var query = new BmobQuery();
            query.WhereContainedIn<string>("Title", OldTitle);
            var future = Bmob.FindTaskAsync<CodeModel>("Code_tb", query);
            string Objectid="";
            if(future.Result is IBmobWritable)
            {
                Objectid = future.Result.results[0].objectId;
                if (future.Result.results[0].Author != User) { return false; }
            }
            CodeModel codeModel = new CodeModel("Code_tb");
            codeModel.objectId = Objectid;
            codeModel.Title = Title;
            codeModel.Code = Code;
            var future1 = Bmob.UpdateTaskAsync<CodeModel>(codeModel);
            if (future1.Result is IBmobWritable)
            {
                KindModel kindModel = new KindModel("Kind_tb");
                kindModel.objectId = Id;
                kindModel.Name = Title;
                future1 = Bmob.UpdateTaskAsync<KindModel>(kindModel);
                if (future1.Result is IBmobWritable) return true; else return false;
            }
            else return false;
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteKind(string Id)
        {
            var query = new BmobQuery();
            query.WhereContainedIn<string>("ParentId", Id);
            var future = Bmob.FindTaskAsync<CodeModel>("Kind_tb", query);
            if(future.Result is IBmobWritable)
            {
                if (future.Result.results.Count > 0)
                {
                    return false;
                }
            }
            var future1 = Bmob.DeleteTaskAsync("Kind_tb", Id);
            if (future1.Result is IBmobWritable) return true; else return false;

        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool DeleteCode(string Id, string Title)
        {
            var query = new BmobQuery();
            query.WhereContainedIn<string>("Title", Title);
            var future = Bmob.FindTaskAsync<CodeModel>("Code_tb", query);
            string Objectid = "";
            if (future.Result is IBmobWritable)
            {
                Objectid = future.Result.results[0].objectId;
            }
            var future1 = Bmob.DeleteTaskAsync("Code_tb", Objectid);
            if (future1.Result is IBmobWritable)
            {
                future1 = Bmob.DeleteTaskAsync("Kind_tb", Id);
                if (future1.Result is IBmobWritable) return true; else return false;
            }
            else return false;
        }
    }
    public class Kind_Model
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string icon { get; set; }
    }
    public class Code_Model
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string Author { get; set; }
    }
}
