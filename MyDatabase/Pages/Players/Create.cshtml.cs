using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyDatabase.Pages.Players
{
    public class CreateModel : PageModel
    {
        public PlayerInfo playerInfo = new PlayerInfo();
        public String Message = "";

        public void OnGet()
        {
        }

        public void OnPost() 
        { 
            playerInfo.nickname = Request.Form["nickname"];
            playerInfo.role = Request.Form["role"];
            playerInfo.heroes = Request.Form["heroes"];

            if(playerInfo.heroes.Length == 0 || playerInfo.nickname.Length == 0 || 
                playerInfo.role.Length == 0)
            {
                Message = "All the field are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO players " +
                                 "(nickname, role, heroes) VALUES " +
                                 "(@nickname, @roles, @heroes);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nickname", playerInfo.nickname);
                        command.Parameters.AddWithValue("@roles", playerInfo.role);
                        command.Parameters.AddWithValue("@heroes", playerInfo.heroes);

                        command.ExecuteNonQuery();
                        
                    }
                }
            }
            catch(Exception ex)
            {
                Message = ex.Message;
                return;
            }

            playerInfo.nickname = "";
            playerInfo.role = "";
            playerInfo.heroes = "";
            Message = "New Client Added Correctly";
        }
    }
}
