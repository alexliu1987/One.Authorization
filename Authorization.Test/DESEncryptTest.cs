using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSO.Common.SSO;

namespace Authorization.Test
{
    [TestClass]
    public class DESEncryptTest
    {
        [TestMethod]
        public void TestEncrypt()
        {
            Assert.AreEqual(DESEncrypt.Encrypt("admin", "admin"), "145FC7668BA6BF2E");
            Assert.AreEqual(DESEncrypt.Encrypt("123", "xf1"), "1A80D301A5A9A434");
            Assert.AreEqual(DESEncrypt.Encrypt("123", "jc1"), "FF1102CFDD487E25");
            Assert.AreEqual(DESEncrypt.Encrypt("123", "test"), "FA5AD01F88BA8687");
        }
    }
}
