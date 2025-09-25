using Microsoft.AspNetCore.Mvc;
using ServiceLayerWebApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace ServiceLayerWebApi.Services
{
    public class UserCrud
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public UserCrud()
        {
        }
        public UserCrud(IConfiguration config)
        {
            _configuration = config;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public ApiResponseModel<string> InsertUser(UserRequestModel requestModel)
        {
            ApiResponseModel<string> response = new ApiResponseModel<string>();
            using(SqlConnection conn=new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_insertUsers", conn);
                cmd.CommandType=CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", requestModel.Name);
                cmd.Parameters.AddWithValue("@email", requestModel.Email);
                cmd.Parameters.AddWithValue("@password", requestModel.Password);
                cmd.Parameters.AddWithValue("@age", requestModel.Age);
                conn.Open();
                int result=cmd.ExecuteNonQuery();
                if (result>0)
                {
                    response.IsSuccess = true;
                    response.Message = "User inserted successfully";
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Failed to insert user";
                    return response;
                }
            }
        }
    }
}
