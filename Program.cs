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

 IConfiguration config = new ConfigurationBuilder()
     .AddJsonFile("appsettings.json")
     .Build();

DataContextDapper dapper = new DataContextDapper(config);
DataContextEF EntityFramework = new DataContextEF(config);


string sqlCommand = "SELECT GETDATE()";

DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);

Console.WriteLine(rightNow);


Computer myComputer = new Computer()
   {
        Motherboard = "2690",
        HasWifi = true,
        HasLTE = false,
        ReleaseDate = DateTime.Now,
        Price = 943.87m,
        VideoCard = "RTX 2060"

   };

EntityFramework.Add(myComputer);

EntityFramework.SaveChanges();


   string sql = @"INSERT INTO TutorialAppSchema.Computer (
          Motherboard,
          HasWifi,
          HasLTE,
          ReleaseDate,
          Price,
          VideoCard 
   ) VALUES ('" + myComputer.Motherboard 
               + "','" + myComputer.HasWifi 
               + "','" + myComputer.HasLTE 
               + "','" + myComputer.ReleaseDate.ToString("yyyy-MM-dd")
               + "','" + myComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
               + "','" + myComputer.VideoCard 
          + "')";

     // Console.WriteLine(sql);

     // Con Dapper
     bool result1 = dapper.ExecuteSql(sql);
     int result = dapper.ExecuteSqlWithRowCount(sql);

     Console.WriteLine(result1);
     Console.WriteLine(result);

     string sqlSelect = @"SELECT 
          Computer.ComputerId,
          Computer.Motherboard,
          Computer.HasWifi,
          Computer.HasLTE,
          Computer.ReleaseDate,
          Computer.Price,
          Computer.VideoCard 
      FROM TutorialAppSchema.Computer";

     IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

     Console.WriteLine(
               "'ComputerId','Motherboard','HasWifi','HasLTE',`ReleaseDate`,`Price`,'VideoCard'"
     );
     foreach(Computer singleComputer in computers)
     {

          Console.WriteLine(
               "'" + singleComputer.ComputerId 
               + "','" + singleComputer.Motherboard 
               + "','" + singleComputer.HasWifi 
               + "','" + singleComputer.HasLTE 
               + "','" + singleComputer.ReleaseDate.ToString("yyyy-MM-dd")
               + "','" + singleComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
               + "','" + singleComputer.VideoCard 
          + "'"
          );
     }
     
     IEnumerable<Computer>? computersEF = EntityFramework.Computer?.ToList<Computer>();

     if(computersEF != null)
     {

          Console.WriteLine(
                    "'ComputerId','Motherboard','HasWifi','HasLTE',`ReleaseDate`,`Price`,'VideoCard'"
          );
          foreach(Computer singleComputer in computersEF)
          {

               Console.WriteLine(
                    "'" + singleComputer.ComputerId 
                    + "','" + singleComputer.Motherboard 
                    + "','" + singleComputer.HasWifi 
                    + "','" + singleComputer.HasLTE 
                    + "','" + singleComputer.ReleaseDate.ToString("yyyy-MM-dd")
                    + "','" + singleComputer.Price.ToString("0.00", CultureInfo.InvariantCulture)
                    + "','" + singleComputer.VideoCard 
               + "'"
               );
          }
     }

     

//    Console.WriteLine(myComputer.Motherboard);
//    Console.WriteLine(myComputer.HasWifi);
//    Console.WriteLine(myComputer.HasLTE);
//    Console.WriteLine(myComputer.VideoCard);