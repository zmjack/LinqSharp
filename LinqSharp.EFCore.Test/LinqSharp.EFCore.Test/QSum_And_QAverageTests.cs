using NStandard.UnitValues;
using System;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class QSum_And_QAverageTests
    {
        [Fact]
        public void ValueTest()
        {
            var values = new StorageValue[] { new StorageValue(10), new StorageValue(20), new StorageValue(45) };
            Assert.Equal(75, values.QSum().BitValue);
            Assert.Equal(25, values.QAverage().BitValue);
            Assert.Equal(25, values.QAverageOrDefault().BitValue);
            Assert.Equal(25, values.QAverageOrDefault(new StorageValue(-1)).BitValue);
        }

        [Fact]
        public void NullableValueTest()
        {
            var values = new StorageValue?[] { new StorageValue(10), new StorageValue(20), new StorageValue(45) };
            Assert.Equal(75, values.QSum().Value.BitValue);
            Assert.Equal(25, values.QAverage().Value.BitValue);
            Assert.Equal(25, values.QAverageOrDefault().Value.BitValue);
            Assert.Equal(25, values.QAverageOrDefault(new StorageValue(-1)).Value.BitValue);
        }

        [Fact]
        public void EmptyValueTest()
        {
            var values = new StorageValue[] { };
            Assert.Equal(0, values.QSum().BitValue);
            Assert.ThrowsAny<InvalidOperationException>(() => values.QAverage());
            Assert.Equal(default, values.QAverageOrDefault());
            Assert.Equal(-1, values.QAverageOrDefault(new StorageValue(-1)).BitValue);
        }

        [Fact]
        public void EmptyNullableValueTest()
        {
            var values = new StorageValue?[] { };
            Assert.Equal(0, values.QSum().Value.BitValue);
            Assert.Null(values.QAverage());
            Assert.Null(values.QAverageOrDefault());
            Assert.Equal(-1, values.QAverageOrDefault(new StorageValue(-1)).Value.BitValue);
        }

    }
}
