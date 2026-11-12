using Payments.Business.Interfaces;
using Payments.Business.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Payments.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string _connectionString;

        public CompanyRepository()
        {
            _connectionString =
                ConfigurationManager
                    .ConnectionStrings["ESolutionsPayments"]
                    .ConnectionString;
        }

        public List<Company> GetAll()
        {
            var companies = new List<Company>();

            const string query = @"
                SELECT
                    Id,
                    Name,
                    LogoUrl
                FROM dbo.Companies
                WHERE IsActive = 1
                ORDER BY Name;";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        companies.Add(new Company
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = Convert.ToString(reader["Name"]),
                            LogoUrl = reader["LogoUrl"] == DBNull.Value
                                ? null
                                : Convert.ToString(reader["LogoUrl"])
                        });
                    }
                }
            }

            return companies;
        }
    }
}
