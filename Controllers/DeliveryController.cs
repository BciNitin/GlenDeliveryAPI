using Azure;
using BCILLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net.NetworkInformation;
using WebApplication1.Data;
using WebApplication1.Model;
using static System.Net.Mime.MediaTypeNames;

namespace GlenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly Logger oLog;
        private readonly ILogger<DeliveryController> _logger;
        public DeliveryController(DataContext dbContext, ILogger<DeliveryController> logger, Logger _oLog)
        { 
            _dbContext = dbContext;
            _logger = logger;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            oLog = _oLog;
            oLog.AppPath = path;
            oLog.ErrorFileName = "Glen";
            oLog.AppPath = path;
            oLog.EnableLogging = true;
            oLog.FileType = "DELIVERY";
            oLog.MaxFileSize = 5;
            oLog.NoOfDaysToDeleteFile = 5;
            oLog.WriteLog("Init Logging...");
            
        }

        [HttpPost("SaveDelivery")]
        public async Task<ActionResult> SaveDelivery(List<Delivery> RegisterObject)
        {
            oLog.WriteLog("Initilize Logger To SaveDelivery----");
            string message = "Delivery added successfully";
            Delivery Exdelivery = new Delivery();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    foreach (var item in RegisterObject)
                    {
                        oLog.WriteLog("SalesOrderNo : " + item.SalesOrderNo + " CustomerCode : " + item.CustomerCode + " CustomerName : " + item.CustomerName + " DeliveryNo : " + item.DeliveryNo + " DocumentType : " + item.DocumentType + " CustomerCode : " + item.CustomerCode + " ItemCode : " + item.ItemCode + " Quantity : " + item.Quantity);
                        // _logger.LogInformation("SalesOrderNo : " + item.SalesOrderNo + " CustomerCode : " + item.CustomerCode + " CustomerName : " + item.CustomerName + " DeliveryNo : " + item.DeliveryNo + " DocumentType : " + item.DocumentType + " CustomerCode : " + item.CustomerCode + " ItemCode : " + item.ItemCode + " Quantity : " + item.Quantity);
                        var del = new Delivery
                        {
                            SalesOrderNo = item.SalesOrderNo,
                            CustomerCode = item.CustomerCode,
                            CustomerName = item.CustomerName,
                            DeliveryNo = item.DeliveryNo,
                            DocumentType = item.DocumentType,
                            ItemCode = item.ItemCode,
                            Quantity = item.Quantity,
                        };
                        Exdelivery = del;
                       await _dbContext.Delivery.AddAsync(del);
                        var response = _dbContext.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    message = "Sales Order No"+ " " + Exdelivery.SalesOrderNo +" "+ " Exception  :" + ex.Message.ToString();
                    oLog.WriteLog("SalesOrderNo : " + " " + Exdelivery.SalesOrderNo + " " + " CustomerCode : " + Exdelivery.CustomerCode + " " + " CustomerName : " + Exdelivery.CustomerName + " DeliveryNo : " + Exdelivery.DeliveryNo + " " + " DocumentType : " + Exdelivery.DocumentType + " " + " CustomerCode : " + " " + Exdelivery.CustomerCode + " " + " ItemCode : " + Exdelivery.ItemCode + " " + " Quantity : " + Exdelivery.Quantity + " " + " Exception : " + ex.Message.ToString());
                     // _logger.LogError("SalesOrderNo : " + " " + Exdelivery.SalesOrderNo + " " + " CustomerCode : " + Exdelivery.CustomerCode + " " + " CustomerName : " + Exdelivery.CustomerName + " DeliveryNo : " + Exdelivery.DeliveryNo + " " + " DocumentType : " + Exdelivery.DocumentType +" "+ " CustomerCode : "+" " + Exdelivery.CustomerCode +" "+ " ItemCode : " + Exdelivery.ItemCode +" "+ " Quantity : " + Exdelivery.Quantity +" "+ " Exception : " + ex.Message.ToString());
                     transaction.Rollback();
                }
            }
            return Ok(message);
        }

        [HttpPut("UpdateDeliveryDetails")]
        public async Task<IActionResult> UpdateDelivery(Delivery delivery)
        {
            oLog.WriteLog("Initilize Logger To UpdateDelivery----");
            string message = "Delivery Update successfully";
            int response = 0;
            var result = _dbContext.Delivery.Where(x => x.DeliveryNo == delivery.DeliveryNo && x.SalesOrderNo == delivery.SalesOrderNo && x.ItemCode == delivery.ItemCode).ToList();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (result.Count() > 0)
                    {
                        foreach (var item in result)
                        {
                            oLog.WriteLog("SalesOrderNo : " + item.SalesOrderNo + " CustomerCode : " + item.CustomerCode + " CustomerName : " + item.CustomerName + " DeliveryNo : " + item.DeliveryNo + " DocumentType : " + item.DocumentType + " CustomerCode : " + item.CustomerCode + " ItemCode : " + item.ItemCode + " Quantity : " + item.Quantity);
                            //_logger.LogInformation("SalesOrderNo : " + item.SalesOrderNo + " CustomerCode : " + item.CustomerCode + " CustomerName : " + item.CustomerName + " DeliveryNo : " + item.DeliveryNo + " DocumentType : " + item.DocumentType + " CustomerCode : " + item.CustomerCode + " ItemCode : " + item.ItemCode + " Quantity : " + item.Quantity);
                            item.CustomerName = delivery.CustomerName;
                            item.CustomerCode = delivery.CustomerCode;
                            item.Quantity = delivery.Quantity;
                            item.DocumentType = delivery.DocumentType;
                            response = await _dbContext.SaveChangesAsync();

                        }
                        transaction.Commit();
                        if (response.ToString().Count() > 0)
                        {
                            var _response = new { Message = message, UpdatedRows = response };
                            return Ok(_response);
                        }
                        else
                        {

                            return BadRequest();
                        }

                    }
                    else
                    {
                        oLog.WriteLog("Please Enter the Correct DeliveryNo , SalesOrderNo,ItemCode ");
                        return NotFound("Please Enter the Correct DeliveryNo , SalesOrderNo,ItemCode ");
                    }
                }
                catch (Exception ex)
                {
                    message = "Sales Order No" + " " + delivery.SalesOrderNo + " " + " Exception  :" + ex.Message.ToString();
                    oLog.WriteLog("SalesOrderNo : " + " " + delivery.SalesOrderNo + " " + " CustomerCode : " + delivery.CustomerCode + " " + " CustomerName : " + delivery.CustomerName + " DeliveryNo : " + delivery.DeliveryNo + " " + " DocumentType : " + delivery.DocumentType + " " + " CustomerCode : " + " " + delivery.CustomerCode + " " + " ItemCode : " + delivery.ItemCode + " " + " Quantity : " + delivery.Quantity + " " + " Exception : " + ex.Message.ToString());
                    //_logger.LogError("SalesOrderNo : " + " " + delivery.SalesOrderNo + " " + " CustomerCode : " + delivery.CustomerCode + " " + " CustomerName : " + delivery.CustomerName + " DeliveryNo : " + delivery.DeliveryNo + " " + " DocumentType : " + delivery.DocumentType + " " + " CustomerCode : " + " " + delivery.CustomerCode + " " + " ItemCode : " + delivery.ItemCode + " " + " Quantity : " + delivery.Quantity + " " + " Exception : " + ex.Message.ToString());
                    transaction.Rollback();
                    throw;
                }

            }
        }
    }
}
