using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WooliesX.Models;

namespace WooliesX.Tests
{

    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
                
        }

        [TestMethod]
        public void TestMethod1()
        {
            var trolley = new Trolley
            {
                Products = new List<BaseProduct>
                {
                    new BaseProduct
                    {
                        Name = "1",
                        Price = 5.0m
                    },
                    new BaseProduct
                    {
                        Name = "2",
                        Price = 2.0m
                    }
                },
                Specials = new[]
                {
                    new Special
                    {
                        Quantities = new List<ProductQuantity>
                        {
                            new ProductQuantity
                            {
                                Name = "1", Quantity = 3
                            }, new ProductQuantity
                            {
                                Name = "2", Quantity = 0
                            }
                        }, Total = 5m
                    },
                    new Special
                    {
                        Quantities = new List<ProductQuantity>
                        {
                            new ProductQuantity
                            {
                                Name = "1", Quantity = 1
                            }, new ProductQuantity
                            {
                                Name = "2", Quantity = 2
                            }
                        }, Total = 10m
                    }
                },
                Quantities = new ProductQuantity[]
                {
                    new ProductQuantity
                    {
                        Name = "1", Quantity = 3
                    }, new ProductQuantity{ Name = "2", Quantity = 2}
                }

            };
        }
    }
}
