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
    public class WomenClothesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public WomenClothesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
            select WomenClothesID, WomenClothesName, WomenClothesPrice, WomenClothesQuantity, WomenClothesPhoto 
            from dbo.womenclothes";
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
        public JsonResult Post(WomenClothes clot)
        {
            string query = @"
            insert into dbo.womenclothes 
            (WomenClothesName, WomenClothesPrice, WomenClothesQuantity, WomenClothesPhoto) values
            (
            '" + clot.WomenClothesName + @"'
            ,'" + clot.WomenClothesPrice + @"'
            ,'" + clot.WomenClothesQuantity + @"'
            ,'" + clot.WomenClothesPhoto + @"'
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
        public JsonResult Put(WomenClothes clot)
        {
            string query = @"
            update dbo.womenclothes set 
            WomenClothesName = '" + clot.WomenClothesName + @"'
            ,WomenClothesPrice = '" + clot.WomenClothesPrice + @"'
            ,WomenClothesQuantity = '" + clot.WomenClothesQuantity + @"'
            ,WomenClothesPhoto = '" + clot.WomenClothesPhoto + @"'
            where WomenClothesID = " + clot.WomenClothesID + @" 
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
            delete from dbo.womenclothes where WomenClothesID = " + id + @" 
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