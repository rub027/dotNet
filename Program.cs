// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");/* 

/* int myInt = 5;

myInt++;

Console.WriteLine("Este es mi variable = "+ myInt);

myInt += 7;

Console.WriteLine("Este es mi variable = "+ myInt);

myInt -= 8;

Console.WriteLine("Este es mi variable = "+ myInt); */

// int[] instsToCOmpress = new int[] {10, 15, 20, 25, 30, 12, 34};

// int totalValue = 0;


// for (int i = 0; i < instsToCOmpress.Length; i++){
//     totalValue += instsToCOmpress[i]; 
// }

// Console.WriteLine(" El total sumado es = "+ totalValue);

// totalValue = 0;

// foreach (int nuevaVariable in instsToCOmpress)
// {
//     totalValue += nuevaVariable;
// }
// Console.WriteLine(" El total sumado es = "+ totalValue);

// int prueba = instsToCOmpress.Sum();

// Console.WriteLine("El valor sumado es = "+ prueba); */



using System.Data;
using System.Globalization;
using Dapper;
using HelloWorld.Models;
using HelloWorld.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

IConfiguration config = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json")
     .Build();

DataContextDapper dapper = new DataContextDapper(config);
DataContextEF EntityFramework = new DataContextEF(config);



// Computer myComputer = new Computer()
//    {
//         Motherboard = "2690",
//         HasWifi = true,
//         HasLTE = false,
//         ReleaseDate = DateTime.Now,
//         Price = 943.87m,
//         VideoCard = "RTX 2060"

//    };

//    string sql = @"INSERT INTO TutorialAppSchema.Computer (
//           Motherboard,
//           HasWifi,
//           HasLTE,
//           ReleaseDate,
//           Price,
//           VideoCard 
//    ) VALUES ('" + myComputer.Motherboard 
//                + "','" + myComputer.HasWifi 
//                + "','" + myComputer.HasLTE 
//                + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
//                + "','" + myComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
//                + "','" + myComputer.VideoCard 
//           + "')";

// File.WriteAllText("log.txt","\n" + sql + "\n");

// using StreamWriter openFile = new("log.txt", append: true);

// openFile.WriteLine("\n" + sql + "\n");

// openFile.Close();

// Console.WriteLine(File.ReadAllText("log.txt"));

string computersJson = File.ReadAllText("Computers.json");

// Console.WriteLine(computersJson);

JsonSerializerOptions options = new JsonSerializerOptions()
{
     PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// JsonSerializerSettings settings1 = new JsonSerializerSettings
// {
//      NullValueHandling = NullValueHandling.Ignore
// };

IEnumerable<Computer>?  computersNewtonsoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computersJson);
IEnumerable<Computer>?  computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computersJson, options);



if(computersNewtonsoft != null)
{
     foreach( Computer computer in computersNewtonsoft)
     {
          // Console.WriteLine(computer.Motherboard);
             string sql = @"INSERT INTO TutorialAppSchema.Computer (
                    Motherboard,
                    HasWifi,
                    HasLTE,
                    ReleaseDate,
                    Price,
                    VideoCard 
             ) VALUES ('" + EscapeSingleQuote(computer.Motherboard )
                         + "','" + computer.HasWifi 
                         + "','" + computer.HasLTE 
                         + "','" + computer.ReleaseDate?.ToString("yyyy-MM-dd")
                         + "','" + computer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                         + "','" + EscapeSingleQuote(computer.VideoCard )
                    + "')";

          dapper.ExecuteSql(sql);
     }
}

JsonSerializerSettings settings = new JsonSerializerSettings()
{
     ContractResolver = new CamelCasePropertyNamesContractResolver(),
     // NullValueHandling = NullValueHandling.Ignore // Ignorar valores nulos al momento de serializar
};

string computersCopyNewtonsoft = JsonConvert.SerializeObject(computersNewtonsoft, settings);
File.WriteAllText("computersCopyNewtonsoft.txt","\n" + computersCopyNewtonsoft + "\n");

string computersCopySystem = System.Text.Json.JsonSerializer.Serialize(computersSystem, options);
File.WriteAllText("computersCopySystem.txt","\n" + computersCopySystem + "\n");

static string EscapeSingleQuote(string input)
{
     
     string output = input.Replace("'", "''");

     return output;
          
     
}