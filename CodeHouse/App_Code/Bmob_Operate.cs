using cn.bmob.api;
using cn.bmob.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Bmob_Operate 的摘要说明
/// </summary>
namespace Bmob_space
{
    //bmob初始化单利
    public class Bmob_Initial
    {
        private static Bmob_Initial initial = null;
        private BmobWindows bmob;
        public static Bmob_Initial Initial()
        {
            if (initial == null)
            {
                initial = new Bmob_Initial();
            }
            return initial;
        }
        private Bmob_Initial() : base()
        {
            
            bmob = new BmobWindows();
            //初始化，这个ApplicationId/RestKey需要更改为你自己的ApplicationId/RestKey（ http://www.bmob.cn 上注册登录之后，创建应用可获取到ApplicationId/RestKey）
            Bmob.initialize("bcab92606ca5634cc0a65811ee7940f7", "f23363d3111faa6d14ba23d510a521f7");
        }
        private BmobWindows Bmob
        {
            get { return bmob; }
        }
    }
    public class Bmob_Operate
    {

        public Bmob_Operate()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        
    }
    //代码数据模型
    public class CodeModel : BmobTable
    {
        private String fTable;
        public string Title { get; set; }
        public string Code { get; set; }
        public string Author { get; set; }


        //构造函数
        public CodeModel() { }

        //构造函数
        public CodeModel(String tableName)
        {
            this.fTable = tableName;
        }

        public override string table
        {
            get
            {
                if (fTable != null)
                {
                    return fTable;
                }
                return base.table;
            }
        }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.Title = input.getString("Title");
            this.Code = input.getString("Code");
            this.Author= input.getString("Author");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("Title", this.Title);
            output.Put("Code", this.Code);
            output.Put("Author", this.Author);
        }
    }
    //分类数据模型
    public class KindModel : BmobTable
    {
        private String fTable;
        public string ParentId { get; set; }
        public string Name { get; set; }


        //构造函数
        public KindModel() { }

        //构造函数
        public KindModel(String tableName)
        {
            this.fTable = tableName;
        }

        public override string table
        {
            get
            {
                if (fTable != null)
                {
                    return fTable;
                }
                return base.table;
            }
        }

        //读字段信息
        public override void readFields(BmobInput input)
        {
            base.readFields(input);

            this.ParentId= input.getString("ParentId");
            this.Name = input.getString("Name");
        }

        //写字段信息
        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);

            output.Put("ParentId", this.ParentId);
            output.Put("Name", this.Name);
        }
    }
}
