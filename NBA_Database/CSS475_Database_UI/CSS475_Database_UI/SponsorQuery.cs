using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CSS475_Database_UI;

namespace NBA_Sponsor
{
   public partial class SponsorQuery : Form
   {           
      public SponsorQuery()
      {
         InitializeComponent();

      }
      
      /*Query to retrieve all players who have sponsor contract(s) in a team (user inputs team name)
       *    each row is a financial sponsorship contract term between a player and a sponsor
       * 
       * Attributes to display:
       *    1/ PLAYER NAME
       *    2/ SPONSOR NAME
       *    3/ AMOUNT: the monetary value of the contract
       *    4/ START DATE
       *    5/ END DATE
       *    6/ LENGTH: the length of the contract in years
       *    
       * NOTE: this query includes both current and expired contracts
       *       one player could have multiple sponsorship contracts with different sponsors
       * 
       * User input (in 'Team Name' textbox): name of the team to be checked   
       */

      private void btnTeamGetSpContract_Click(object sender, EventArgs e)
      {
         List<string> result = new List<string>();
         String format = "{0, -20} {1, -20} {2, -10} {3, 12} {4, 12} {5, 15}";   //used to align columns
         String header = String.Format(format, "PLAYER NAME", "SPONSOR NAME", "AMOUNT", "START DATE", "END DATE", "LENGTH (Years)");
         
         using (SqlConnection connection = new SqlConnection(connectionString().ConnectionString))
         {
            connection.Open();
            String sql = "SELECT Pname, sponsorName, amount, startDate, endDate, DATEDIFF(year, startDate, endDate) AS LENGTH " +
                           "FROM PLAYER_PROFILE AS PP, PLAYER_CONTRACT AS PC, SPONSOR, SPONSOR_CONTRACT, TEAM " +
                           $"WHERE TEAM = team_Name AND team_Name = '{txtTeamName.Text}' AND PP.PLAYER_ID = PC.PLAYER_ID AND " +
		                         "sponsorId = spId AND PP.PLAYER_ID = plId " +
                           "ORDER BY Pname, sponsorName;";
            result = retrieveData(sql, connection, format);
            connection.Close();
         }
         result.Insert(0, header);
         lstBoxOutput.DataSource = result;
      }


      /*Query to retrieve the total amount of sponsor values of player(s) in a team (user inputs team name)
       *    who achieved above average Field Goal percentage
       *    This could be used as a performance metric between the player actual performance vs. financial incentives offered to him 
       * 
       * Attributes to display:
       *    1/ PLAYER NAME
       *    2/ FIELD GOAL %
       *    3/ TOTAL AMOUNT: the sum of value of all contracts signed by the player
       *    4/ # of CONTRACT: how many sponsorship contracts does the player have currently?
       *    
       * NOTE: this query includes only current (valid) contracts
       * 
       * User input (in 'Team Name' textbox): name of the team to be checked   
       */
      private void btnGetTotalSponsorWAboveAvgFG_Click(object sender, EventArgs e)
      {
         List<string> result = new List<string>();
         String format = "{0, -30} {1, -12} {2, 15} {3, 15}";     //used to align columns
         String header = String.Format(format, "PLAYER NAME", "FIELD GOAL %", "TOTAL AMOUNT", "# of CONTRACT");

         using (SqlConnection connection = new SqlConnection(connectionString().ConnectionString))
         {
            connection.Open();
            String sql = "SELECT Pname, FG, SUM(amount) as SP, COUNT(*) as CT " +
                           "FROM PLAYER_PROFILE AS PP, PLAYER_CONTRACT AS PC, SPONSOR_CONTRACT, TEAM, PLAYER_STATS AS PS " +
                           $"WHERE TEAM = team_Name AND team_Name = '{txtTeamName.Text}' AND PP.PLAYER_ID = PC.PLAYER_ID AND " +
		                      "PP.PLAYER_ID = plId AND PP.PLAYER_ID = PS.PLAYER_ID AND endDate > GETDATE() AND FG > " +
		                      "(SELECT AVG(FG) " +
		                      "FROM PLAYER_PROFILE AS PP, PLAYER_STATS AS PS, PLAYER_CONTRACT AS PC, TEAM " +
		                      "WHERE TEAM = team_Name AND team_Name = 'seattle sonics' AND PP.PLAYER_ID = PS.PLAYER_ID AND PC.PLAYER_ID = PS.PLAYER_ID) " +
                           "GROUP BY Pname, FG " +
                           "ORDER BY SP DESC;";
            result = retrieveData(sql, connection, format);
            connection.Close();
         }
         result.Insert(0, header);
         lstBoxOutput.DataSource = result;
      }


