using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AzureStorage.Controllers
{
    [ApiController]
    [Route("/[Controller]/[action]")]
    public class TableController : Controller
    {
        private readonly TableClient table;
        public TableController(TableClient tableClient)
        {
            table = tableClient;
        }
        [HttpGet]
        public IActionResult GetRow(string Id)
        {
            try
            {
                Pageable<TableEntity> entities = table.Query<TableEntity>();
                //entities.Select(e => MapTableToWeatherDataModel(e));
            }
            catch {
                return BadRequest();
            }

            return Ok();
        }

        public IActionResult UpsertRow([FromBody] Employee employee)
        {
            try
            {
                TableEntity entity = new TableEntity();
                entity.PartitionKey = employee.LastName;
                entity.RowKey = $"{employee.LastName}{employee.UID}";

                entity["EmailId"] = employee.EmailID;

                table.UpsertEntity(entity);
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        public IActionResult DeleteRow(string partitionKey, string rowKey)
        {
            try
            {
                table.DeleteEntity(partitionKey, rowKey);
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        public IActionResult UpdateRow(Employee employee)
        {
            var PartitionKey = employee.LastName;
            var RowKey = $"{employee.LastName}{employee.UID}";
            try
            {
                var entity = table.GetEntity<TableEntity>(PartitionKey, RowKey).Value;

                foreach (string propertyName in employee.PropertyNames)
                {
                    entity[propertyName] = employee[propertyName];
                }

                table.UpdateEntity(entity, new ETag());
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
