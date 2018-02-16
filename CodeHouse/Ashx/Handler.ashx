<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Web.SessionState;
using Operate;
using Search;
using System.Collections.Generic;
public class Handler : IHttpHandler,IRequiresSessionState{

    public void ProcessRequest (HttpContext context) {
        string id = HttpContext.Current.Request.Form["id"].ToString();
        Operation operation = new Operation();
        SearchOperate searchOperate = new SearchOperate();
        string result_str;
        bool result_bol;
        switch (id)
        {
            case "AddData":
                result_str = operation.AddData(HttpContext.Current.Request.Form["Title"],HttpContext.Current.Request.Form["Key"].ToString(), HttpContext.Current.Request.Form["Code"],HttpContext.Current.Request.Form["User"]);
                context.Response.Write("{\"result\":\"" + result_str + "\"}");
                break;
            case "AddKind":
                result_str = operation.AddKind(HttpContext.Current.Request.Form["ParentId"], HttpContext.Current.Request.Form["Name"]);
                context.Response.Write("{\"result\":\"" + result_str + "\"}");
                break;
            case "Initial":
                result_str = operation.ToKindJson(operation.GetAllKind(),operation.GetAllCode());
                context.Response.Write(result_str);
                break;
            case "ShowData":
                result_str = operation.GetCode(HttpContext.Current.Request.Form["Name"]);
                if (result_str == null) { break; }
                context.Response.Write(result_str);
                break;
            case "ModifyKind":
                result_bol = operation.ModifyKind(HttpContext.Current.Request.Form["Name"], HttpContext.Current.Request.Form["Key"]);
                if (result_bol) { context.Response.Write("{\"result\":\"" + result_bol + "\"}"); }
                break;
            case "ModifyCode":
                result_str = operation.ModifyCode(HttpContext.Current.Request.Form["Title"], HttpContext.Current.Request.Form["Code"], HttpContext.Current.Request.Form["Key"],HttpContext.Current.Request.Form["OldTitle"]);
                context.Response.Write("{\"result\":\"" + result_str + "\"}");
                break;
            case "login":
                result_str = operation.TestUser(HttpContext.Current.Request.Form["username"], HttpContext.Current.Request.Form["password"]);
                if (result_str != null) { HttpContext.Current.Session["username"] = result_str; context.Response.Write("{\"result\":\""+result_str+"\"}");}
                break;
            case "FirstUser":
                if (HttpContext.Current.Session["username"] != null)
                {
                    context.Response.Write("{\"result\":\""+HttpContext.Current.Session["username"]+"\"}");
                }
                break;
            case "DeleteKind":
                result_str = operation.DeleteKind(HttpContext.Current.Request.Form["Key"]);
                context.Response.Write("{\"result\":\"" + result_str + "\"}");
                break;
            case "DeleteCode":
                result_str = operation.DeleteCode(HttpContext.Current.Request.Form["Key"],HttpContext.Current.Request.Form["Title"]);
                context.Response.Write("{\"result\":\"" + result_str + "\"}");
                break;
            case "Search":
                result_str = searchOperate.SearchTitle(HttpContext.Current.Request.Form["search"]);
                if (result_str.Substring(0, 1) != "[") { context.Response.Write("{\"result\":\"" + result_str + "\"}"); }else { context.Response.Write(result_str);}
                break;
            case "MoveKind":
                result_str = operation.MoveKind(HttpContext.Current.Request.Form["node"], HttpContext.Current.Request.Form["newnode"]);
                context.Response.Write("{\"result\":\"" + result_str + "\"}");break;
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}