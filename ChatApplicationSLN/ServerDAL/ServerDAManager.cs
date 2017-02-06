using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ServerDAL
{
    public class ServerDAManager
    {        
        SqlConnection cn = new SqlConnection(
            ConfigurationManager.ConnectionStrings["mycnstr"].ConnectionString);

        #region DBVoidFunctions
        public void UpdateDisconnected(string userName1)
        {            
            SqlCommand com = new SqlCommand("spUpdateDisconnected", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserName = new SqlParameter()
            {
                ParameterName = "@UserName",
                Value = userName1
            };
            com.Parameters.Add(parameterUserName);

            cn.Open();
            com.ExecuteNonQuery();
            cn.Close();
        }

        public void UpdateConnected(string userName1, string date1)
        {            
            SqlCommand com = new SqlCommand("spUpdateConnected", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserName = new SqlParameter()
            {
                ParameterName = "@UserName",
                Value = userName1
            };
            com.Parameters.Add(parameterUserName);

            SqlParameter parameterLastConnectionDate = new SqlParameter()
            {
                ParameterName = "@LastConnectionDate",
                Value = date1
            };
            com.Parameters.Add(parameterLastConnectionDate);

            cn.Open();
            com.ExecuteNonQuery();
            cn.Close();
        }

        public void InsertClient(string name1, string userName1, string date1)
        {            
            SqlCommand com = new SqlCommand("spInsertClient", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterName = new SqlParameter()
            {
                ParameterName = "@Name",
                Value = name1
            };
            com.Parameters.Add(parameterName);

            SqlParameter parameterUserName = new SqlParameter()
            {
                ParameterName = "@UserName",
                Value = userName1
            };
            com.Parameters.Add(parameterUserName);

            SqlParameter parameterLastConnectionDate = new SqlParameter()
            {
                ParameterName = "@LastConnectionDate",
                Value = date1
            };
            com.Parameters.Add(parameterLastConnectionDate);

            cn.Open();
            com.ExecuteNonQuery();
            cn.Close();
        }

        public void InsertMessage(string message1, string senderid1, 
            string date1, string recipientid1, string category1)
        {            
            SqlCommand com = new SqlCommand("spInsertMessage", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterMessageText = new SqlParameter()
            {
                ParameterName = "@MessageText",
                Value = message1
            };
            com.Parameters.Add(parameterMessageText);

            SqlParameter parameterUserId = new SqlParameter()
            {
                ParameterName = "@UserId",
                Value = int.Parse(senderid1)
            };
            com.Parameters.Add(parameterUserId);

            SqlParameter parameterSentDate = new SqlParameter()
            {
                ParameterName = "@SentDate",
                Value = date1
            };
            com.Parameters.Add(parameterSentDate);

            SqlParameter parameterRecipientId = new SqlParameter()
            {
                ParameterName = "@RecipientId",
                Value = int.Parse(recipientid1)
            };
            com.Parameters.Add(parameterRecipientId);

            SqlParameter parameterCategory = new SqlParameter()
            {
                ParameterName = "@Category",
                Value = category1
            };
            com.Parameters.Add(parameterCategory);

            cn.Open();
            com.ExecuteNonQuery();
            cn.Close();
        }

        public void DeleteClient(string id1)
        {            
            SqlCommand com = new SqlCommand("spDeleteClient", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterId = new SqlParameter()
            {
                ParameterName = "@Id",
                Value = int.Parse(id1)
            };
            com.Parameters.Add(parameterId);

            cn.Open();
            com.ExecuteNonQuery();
            cn.Close();
        }
        #endregion

        #region DBIntFunctions
        //GetClientIdByUserName will return the client id,
        //or -99 if the client does not exists
        public int GetClientIdByUserName(string userName1)
        {            
            int clientId = 0;
            SqlCommand com = new SqlCommand("spGetClientIdByUserName", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserName = new SqlParameter()
            {
                ParameterName = "@UserName",
                Value = userName1
            };
            com.Parameters.Add(parameterUserName);

            cn.Open();
            object obj1 = com.ExecuteScalar();
            if (obj1 == null)
                clientId = -99;
            else
                clientId = (int)obj1;
            cn.Close();
            return clientId;
        }
        #endregion

        #region DBBoolFunctions
        public bool UserNameExists(string userName1)
        {            
            SqlCommand com = new SqlCommand("spGetUserNameByUserName", cn);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserName = new SqlParameter()
            {
                ParameterName = "@UserName",
                Value = userName1
            };
            com.Parameters.Add(parameterUserName);

            cn.Open();
            object obj1 = com.ExecuteScalar();
            cn.Close();
            return obj1 != null;
        }
        #endregion

        #region DBDataTableFunctions
        public DataTable GetAllMessages()
        {            
            SqlDataAdapter adp = new SqlDataAdapter("spGetAllMessages", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;            
            
            DataTable dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }

        public DataTable GetAllClients()
        {            
            SqlDataAdapter adp = new SqlDataAdapter("spGetAllClients", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            
            DataTable dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }

        public DataTable GetMessagesByWord(string word1)
        {            
            SqlDataAdapter adp = new SqlDataAdapter("spGetMessagesByWord", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterWord = new SqlParameter()
            {
                ParameterName = "@Word",
                Value = word1
            };
            adp.SelectCommand.Parameters.Add(parameterWord);

            DataTable dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }

        public DataTable GetClientById(string id1)
        {            
            SqlDataAdapter adp = new SqlDataAdapter("spGetClientById", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterId = new SqlParameter()
            {
                ParameterName = "@Id",
                Value = int.Parse(id1)
            };
            adp.SelectCommand.Parameters.Add(parameterId);

            DataTable dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }

        public DataTable GetMessagesByDate(string date1)
        {            
            SqlDataAdapter adp = new SqlDataAdapter("spGetMessagesByDate", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterSentDate = new SqlParameter()
            {
                ParameterName = "@SentDate",
                Value = date1
            };
            adp.SelectCommand.Parameters.Add(parameterSentDate);

            DataTable dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }

        public DataTable GetClientByUserName(string userName1)
        {            
            SqlDataAdapter adp = new SqlDataAdapter("spGetClientByUserName", cn);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter parameterUserName = new SqlParameter()
            {
                ParameterName = "@UserName",
                Value = userName1
            };
            adp.SelectCommand.Parameters.Add(parameterUserName);

            DataTable dt = new DataTable();
            adp.Fill(dt);
            return dt;
        }

        public DataTable GetTableClientsForList()
        {            
            DataTable dt = new DataTable();
            dt.Columns.Add("clientID1", typeof(int));
            dt.Columns.Add("displayMember1", typeof(string));
            SqlCommand com = new SqlCommand("spGetAllClients", cn);
            com.CommandType = CommandType.StoredProcedure;

            cn.Open();
            SqlDataReader dr1 = com.ExecuteReader();
            while (dr1.Read())
            {
                DataRow drow = dt.NewRow();
                drow[0] = dr1[0];
                drow[1] = "ID: " + dr1[0] + ", Name: " + dr1[1] + ", User name: " + dr1[2];
                dt.Rows.Add(drow);
            }
            cn.Close();
            return dt;
        }
        #endregion                
    }
}
