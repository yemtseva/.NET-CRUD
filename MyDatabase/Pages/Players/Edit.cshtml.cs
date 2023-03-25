using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyDatabase.Pages.Players
{
    public class EditModel : PageModel
    {
        public PlayerInfo playerInfo = new PlayerInfo();
        public String Message = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM players WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                playerInfo.id = "" + reader.GetInt32(0);
                                playerInfo.nickname = reader.GetString(1);
                                playerInfo.role = reader.GetString(2);
                                playerInfo.heroes = reader.GetString(3);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public void OnPost()
        {

            String id = Request.Query["id"];

            playerInfo.nickname = Request.Form["nickname"];
            playerInfo.role = Request.Form["role"];
            playerInfo.heroes = Request.Form["heroes"];

            if (playerInfo.heroes.Length == 0 || playerInfo.nickname.Length == 0 ||
                playerInfo.role.Length == 0)
            {
                Message = "All the field are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "UPDATE players " +
                                 "SET nickname=@nickname, role=@role, heroes=@heroes " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@nickname", playerInfo.nickname);
                        command.Parameters.AddWithValue("@role", playerInfo.role);
                        command.Parameters.AddWithValue("@heroes", playerInfo.heroes);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return;
            }

            Message = "The player was successfully updated!";
        }
    }
}
