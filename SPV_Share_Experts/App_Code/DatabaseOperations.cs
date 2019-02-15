using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Security.Cryptography;

/// <summary>
/// Summary description for DatabaseConnection
/// </summary>
public class DatabaseOperations
{
    string connectionString;
    SqlConnection con;
    SqlCommand cmd;
    public DatabaseOperations(string mappath)
    {
        
        connectionString=@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename ="+mappath+"Share_Experts.mdf; Integrated Security = True; Connect Timeout = 30;";
        con = new SqlConnection
        {
            //connectionString= ConfigurationManager.ConnectionStrings["ShareExpertsDB"].ConnectionString;

            ConnectionString = connectionString
        };


    }

    public bool VerifyAdmin(string username,string password)
    {
        try
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandText = "select * from admin where uname='" +username+ "';";
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            string pass = dr.GetString(1);
            con.Close();
            if (pass.Equals(password))
            {
                return true;
            }
            return false;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }

    public List<string> GetCodes(string id)
    {
        con.Open();
        cmd = con.CreateCommand();
        cmd.CommandText = "select code from savedcodes where u_id= '"+id+"';";
        SqlDataReader dr= cmd.ExecuteReader();
        List<string> codes = new List<string>();
        while(dr.Read())
        {
            codes.Add(dr.GetString(0));
        }
        return codes;
    }


    public int InsertUser(ref User u)
    {
        try
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "InsertUser";
            cmd.Parameters.AddWithValue("@email",u.Email);
            cmd.Parameters.AddWithValue("@user_pass", u.Pass);
            cmd.Parameters.AddWithValue("@name", InitCap(ref u));

             int i=cmd.ExecuteNonQuery();
           
            con.Close();

            return i;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return 0;
    }
    public int InsertCode(string code,string u_id)
    {
        try
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "savecode";

            cmd.Parameters.AddWithValue("@code", code);
            cmd.Parameters.AddWithValue("@u_id",u_id);

            int i = cmd.ExecuteNonQuery();

            con.Close();

            return i;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        return 0;
    }
    public bool VerifyUser(ref User u)
    {
        try
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandText="select * from users where email_id='"+u.Email+"';";
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            
            string passhash = dr.GetString(2);
            u.Id = dr.GetInt32(0);
            u.Name = dr.GetString(3);
            con.Close();
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(passhash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(u.Pass, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }
            return true;
            
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }

    private string InitCap(ref User u)
    {
        u.Name = u.Name.Substring(0,1).ToUpper()+u.Name.Substring(1).ToLower();
        return u.Name;
    }

    public int DeleteCode(string code, string u_id)
    {
        try
        {
            con.Open();
            cmd = con.CreateCommand();
            cmd.CommandText = "deletecode";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@code",code);
            cmd.Parameters.AddWithValue("@u_id",u_id);

            int i = cmd.ExecuteNonQuery();

            con.Close();

            return i;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
        return 0;
    }
}