      /*Query to retrieve the top ten players with the highest amount of sponsorship in the whole NBA league
       *    This could be used as one way to check for the current hottest players since sponsorship often
       *    favors the best performance players 
       * 
       * Attributes to display:
       *    1/ PLAYER NAME
       *    2/ TEAM NAME: the current team of the player
       *    3/ TOTAL AMOUNT: the sum of value of all contracts signed by the player
       *    4/ # of CONTRACT: how many sponsorship contracts does the player have currently?
       *    
       * NOTE: this query includes only current (valid) contracts
       * 
       * User input: NONE 
       */
      private void btnTopTenSponsored_Click(object sender, EventArgs e)
      {
         List<string> result = new List<string>();
         String format = "{0, -30} {1, -30} {2, 15} {3, 15}";     //used to align columns
         String header = String.Format(format, "PLAYER NAME", "TEAM NAME", "TOTAL AMOUNT", "# of CONTRACT");

         using (SqlConnection connection = new SqlConnection(connectionString().ConnectionString))
         {
            connection.Open();
            String sql = @"SELECT TOP (10) Pname, team_Name, SUM(amount) as SP,COUNT(*) as CT
                           FROM PLAYER_PROFILE AS PP, SPONSOR, SPONSOR_CONTRACT, PLAYER_CONTRACT AS PC, TEAM
                           WHERE TEAM = team_Name AND PP.PLAYER_ID = PC.PLAYER_ID AND
		                      sponsorId = spId AND PP.PLAYER_ID = plId AND endDate > GETDATE()
                           GROUP BY Pname, team_Name
                           ORDER BY SP DESC;";
            result = retrieveData(sql, connection, format);
            connection.Close();
         }
         result.Insert(0, header);
         lstBoxOutput.DataSource = result;
      }


      /*Query to retrieve the total amount of financial sponsor aggregated for each industry
       *    This could be used as a market research metric to see which industries have a higher interest in
       *    advertising with NBA (similar customer base or product?)
       * 
       * Attributes to display:
       *    1/ SPONSOR INDUSTRY: the aggregated industry (multiple businesses in 1 industry)
       *    2/ TOTAL AMOUNT: the sum of value of all contracts offered by such industry
       *    
       * NOTE: this query includes only current (valid) contracts
       * 
       * User input: NONE 
       */
      private void btnTotalPerIndustry_Click(object sender, EventArgs e)
      {
         List<string> result = new List<string>();
         String format = "{0, -20} {1, 20}";     //used to align columns
         String header = String.Format(format, "SPONSOR INDUSTRY", "TOTAL AMOUNT");

         using (SqlConnection connection = new SqlConnection(connectionString().ConnectionString))
         {
            connection.Open();
            String sql = @"SELECT sponsorBusiness, SUM(amount) AS Total
                           FROM SPONSOR, SPONSOR_CONTRACT
                           WHERE sponsorId = spId AND endDate > GETDATE()
                           GROUP BY sponsorBusiness
                           ORDER BY Total DESC;";
            result = retrieveData(sql, connection, format);
            connection.Close();
         }
         result.Insert(0, header);
         lstBoxOutput.DataSource = result;
      }


      //Function to build connection string to the SQL server (Azure SQL)
      public SqlConnectionStringBuilder connectionString()
      {
         SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
         builder.DataSource = "css475-10.database.windows.net";
         builder.UserID = "css475";
         builder.Password = "475cssqaz#";
         builder.InitialCatalog = "CSS475";
         return builder;
      }

        /*Function to retrieve data from the SQL server and format the data based on parameter input
              Parameters:
                 1/ String sql: the SQL query statement
                 2/ SqlConnection connection: the connection string to connect to the server
                 3/ String format: the column format for alignment
        */
        static List<string> retrieveData(String sql, SqlConnection connection, String format)
      {
         List<string> result = new List<string>();
         StringBuilder sb = new StringBuilder();
         using (SqlCommand command = new SqlCommand(sql, connection))
         {
            using (SqlDataReader reader = command.ExecuteReader())
            {
               while (reader.Read())
               {
                  string[] data = new string[reader.FieldCount];
                  //Retrieve each colunm data of a row
                  for (int i = 0; i < reader.FieldCount; i++)
                  {
                     String current = reader.GetValue(i).ToString();
                     //Check for DateTime datatype to remove the unnecessary hours part
                     if (reader.GetFieldType(i).ToString() == "System.DateTime")
                     {
                        current = current.Remove((current.Length - 11));   //last 11 chars are the hours
                     }
                     data[i] = current;
                  }
                  sb.AppendFormat(format, data);
                  result.Add(sb.ToString());
                  sb.Clear();
               }
            }
         }
         return result;
      }
    }
}
