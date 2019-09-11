using System;
using System.Collections.Generic;
using System.Linq;

namespace KrakInvoices.Domain
{
    public class Tenement
    {
        int NWD(int a, int b)
        {
            int pom;
            while (b != 0)
            {
                pom = b;
                b = a % b;
                a = pom;
            }
            return a;
        }
        private (int n, int d) Reduce(int numerator, int denominator)
        {
            var divisor = NWD(numerator, denominator);
            return (numerator / divisor, denominator / divisor);
        }

        public Tenant AddTenant()
        {
            var tenant = new Tenant();
            Tenants.Add(tenant);
            return tenant;
        }

        public List<Owner> Owners { get; } = new List<Owner>();
        public List<Tenant> Tenants { get; } = new List<Tenant>();
        public Owner Split(Owner owner, int nom, int den)
        {
            int n = owner.Share.Numerator,
                d = owner.Share.Denominator;

            var n1 = n * nom;
            var d1 = d * den;

            var n2 = n * d1 - n1 * d;
            var d2 = d1 * d;

            (n1, d1) = Reduce(n1, d1);
            (n2, d2) = Reduce(n2, d2);
            owner.Share.Numerator = n2;
            owner.Share.Denominator = d2;

            var newOwner = new Owner
            {
                Share = new Share
                {
                    Numerator = n1,
                    Denominator = d1,
                }
            };
            Owners.Add(newOwner);
            if (n2 == 0)
            {
                Owners.Remove(owner);
            }
            return newOwner;
        }
        public bool IsOwnerShareValid()
        {
            var num = 0;
            var den = 1;
            foreach (var item in Owners)
            {
                var n = item.Share.Numerator;
                var d = item.Share.Denominator;
                n *= den;
                den *= d;
                num *= d;
                num += n;
            }
            return num == den;
        }

        public IEnumerable<InvoiceData> GetInvoiceData(int year, int month)
        {
            foreach (var tenant in Tenants)
            {
                foreach (var owner in Owners)
                {
                    yield return new InvoiceData
                    {
                        From = owner.Name,
                        To = tenant.Name,
                        InvoiceItems = tenant.Costs.Select(o => new InvoiceItem
                        {
                            Name = o.Name,
                            Amount = o.Amount * owner.Share.Numerator / owner.Share.Denominator

                        }).ToArray()
                    };
                }
            }
        }

        public Owner AddOwner(int numerator, int denominator)
        {
            var owner = new Owner()
            {
                Share = new Share
                {
                    Numerator = numerator,
                    Denominator = denominator
                }
            };
            return owner;
        }
    }
    public static class DomainExtensions
    {
        public static Owner WithName(this Owner owner, string name)
        {
            owner.Name = name;
            return owner;
        }

        public static Tenant WithName(this Tenant tenant, string name)
        {
            tenant.Name = name;
            return tenant;
        }
    }
    public class InvoiceData
    {
        public InvoiceItem[] InvoiceItems { get; set; }
        public string From { get; internal set; }
        public string To { get; internal set; }
    }
    public class InvoiceItem
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
    }
}
