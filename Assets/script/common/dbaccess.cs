/*using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;
public class dbaccess : MonoBehaviour {

	// Use this for initialization
	 private SqliteConnection dbConnection;
 
    private SqliteCommand dbCommand;
 
    private SqliteDataReader reader;
 
    public dbaccess (string connectionString)
 
    {
 
        OpenDB (connectionString);
      
    }
   
 
    public void OpenDB (string connectionString)
 
    {
		try
   		 {
        	dbConnection = new SqliteConnection (connectionString);
 
       	 	dbConnection.Open ();
 
       		Debug.Log ("已连接到数据库");
		 }
    	catch(Exception e)
    	{
       		string temp1 = e.ToString();
       		Debug.Log(temp1);
    	}
 
    }
 
    public void CloseSqlConnection ()
 
    {
 
        if (dbCommand != null) {

            dbCommand.Dispose();
 
        }
 
        dbCommand = null;
 
        if (reader != null) {

            reader.Dispose();
 
        }
 
        reader = null;
 
        if (dbConnection != null) {
 
            dbConnection.Close ();
 
        }
 
        dbConnection = null;
 
        Debug.Log ("数据库连接已断开");
 
    }
 
    public SqliteDataReader ExecuteQuery (string sqlQuery)
 
    {
 
        dbCommand = dbConnection.CreateCommand ();
 
        dbCommand.CommandText = sqlQuery;
 
        reader = dbCommand.ExecuteReader ();
 
        return reader;
 
    }
 
    public SqliteDataReader ReadFullTable (string tableName)
 
    {
 
        string query = "SELECT * FROM " + tableName;
 
        return ExecuteQuery (query);
 
    }
    public ArrayList to0bj(byte[] blob,char[] shouwei){
     string  str = System.Text.Encoding.Default.GetString(blob).Trim(shouwei);
     Debug.Log("toobj中处理的str为"+str);
     Debug.Log("开始获取字符串数组s");
    string[] s=str.Split(new char[]{','});
  
    Debug.Log("进入for循环");
    Debug.Log("字符串数组s的长度为" + s.Length);
    for (int t = 0; t < s.Length;t++ ) {

        Debug.Log("字符串数组s的第" + t + "个元素为" + s[t] + "字符串数组的长度是" + s.Length);
    
    }
    Debug.Log("退出for循环");
    ArrayList f = new ArrayList();
    Debug.Log("已获取arraylist对象");
    foreach(string s1 in s){
        
        if (s1 != null)
        {f.Add(Convert.ToSingle( s1)); }
   // f.Add(Convert.ToSingle(s1));
    
}
    Debug.Log("toobj完毕");
    return f;
}
    public bool existtable(string tableName) {
        string query = "select count(*) from sqlite_master where type='table' and name="+"'"+tableName+"'"+";";
        dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query;
        if (0 == Convert.ToInt32(dbCommand.ExecuteScalar()))
        {
            Debug.Log("目标数据库不存在表即将创建表'"+tableName+"'");
            return true;
            //table - Student does not exist.  
        }
        else {
            Debug.Log("目标数据库存在,返回值为"+Convert.ToInt32(dbCommand.ExecuteScalar()));
            return false;
        }
       // dbCommand.Dispose();
        //Debug.Log(query);
        
    }
    public SqliteDataReader InsertInto (string tableName, string[] values)
 
    {
 
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
 
        for (int i = 1; i < values.Length; ++i) {
 
            query += ", " + values[i];
 
        }
 
        query += ")";
 
        return ExecuteQuery (query);
 
    }
 
	public SqliteDataReader UpdateInto (string tableName, string []cols,string []colsvalues,string selectkey,string selectvalue)
	{
 
		string query = "UPDATE "+tableName+" SET "+cols[0]+" = "+colsvalues[0];
 
		for (int i = 1; i < colsvalues.Length; ++i) {
 
		 	 query += ", " +cols[i]+" ="+ colsvalues[i];
		}
 
		 query += " WHERE "+selectkey+" = "+"'"+selectvalue+"'"+" ";
         Debug.Log(query);
		return ExecuteQuery (query);
	}
 
	public SqliteDataReader Delete(string tableName,string []cols,string []colsvalues)
	{
			string query = "DELETE FROM "+tableName + " WHERE " +cols[0] +" = " + colsvalues[0];
 
			for (int i = 1; i < colsvalues.Length; ++i) {
 
		 	    query += " or " +cols[i]+" = "+ colsvalues[i];
			}
		Debug.Log(query);
		return ExecuteQuery (query);
	}
 
    public SqliteDataReader InsertIntoSpecific (string tableName, string[] cols, string[] values)
 
    {
 
        if (cols.Length != values.Length) {
 
            throw new SqliteException ("columns.Length != values.Length");
 
        }
 
        string query = "INSERT INTO " + tableName + "(" + cols[0];
 
        for (int i = 1; i < cols.Length; ++i) {
 
            query += ", " + cols[i];
 
        }
 
        query += ") VALUES (" + values[0];
 
        for (int i = 1; i < values.Length; ++i) {
 
            query += ", " + values[i];
 
        }
 
        query += ")";
 
        return ExecuteQuery (query);
 
    }
 
    public SqliteDataReader DeleteContents (string tableName)
 
    {
 
        string query = "DELETE FROM " + tableName;
 
        return ExecuteQuery (query);
 
    }
 
    public SqliteDataReader CreateTable ( string name, string[] col, string[] colType)
 
    {
        if (existtable(name))
        {
            if (col.Length != colType.Length)
            {

                throw new SqliteException("columns.Length != colType.Length");

            }

            string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];

            for (int i = 1; i < col.Length; ++i)
            {

                query += ", " + col[i] + " " + colType[i];

            }

            query += ")";

            return ExecuteQuery(query);
        }
        else {
            return ExecuteQuery("");
            Debug.Log("表"+name+"已存在");
        }
 
    }
    public SqliteDataReader Select(string tableName, string LastName,string value) { 
    string item="select "+LastName+"="+"'"+value+"'"+" from "+tableName;
    return ExecuteQuery(item);
    }
    public SqliteDataReader SelectWhere (string tableName, string[] items, string[] col, string[] operation, string[] values)
 
    {
 
        if (col.Length != operation.Length || operation.Length != values.Length) {
 
            throw new SqliteException ("col.Length != operation.Length != values.Length");
 
        }
 
        string query = "SELECT " + items[0];
 
        for (int i = 1; i < items.Length; ++i) {
 
            query += ", " + items[i];
 
        }
 
        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
 
        for (int i = 1; i < col.Length; ++i) {
 
            query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
 
        }
 
        return ExecuteQuery (query);
 
    }
}
/*
// AttributesTutorial.cs
// This example shows the use of class and method attributes.

using System;
using System.Reflection;
using System.Collections;

// The IsTested class is a user-defined custom attribute class.
// It can be applied to any declaration including
//  - types (struct, class, enum, delegate)
//  - members (methods, fields, events, properties, indexers)
// It is used with no arguments.
public class IsTested : Attribute
{
    public override string ToString()
    {
        return "Is Tested";
    }
}

// The AuthorAttribute class is a user-defined attribute class.
// It can be applied to classes and struct declarations only.
// It takes one unnamed string argument (the author's name).
// It has one optional named argument Version, which is of type int.
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class AuthorAttribute : Attribute
{
    // This constructor specifies the unnamed arguments to the attribute class.
    public AuthorAttribute(string name)
    {
        this.name = name;
        this.version = 0;
    }

    // This property is readonly (it has no set accessor)
    // so it cannot be used as a named argument to this attribute.
    public string Name
    {
        get
        {
            return name;
        }
    }

    // This property is read-write (it has a set accessor)
    // so it can be used as a named argument when using this
    // class as an attribute class.
    public int Version
    {
        get
        {
            return version;
        }
        set
        {
            version = value;
        }
    }

    public override string ToString()
    {
        string value = "Author : " + Name;
        if (version != 0)
        {
            value += " Version : " + Version.ToString();
        }
        return value;
    }

    private string name;
    private int version;
}

// Here you attach the AuthorAttribute user-defined custom attribute to 
// the Account class. The unnamed string argument is passed to the 
// AuthorAttribute class's constructor when creating the attributes.
[Author("Joe Programmer")]
class Account
{
    // Attach the IsTestedAttribute custom attribute to this method.
    [IsTested]
    public void AddOrder(Order orderToAdd)
    {
        orders.Add(orderToAdd);
    }

    private ArrayList orders = new ArrayList();
}

// Attach the AuthorAttribute and IsTestedAttribute custom attributes 
// to this class.
// Note the use of the 'Version' named argument to the AuthorAttribute.
[Author("Jane Programmer", Version = 2), IsTested()]
class Order
{
    // add stuff here ...
}

class MainClass
{
    private static bool IsMemberTested(MemberInfo member)
    {
        foreach (object attribute in member.GetCustomAttributes(true))
        {
            if (attribute is IsTested)
            {
                return true;
            }
        }
        return false;
    }

    private static void DumpAttributes(MemberInfo member)
    {
        Console.WriteLine("Attributes for : " + member.Name);
        foreach (object attribute in member.GetCustomAttributes(true))
        {
            Console.WriteLine(attribute);
        }
    }

    public static void Main()
    {
        // display attributes for Account class
        DumpAttributes(typeof(Account));

        // display list of tested members
        foreach (MethodInfo method in (typeof(Account)).GetMethods())
        {
            if (IsMemberTested(method))
            {
                Console.WriteLine("Member {0} is tested!", method.Name);
            }
            else
            {
                Console.WriteLine("Member {0} is NOT tested!", method.Name);
            }
        }
        Console.WriteLine();

        // display attributes for Order class
        DumpAttributes(typeof(Order));

        // display attributes for methods on the Order class
        foreach (MethodInfo method in (typeof(Order)).GetMethods())
        {
            if (IsMemberTested(method))
            {
                Console.WriteLine("Member {0} is tested!", method.Name);
            }
            else
            {
                Console.WriteLine("Member {0} is NOT tested!", method.Name);
            }
        }
        Console.WriteLine();
    }
}
/*Attributes for : Account
Author : Joe Programmer
Member GetHashCode is NOT tested!
Member Equals is NOT tested!
Member ToString is NOT tested!
Member AddOrder is tested!
Member GetType is NOT tested!

Attributes for : Order
Author : Jane Programmer Version : 2
Is Tested
Member GetHashCode is NOT tested!
Member Equals is NOT tested!
Member ToString is NOT tested!
Member GetType is NOT tested*/