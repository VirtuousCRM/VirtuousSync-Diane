using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;  //I added the Dapper nuget;

namespace Sync
{
    internal class ContactsDatabase
    {
        public void CreateContacts(IEnumerable<AbbreviatedContact> contacts)
        {
            string connectionString = "AConnectionString";
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = @"
                        INSERT INTO AbbreviatedContacts (Name, ContactType, ContactName, Address, Email, Phone)
                        VALUES (@Name, @ContactType, @ContactName, @Address, @Email, @Phone)";

                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var contact in contacts)
                        {
                            connection.Execute(sql, contact, transaction: transaction);
                        }
                        transaction.Commit();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
