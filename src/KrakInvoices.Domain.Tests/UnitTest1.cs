using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace KrakInvoices.Domain.Tests
{
    public class TenementTests
    {
        [Fact]
        public void Cost()
        {
            var tenement = new Tenement();
            tenement.AddOwner(1, 33).WithName("A");
            tenement.AddOwner(32, 33).WithName("B");

            tenement.AddTenant().WithName("X");
            tenement.AddTenant().WithName("Y");
            tenement.AddTenant().WithName("Z");

            tenement.Tenants[0].AddCost(2019, 9, 1000.55m, "woda");
            tenement.Tenants[0].AddCost(2019, 9, 10000, "umowa");

            tenement.Tenants[1].AddCost(2019, 9, 2000, "woda");
            tenement.Tenants[1].AddCost(2019, 9, 20000, "umowa");

            tenement.Tenants[2].AddCost(2019, 9, 3000, "woda");
            tenement.Tenants[2].AddCost(2019, 9, 30000, "umowa");

            var data = tenement.GetInvoiceData(2019, 9);

        }
        [Fact]
        public void Split()
        {
            var tenement = new Tenement();
            tenement.AddOwner(1, 2).WithName("A");
            tenement.AddOwner(1, 2).WithName("B");
            var owner = tenement
                .Split(tenement.Owners[0], 1, 3)
                .WithName("C");
            
            owner.Share.Numerator.Should().Be(1);
            owner.Share.Denominator.Should().Be(6);
            tenement.Owners[0].Share.Numerator.Should().Be(1);
            tenement.Owners[0].Share.Denominator.Should().Be(3);
            tenement.IsOwnerShareValid().Should().BeTrue();
        }

        [Fact]
        public void IsOwnerShareValid()
        {
            var tenement = new Tenement();
            tenement.AddOwner(1, 2).WithName("A");
            tenement.AddOwner(1, 4).WithName("B");
            tenement.AddOwner(1, 4).WithName("C");

            tenement.IsOwnerShareValid().Should().BeTrue();
        }
    }
}
