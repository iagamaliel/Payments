using Payments.Business.Interfaces;
using Payments.Business.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Payments.Data.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly string _connectionString;

        public ChannelRepository(string connectionString = null)
        {
            _connectionString = connectionString ??
                ConfigurationManager.ConnectionStrings["ESolutionsPayments"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(_connectionString))
                throw new InvalidOperationException(
                    "ESolutionsPayments connection string is not configured.");
        }

        public ChannelConfiguration GetConfiguration(string channelCode)
        {
            if (string.IsNullOrWhiteSpace(channelCode))
                return null;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("dbo.usp_GetChannelConfiguration", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@ChannelCode", SqlDbType.VarChar, 10).Value =
                    channelCode.Trim();

                connection.Open();

                using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!reader.Read())
                        return null;

                    return new ChannelConfiguration
                    {
                        ChannelCode = reader["ChannelCode"].ToString(),
                        ServiceUrl = reader["ServiceUrl"].ToString(),
                        ApiKey = reader["ApiKey"].ToString(),
                        ShowMenu = Convert.ToBoolean(reader["ShowMenu"]),
                        DestinationPage = reader["DestinationPage"].ToString()
                    };
                }
            }
        }
    }
}
