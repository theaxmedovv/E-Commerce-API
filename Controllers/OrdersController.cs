using AtirAPI.Models;
using ECommerceAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AtirUz.DTOs;


using ECommerceAPI.DTOs;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public OrdersController(ECommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a paginated list of orders with customer and product details.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest("Invalid page or pageSize.");

            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific order by ID with related customer and product information.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return order;
        }

        /// <summary>
        /// Creates a new order and updates product stock accordingly.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (orderDto.OrderItems == null || !orderDto.OrderItems.Any())
                return BadRequest("At least one order item is required.");

            var customer = await _context.Customers.FindAsync(orderDto.CustomerId);
            if (customer == null)
                return BadRequest("Customer not found.");

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var itemDto in orderDto.OrderItems)
                {
                    if (itemDto.Quantity <= 0)
                        return BadRequest($"Invalid quantity for product ID {itemDto.ProductId}");

                    var product = await _context.Products.FindAsync(itemDto.ProductId);
                    if (product == null || product.Stock < itemDto.Quantity)
                        return BadRequest($"Insufficient stock for product ID {itemDto.ProductId}");

                    var orderItem = new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        Price = product.Price * itemDto.Quantity
                    };
                    order.OrderItems.Add(orderItem);

                    totalAmount += orderItem.Price;
                    product.Stock -= itemDto.Quantity;
                }

                order.TotalAmount = totalAmount;
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Load related data for response
                var createdOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == order.Id);

                Log.Information("Order created for CustomerId {CustomerId} with TotalAmount {TotalAmount}", order.CustomerId, order.TotalAmount);

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error(ex, "Failed to create order for CustomerId {CustomerId}", orderDto.CustomerId);
                throw;
            }
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, [FromBody] Order order)
        {
            if (id != order.Id)
                return BadRequest("Order ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existingOrder == null)
                return NotFound();

            // Update logic (simplified; add stock validation if needed)
            _context.Entry(existingOrder).CurrentValues.SetValues(order);

            try
            {
                await _context.SaveChangesAsync();
                Log.Information("Order {OrderId} updated", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an order and restores product stock.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                        product.Stock += item.Quantity;
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                Log.Information("Order {OrderId} deleted", id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Log.Error(ex, "Failed to delete order {OrderId}", id);
                throw;
            }

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(o => o.Id == id);
        }
    }

    
}