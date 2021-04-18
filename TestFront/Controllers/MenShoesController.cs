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
    public class MenShoesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public MenShoesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
            select MenShoesID, MenShoesName, MenShoesPrice, MenShoesQuantity, MenShoesPhoto from dbo.menshoes";
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
        public JsonResult Post(MenShoes shoe)
        {
            string query = @"
            insert into dbo.menshoes
            (MenShoesName, MenShoesPrice, MenShoesQuantity, MenShoesPhoto) values
            (
            '" + shoe.MenShoesName + @"'
            ,'" + shoe.MenShoesPrice + @"'
            ,'" + shoe.MenShoesQuantity + @"'
            ,'" + shoe.MenShoesPhoto + @"'
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
        public JsonResult Put(MenShoes shoe)
        {
            string query = @"
            update dbo.menshoes set 
            MenShoesName = '" + shoe.MenShoesName + @"'
            ,MenShoesPrice = '" + shoe.MenShoesPrice + @"'
            ,MenShoesQuantity = '" + shoe.MenShoesQuantity + @"'
            ,MenShoesPhoto = '" + shoe.MenShoesPhoto + @"'
            where MenShoesID = " + shoe.MenShoesID + @" 
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
            delete from dbo.menshoes where MenShoesID = " + id + @" 
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
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }

    }
}
