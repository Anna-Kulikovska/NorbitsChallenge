using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NorbitsChallenge.Bll;

namespace NorbitsChallenge.Dal
{
    public class SettingsDb
    {
        private readonly IConfiguration _config;

        public SettingsDb(IConfiguration config)
        {
            _config = config;
        }

        public string GetCompanyName(int companyId)
        {
            string companyName = "";

            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand {Connection = connection, CommandType = CommandType.Text})
                {
                    command.CommandText = "select * from Settings where setting = 'companyname'";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["companyId"].ToString() == companyId.ToString())
                            {
                                companyName = reader["settingValue"].ToString();
                            }
                        }
                    }
                }
            }

            return companyName;
        }

        public List<Setting> GetSettings(int companyId)
        {
            var settings = new List<Setting>();

            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"select * from settings where companyId = {companyId}";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var setting = new Setting();

                            setting.Key = reader["setting"].ToString();
                            setting.Value = reader["settingValue"].ToString();
                            setting.CompanyId = companyId;

                            settings.Add(setting);
                        }
                    }
                }
            }

            return settings;
        }

        public void UpdateSetting(Setting setting, int companyId)
        {
            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand {Connection = connection, CommandType = CommandType.Text})
                {
                    command.CommandText = $"update settings set settingValue = '{setting.Value}' where setting = '{setting.Key}'";

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}