﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ThePower.Data.Repository.IRepository;
using ThePower.Models;
using ThePowerData.Data;

namespace ThePower.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string paymentStatus = null)
        {
            var order = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if(order != null)
            {
                order.OrderStatus = orderStatus;
                if(paymentStatus != null) 
                    order.PaymentStatus = paymentStatus;
            }
        }
    }
}
