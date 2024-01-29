using ShoppingWebApi.EfCore;
using System.Security.Cryptography.X509Certificates;

namespace ShoppingWebApi.Model
{
    public class DbHelper
    {
        private EF_DataContext _context;
        public DbHelper(EF_DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <returns></returns>
        public List<ProductModel> GetProducts()
        {
            List<ProductModel> response = new List<ProductModel>();
            var dataList = _context.Products.ToList();
            dataList.ForEach(row => response.Add(new ProductModel()
            {
                brand = row.brand,
                id = row.Id,
                name = row.name,
                price = row.price,
                size = row.size
            }));
            return response;
        }

        public ProductModel GetProductById(int id)
        {
            ProductModel response = new ProductModel();
            var dataList = _context.Products.ToList();
            var row = _context.Products.Where(d => d.Id.Equals(id)).FirstOrDefault();
            return new ProductModel()
            {
                brand = row.brand,
                id = row.Id,
                name = row.name,
                price = row.price,
                size = row.size
            };
        }

        /// <summary>
        /// IT SERVES POST/PUT/PATCH
        /// </summary>
        /// <param name="orderModel"></param>
        public void SaveOrder(OrderModel orderModel) 
        {
            Order dbTable = new Order();
            if(orderModel.Id > 0)
            {
                //PUT
                dbTable = _context.Orders.Where(d=>d.id.Equals(orderModel.Id)).FirstOrDefault();
                if(dbTable != null)
                {
                    dbTable.phone=orderModel.phone;
                    dbTable.address = orderModel.address;
                }
            }
            else
            {
                //POST
                dbTable.phone = orderModel.phone;
                dbTable.address = orderModel.address;
                dbTable.name = orderModel.name;
                dbTable.Product = _context.Products.Where(f => f.Id.Equals(orderModel.product_id)).FirstOrDefault();
                _context.Orders.Add(dbTable);
            }
            _context.SaveChanges();
        }


        /// <summary>
        /// DELETE
        /// </summary>
        /// <param name="id"></param>
        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Where(d => d.id.Equals(id)).FirstOrDefault();
            if(order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}
