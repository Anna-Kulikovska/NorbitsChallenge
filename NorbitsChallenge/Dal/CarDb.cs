using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NorbitsChallenge.Models;

namespace NorbitsChallenge.Dal
{
    public class CarDb
    {
        private readonly IConfiguration _config;

        public CarDb(IConfiguration config)
        {
            _config = config;
        }

        /*public int GetTireCount(int companyId, string licensePlate)
        {
            int result = 0;

            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"select * from car where companyId = {companyId} and licenseplate = '{licensePlate}'";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = (int)reader["tireCount"];
                        }
                    }
                }
            }

            return result;
        }*/
        public CarModel GetCarInfo(int companyId, string licensePlate)
        {
            var carInfo = new CarModel();

            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"select * from car where companyId = {companyId} and licenseplate = '{licensePlate}'";

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        if (reader.Read())
                        {
                            carInfo.companyId = (int)reader["companyId"];
                            carInfo.licensePlate = (string)reader["licensePlate"];
                            carInfo.model = (string)reader["model"];
                            carInfo.brand = (string)reader["brand"];
                            carInfo.description = (string)reader["description"];
                            carInfo.TireCount = (int)reader["tireCount"];
                        }
                    }
                }
            }

            return carInfo;
        }

        public List<CarModel> GetAllCars(int companyId)
        {

            List<CarModel> allCars = new List<CarModel>();

            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"select * from car where companyId = {companyId}";

                    using (var reader = command.ExecuteReader())
                    {

                        foreach (DbDataRecord car in reader)
                        {
                            Debug.WriteLine(car);

                            var carInfo = new CarModel();

                            carInfo.companyId = (int)car["companyId"];
                            carInfo.licensePlate = (string)car["licensePlate"];
                            carInfo.model = (string)car["model"];
                            carInfo.brand = (string)car["brand"];
                            carInfo.description = (string)car["description"];
                            carInfo.TireCount = (int)car["tireCount"];

                            allCars.Add(carInfo);
                            //Debug.WriteLine(carInfo.companyId);
                        }

                    }
                }
            }

            return allCars;
        }
        public bool AddCar(int companyId, string licensePlate, string model, string brand, string description, int tireCount)
        {
            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"select * from car where companyId = {companyId} and licenseplate = '{licensePlate}'";

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            throw new Exception("Car is alredy in the DB");
                        }
                    }
                }
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"insert into car (companyId, licenseplate, model, brand, description, tireCount) " +
                        $"values({companyId}, '{licensePlate}', '{model}', '{brand}', '{description}', {tireCount})";

                    command.ExecuteNonQuery();

                }
            }

            return true;
        }

        public bool EditCar(string licensePlate, string model, string brand, string description, int tireCount, string oldLicensePlate)
        {

            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"update car set licenseplate = '{licensePlate}', model = '{model}', brand = '{brand}', description = '{description}', tireCount = {tireCount}" +
                        $" where licenseplate = '{oldLicensePlate}'";
                    if (command.ExecuteNonQuery() == 0)
                    {
                        throw new Exception();
                    }

                }
            }

            return true;
        }

        public bool DeleteCar(string licensePlate)
        {
            var connectionString = _config.GetSection("ConnectionString").Value;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand { Connection = connection, CommandType = CommandType.Text })
                {
                    command.CommandText = $"delete from car where licenseplate='{licensePlate}'";

                    if (command.ExecuteNonQuery() == 0)
                    {
                        throw new Exception();
                    }
                }
            }

            return true;
        }
    }
}
