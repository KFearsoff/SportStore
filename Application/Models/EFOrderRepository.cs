using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private StoreDbContext _context;
        public EFOrderRepository(StoreDbContext context) => _context = context;

        //Ensures that reading an Order from the database also pulls associated Lines and their associated Products
        public IQueryable<Order> Orders => _context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            //Ensures that the Products accosiated with Order shouldn't be stored in the database unless they are modified
            _context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0) _context.Orders.Add(order);
            _context.SaveChanges();
        }
    }
}
