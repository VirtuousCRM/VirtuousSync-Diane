using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sync
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Sync().GetAwaiter().GetResult();
        }

        private static async Task Sync()
        {
            var apiKey = "v_eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiN2VhYTBhNTQtYTBiZC00OTNlLWFjNDMtZjNjZGEwZmVlNWQ5IiwiZXhwIjoyMTQ3NDgzNjQ3LCJpc3MiOiJodHRwczovL2FwcC52aXJ0dW91c3NvZnR3YXJlLmNvbSIsImF1ZCI6Imh0dHBzOi8vYXBpLnZpcnR1b3Vzc29mdHdhcmUuY29tIn0.oN0bfmYMS7lPxGtVH3ouEVhD0Kuzoqa2nAnuvPTyPpk";
            var configuration = new Configuration(apiKey);
            var virtuousService = new VirtuousService(configuration);

            var skip = 0;
            var take = 100;
            var maxContacts = 1000;
            var hasMore = true;
            var filteredState = "AZ";
            var filteredRecords = new List<AbbreviatedContact>();

            //get the records and keep a filtered list for the state of AZ
            do
            {
                var contacts = await virtuousService.GetContactsAsync(skip, take);
                skip += take;
                var listForState = contacts.List.Where(c => c.Address.Contains(filteredState)).ToList();
                filteredRecords.AddRange(listForState);
                hasMore = skip > maxContacts;
            }
            while (!hasMore);
            
            //add the result to a database.
            //this will work once a database and a table have been created for the results. 
            var dbClass = new ContactsDatabase();
            dbClass.CreateContacts(filteredRecords);
        }
    }
}
