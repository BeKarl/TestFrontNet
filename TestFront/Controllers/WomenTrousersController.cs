using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using TestFront.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TestFront.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WomenTrousersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public WomenTrousersController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
            select WomenTrousersID, WomenTrousersName, WomenTrousersPrice, WomenTrousersQuantity, WomenTrousersPhoto 
            from dbo.womentrousers";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ClothesAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(WomenTrousers trou)
        {
            string query = @"
            insert into dbo.womentrousers
            (WomenTrousersName, WomenTrousersPrice, WomenTrousersQuantity, WomenTrousersPhoto) values
            (
            '" + trou.WomenTrousersName + @"'
            ,'" + trou.WomenTrousersPrice + @"'
            ,'" + trou.WomenTrousersQuantity + @"'
            ,'" + trou.WomenTrousersPhoto + @"'
            )";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ClothesAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successful");
        }

        [HttpPut]
        public JsonResult Put(WomenTrousers trou)
        {
            string query = @"
            update dbo.womentrousers set 
            WomenTrousersName = '" + trou.WomenTrousersName + @"'
            ,WomenTrousersPrice = '" + trou.WomenTrousersPrice + @"'
            ,WomenTrousersQuantity = '" + trou.WomenTrousersQuantity + @"'
            ,WomenTrousersPhoto = '" + trou.WomenTrousersPhoto + @"'
            where WomenTrousersID = " + trou.WomenTrousersID + @" 
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ClothesAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Update Successful");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
            delete from dbo.womentrousers where WomenTrousersID = " + id + @" 
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("ClothesAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Delete Successful");
        }
        [Route("SaveFile")]
        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Photos/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_env.ContentRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
