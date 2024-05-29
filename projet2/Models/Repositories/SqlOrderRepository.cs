using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace projet2.Models.Repositories
{
    public class SqlOrderRepository : IOrderRepository<Order>
    {
        private readonly AppDbContext _context;

        public SqlOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public Order Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        public Order Delete(int orderId)
        {
            Order orderToDelete = _context.Orders.Find(orderId);
            if (orderToDelete != null)
            {
                _context.Orders.Remove(orderToDelete);
                _context.SaveChanges();
            }
            return orderToDelete;
        }

        public Order Get(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.ToList();
        }

        public Order Update(Order orderChanges)
        {
            var order = _context.Orders.Attach(orderChanges);
            order.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return orderChanges;
        }

        // Marquer une commande comme livrée
        public Order MarkAsDelivered(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.IsDelivered = true;
                _context.SaveChanges();
            }
            return order;
        }

        // Supprimer une commande livrée
        public Order DeleteDeliveredOrders()
        {
            var deliveredOrders = _context.Orders.Where(o => o.IsDelivered).ToList();
            foreach (var order in deliveredOrders)
            {
                _context.Orders.Remove(order);
            }
            _context.SaveChanges();
            return null;
        }
        public IEnumerable<Produit> GetOrderProducts(int orderId)
        {
            // Assurez-vous que cette méthode récupère les produits pour une commande donnée
            var order = _context.Orders.Include(o => o.Products)
                                       .FirstOrDefault(o => o.OrderId == orderId);
            return order?.Products ?? new List<Produit>();
        }

    }
}
