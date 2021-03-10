using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SummarizeTest
{
    public class SummarizeTest
    {
        public List<product> products;
        [SetUp]
        public void Setup()
        {
            products = new List<product>
            {
                new product(){ Cost=10,Price=130},
                new product(){ Cost=21,Price=150},
                new product(){ Cost=32,Price=200},
                new product(){ Cost=43,Price=220},
                new product(){ Cost=54,Price=250},
                new product(){ Cost=65,Price=260},
                new product(){ Cost=76,Price=300},
                new product(){ Cost=87,Price=410},
                new product(){ Cost=98,Price=660},
                new product(){ Cost=109,Price=730},
            };
        }

        [Test]
        public void SummarizeCost()
        {
            var excepted = new List<int> { 63, 162, 261, 109 };
            Assert.AreEqual(excepted, MySummarize.SummarizeByGroup(products, 3, p => p.Cost));
        }

        [Test]
        public void SummarizePrice()
        {
            var excepted = new List<int> { 700, 1220, 1390 };
            Assert.AreEqual(excepted, MySummarize.SummarizeByGroup(products, 4, p => p.Price));
        }

        [Test]
        public void SummarizeQuantity()
        {
            var orders = new List<Order>
            {
                new Order(){ Quantity=1 },
                new Order(){ Quantity=2 },
                new Order(){ Quantity=3 },
                new Order(){ Quantity=4 },
                new Order(){ Quantity=5 },
            };
            var excepted = new List<int> { 3, 7, 5 };
            Assert.AreEqual(excepted, MySummarize.SummarizeByGroup(orders, 2, o=>o.Quantity));
        }

        [Test]
        public void SizeWrong()
        {
            
            var orders = new List<Order>
            {
                new Order(){ Quantity=1 },
                new Order(){ Quantity=2 },
                new Order(){ Quantity=3 },
                new Order(){ Quantity=4 },
                new Order(){ Quantity=5 },
            };

            Action act = () => MySummarize.SummarizeByGroup(orders, -1, o => o.Quantity);
            act.Should().Throw<ArgumentException>().WithMessage("size should >0");
        }
    }


    public class product
    {
        public int Cost { get; set; }
        public int Price { get; set; }

    }

    public class Order
    {
        public int Quantity { get; set; }

    }

    public static class MySummarize
    {
        public static IEnumerable<int> SummarizeByGroup<T>(IEnumerable<T> source, int size, Func<T, int> selector)
        {

            if (size < 1) throw new ArgumentException("size should >0");
            int sourceLength = source.Count();
            int group = source.Count() / size;
            int remainder = source.Count() % size;
            var answer = new List<int>();

            //可分組的計算
            for (int i = 0; i < sourceLength - remainder; i = i + size)
            {
                int result = 0;
                for (int j = i; j < i + size; j++)
                {
                    result += selector(source.ElementAt(j));
                }
                answer.Add(result);
            }

            //不可分組的計算
            int remainderSum = 0;
            for (int j = 1; j <= remainder; j++)
            {
                remainderSum += selector(source.ElementAt(sourceLength - j));
            }

            answer.Add(remainderSum);
            return answer;


        }
    }
}