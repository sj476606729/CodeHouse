<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Web.SessionState;
using Operate;
using System.Data;
using System.Collections.Generic;
public class Handler : IHttpHandler,IRequiresSessionState{

    public void ProcessRequest (HttpContext context) {
        string id = HttpContext.Current.Request.Form["id"].ToString();
        Operation operation = new Operation();
        string result_str;
        bool result_bol;
        switch (id)
        {
            case "AddData":
                result_bol = operation.AddData(HttpContext.Current.Request.Form["Title"],HttpContext.Current.Request.Form["Key"].ToString(), HttpContext.Current.Request.Form["Code"],HttpContext.Current.Request.Form["User"]);
                if (result_bol) { context.Response.Write("{\"result\":\"" + result_bol + "\"}"); }
                break;
            case "AddKind":
                result_bol = operation.AddKind(HttpContext.Current.Request.Form["ParentId"], HttpContext.Current.Request.Form["Name"]);
                context.Response.Write("{\"result\":\"" + result_bol + "\"}");
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
                result_bol = operation.ModifyCode(HttpContext.Current.Request.Form["Title"], HttpContext.Current.Request.Form["Code"], HttpContext.Current.Request.Form["Key"],HttpContext.Current.Request.Form["OldTitle"],HttpContext.Current.Request.Form["User"]);
                if (result_bol) { context.Response.Write("{\"result\":\"" + result_bol + "\"}"); }
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
                result_bol = operation.DeleteKind(HttpContext.Current.Request.Form["Key"]);
                if (result_bol) { context.Response.Write("{\"result\":\"" + result_bol + "\"}"); }
                break;
            case "DeleteCode":
                result_bol = operation.DeleteCode(HttpContext.Current.Request.Form["Key"],HttpContext.Current.Request.Form["Title"]);
                if (result_bol) { context.Response.Write("{\"result\":\"" + result_bol + "\"}"); }
                break;
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}