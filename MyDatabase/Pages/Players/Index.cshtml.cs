using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyDatabase.Pages.Players
{
    public class IndexModel : PageModel
    {
        public List<PlayerInfo> PlayerInfos = new List<PlayerInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM players";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                PlayerInfo info = new PlayerInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.nickname = reader.GetString(1);
                                info.role = reader.GetString(2);
                                info.heroes = reader.GetString(3);
                                info.created_at = reader.GetDateTime(4).ToString();

                                PlayerInfos.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class PlayerInfo
    { 
        public String id;
        public String nickname;
        public String role;
        public String heroes;
        public String created_at;
         
    }
}
