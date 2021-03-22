using AB.Domain.Models;
using System;
using Xunit;

namespace AB.Domain.Tests
{
    public class HistogramColumnTests
    {
        [Fact]
        public void Ctor_WithGoodValues_ReturnsGoodColumn()
        {
            HistogramColumn histogramColumn = new(10, 5, DateTime.Today);

            Assert.Equal(DateTime.Today, histogramColumn.Day);
            Assert.Equal(200d, histogramColumn.Value);
        }

        [Fact]
        public void Ctor_WithZeroReturned_ReturnsZeroColumn()
        {
            HistogramColumn histogramColumn = new(0, 5, DateTime.Today);

            Assert.Equal(DateTime.Today, histogramColumn.Day);
            Assert.Equal(0, histogramColumn.Value);
        }

        [Fact]
        public void Ctor_WithZeroInstalled_ReturnsZeroColumn()
        {
            HistogramColumn histogramColumn = new(5, 0, DateTime.Today);

            Assert.Equal(DateTime.Today, histogramColumn.Day);
            Assert.Equal(0, histogramColumn.Value);
        }
    }
}
