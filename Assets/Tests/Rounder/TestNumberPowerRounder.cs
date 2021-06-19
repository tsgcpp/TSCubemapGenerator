using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using TSCubemapGenerator;

namespace Tests.TSCubemapGenerator
{
    public class TestNumberPowerRounder
    {

        [Test]
        public void ctor_ThrowExceptionIfNumberIs0()
        {
            // when, then
            Assert.Throws<CubemapGeneratorException>(() =>
            {
                var target = new NumberPowerRounder(number: 0);
            });
        }


        [Test]
        public void Round_PowerOf2()
        {
            // setup
            var target = new NumberPowerRounder(number: 2);

            // when, then
            Assert.AreEqual(2, target.Round(0));
            Assert.AreEqual(2, target.Round(1));
            Assert.AreEqual(2, target.Round(2));
            Assert.AreEqual(4, target.Round(3));
            Assert.AreEqual(4, target.Round(4));

            Assert.AreEqual(1024, target.Round(1023));
            Assert.AreEqual(1024, target.Round(1024));
            Assert.AreEqual(1024, target.Round(1025));

            Assert.AreEqual(2048, target.Round(2047));
            Assert.AreEqual(2048, target.Round(2048));
            Assert.AreEqual(2048, target.Round(2049));

            Assert.AreEqual(8192, target.Round(8191));
            Assert.AreEqual(8192, target.Round(8192));
            Assert.AreEqual(8192, target.Round(8193));
        }
    }
}