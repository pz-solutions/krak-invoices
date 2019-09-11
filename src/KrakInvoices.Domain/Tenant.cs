using System;
using System.Collections.Generic;

namespace KrakInvoices.Domain
{
    public class Tenant
    {
        public string Name { get; set; }

        public List<Cost> Costs { get; } = new List<Cost>();

        public void AddCost(int year, int month, decimal amount, string name)
        {
            Costs.Add(new Cost
            {
                Name = name,
                Amount = amount,
                Month = new MonthValue
                {
                    Month = month,
                    Year = year
                }
            });
        }
    }

}
