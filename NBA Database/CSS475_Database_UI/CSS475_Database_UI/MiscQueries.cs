using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSS475_Database_UI
{
    public partial class MiscQueries : Form
    {
        private HomePage activeHome;

        public MiscQueries()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes list player by FG percentage query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listByFGPercent_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select PNAME,FG FROM PLAYER_PROFILE,PLAYER_STATS WHERE PLAYER_STATS.PLAYER_ID = PLAYER_PROFILE.PLAYER_ID ");
            sb.Append("ORDER BY FG DESC");
            String sql = sb.ToString();

            activeHome.GetData(sql);
        }

        /// <summary>
        /// Public set method to set the Homepage query results will be displayed to.
        /// </summary>
        /// <param name="newActive">New HomePage.</param>
        public void SetActiveHome(HomePage newActive)
        {
            activeHome = newActive;
        }

        /// <summary>
        /// Executes Divisions by Wins Percentage query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void divisionWinsPercent_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select divName,SUM(champWins) FROM DIVISION,TEAM WHERE d_Id = divId ");
            sb.Append("GROUP BY divName");
            String sql = sb.ToString();

            activeHome.GetData(sql);
        }

        /// <summary>
        /// Executes Coaches by Wins query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void coachesByWins_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select coach_Name,c_champWins FROM COACH ");
            sb.Append("ORDER BY c_champWins");
            String sql = sb.ToString();

            activeHome.GetData(sql);
        }

        /// <summary>
        /// Executes Stadium by Team query.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stadiumByTeam_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select team_Name,Stadium_Name FROM TEAM,STADIUM WHERE S_CityId = t_City");
            String sql = sb.ToString();

            activeHome.GetData(sql);
        }
    }
}
