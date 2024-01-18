using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using WebApplication1.Data;
using WebApplication1.Model;

namespace GlenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly ILogger<DeliveryController> _logger;
        public DeliveryController(DataContext dbContext, ILogger<DeliveryController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("SaveDelivery")]
        public async Task<ActionResult> SaveDelivery(List<Delivery> RegisterObject)
        {

            string message = "Delivery added successfully";
            Delivery Exdelivery = new Delivery();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    foreach (var item in RegisterObject)
                    {
                        _logger.LogInformation("SalesOrderNo : " + item.SalesOrderNo + " CustomerCode : " + item.CustomerCode + " CustomerName : " + item.CustomerName + " DeliveryNo : " + item.DeliveryNo + " DocumentType : " + item.DocumentType + " CustomerCode : " + item.CustomerCode + " ItemCode : " + item.ItemCode + " Quantity : " + item.Quantity);
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
                        _dbContext.Delivery.Add(del);
                        var response = _dbContext.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    message += "Sales Order No"+ " " + Exdelivery.SalesOrderNo +" "+ " Exception  :" + ex.Message.ToString();
                    _logger.LogError("SalesOrderNo : " + " " + Exdelivery.SalesOrderNo + " " + " CustomerCode : " + Exdelivery.CustomerCode + " " + " CustomerName : " + Exdelivery.CustomerName + " DeliveryNo : " + Exdelivery.DeliveryNo + " " + " DocumentType : " + Exdelivery.DocumentType +" "+ " CustomerCode : "+" " + Exdelivery.CustomerCode +" "+ " ItemCode : " + Exdelivery.ItemCode +" "+ " Quantity : " + Exdelivery.Quantity +" "+ " Exception : " + ex.Message.ToString());
                    transaction.Rollback();
                }
            }
            return Ok(message);
        }

        [HttpPut("UpdateDeliveryDetails")]
        public async Task<IActionResult> UpdateDelivery(Delivery delivery)
        {
            string message = "Delivery Update successfully";
            var result = _dbContext.Delivery.Where(x => x.DeliveryNo == delivery.DeliveryNo && x.SalesOrderNo == delivery.SalesOrderNo && x.ItemCode == delivery.ItemCode).ToList();
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (result.Count() > 0)
                    {
                        foreach (var item in result)
                        {
                            _logger.LogInformation("SalesOrderNo : " + item.SalesOrderNo + " CustomerCode : " + item.CustomerCode + " CustomerName : " + item.CustomerName + " DeliveryNo : " + item.DeliveryNo + " DocumentType : " + item.DocumentType + " CustomerCode : " + item.CustomerCode + " ItemCode : " + item.ItemCode + " Quantity : " + item.Quantity);
                            item.CustomerName = delivery.CustomerName;
                            item.CustomerCode = delivery.CustomerCode;
                            item.Quantity = delivery.Quantity;
                            item.DocumentType = delivery.DocumentType;
                            var response = await _dbContext.SaveChangesAsync();
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
                        transaction.Commit();

                    }
                    return NotFound("Please Enter the Correct DeliveryNo , SalesOrderNo,ItemCode ");
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }
    }
}
