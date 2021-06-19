using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using TSCubemapGenerator;

namespace Tests.TSCubemapGenerator
{
    public class TestNumberMultipleRounder
    {

        [Test]
        public void ctor_ThrowExceptionIfNumberIs0()
        {
            // when, then
            Assert.Throws<CubemapGeneratorException>(() =>
            {
                var target = new NumberMultipleRounder(number: 0);
            });
        }


        [Test]
        public void Round_MultipleOf4()
        {
            // setup
            var target = new NumberMultipleRounder(number: 4);

            // when, then
            Assert.AreEqual(0, target.Round(0));
            Assert.AreEqual(0, target.Round(3));
            Assert.AreEqual(4, target.Round(4));

            Assert.AreEqual(1020, target.Round(1023));
            Assert.AreEqual(1024, target.Round(1024));
            Assert.AreEqual(1024, target.Round(1025));
            Assert.AreEqual(1024, target.Round(1027));
            Assert.AreEqual(1028, target.Round(1028));
        }
    }
}