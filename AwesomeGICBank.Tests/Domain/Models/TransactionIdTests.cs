using AwesomeGICBank.Domain.Exceptions;
using AwesomeGICBank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGICBank.Tests.Domain.Models
{
    public class TransactionIdTests
    {
        [Fact]
        public void Generate_ValidParameters_ShouldCreateTransactionId()
        {
            var date = new DateTime(2024, 1, 1);
            var sequence = 1;

            var transactionId = TransactionId.Generate(date, sequence);

            Assert.Equal("20240101-01", transactionId.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Generate_InvalidSequence_ShouldThrowException(int sequence)
        {
            var date = DateTime.Today;

            Assert.Throws<InvalidTransactionIdException>(() =>
                TransactionId.Generate(date, sequence));
        }

        [Fact]
        public void Equals_SameValue_ShouldReturnTrue()
        {
            var date = DateTime.Today;
            var id1 = TransactionId.Generate(date, 1);
            var id2 = TransactionId.Generate(date, 1);

            Assert.Equal(id1, id2);
            Assert.True(id1.Equals(id2));
        }

        [Fact]
        public void GetHashCode_SameValue_ShouldReturnSameHashCode()
        {
            var date = DateTime.Today;
            var id1 = TransactionId.Generate(date, 1);
            var id2 = TransactionId.Generate(date, 1);

            Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
        }
    }
}
