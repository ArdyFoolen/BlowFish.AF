using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlowFish.AF.Tests
{
    public class BlowfishTests
    {
        private static BlowFish blowFish = new BlowFish();

        private readonly uint[] PiKeyDigits = new uint[]
        {
            0x243f6a88,
            0x85a308d3,
            0x13198a2e,
            0x03707344,
            0xa4093822,
            0x299f31d0,
            0x082efa98,
            0xec4e6c89,
            0x452821e6,
            0x38d01377,
            0xbe5466cf,
            0x34e90c6c,
            0xc0ac29b7,
            0xc97c50dd,
            0x3f84d5b5,
            0xb5470917,
            0x9216d5d9,
            0x8979fb1b
        };

        private uint[] TestKeys(uint key)
            => PiKeyDigits.Select(s => s ^ key).ToArray();

        private uint[] TestKeys(uint[] key)
            => PiKeyDigits.Select((s, i) => s ^ key[i % 2]).ToArray();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            blowFish.GenerateSubcstitutionBoxes();
        }

        [Test]
        public void GenerateKeys_32BitKey_Succeeds()
        {
            // Act
            blowFish.GenerateKeys(0x12345678);

            // Arrange
            Assert.That(blowFish.Keys, Is.EquivalentTo(TestKeys(0x12345678)));
        }

        [Test]
        public void GenerateKeys_64BitKey_Succeeds()
        {
            // Act
            blowFish.GenerateKeys(0x12345678, 0x9abcdef0);

            // Arrange
            Assert.That(blowFish.Keys, Is.EquivalentTo(TestKeys(new uint[] { 0x12345678, 0x9abcdef0 })));
        }

        [Test]
        public void GenerateSubcstitutionBoxes_Succeeds()
        {
            // Arrange
            Assert.That(blowFish.Substitution[0][0], Is.EqualTo(0xd1310ba6));
            Assert.That(blowFish.Substitution[0][255], Is.EqualTo(0x6e85076a));

            Assert.That(blowFish.Substitution[1][0], Is.EqualTo(0x4b7a70e9));
            Assert.That(blowFish.Substitution[1][255], Is.EqualTo(0xdb83adf7));

            Assert.That(blowFish.Substitution[2][0], Is.EqualTo(0xe93d5a68));
            Assert.That(blowFish.Substitution[2][255], Is.EqualTo(0x406000e0));

            Assert.That(blowFish.Substitution[3][0], Is.EqualTo(0x3a39ce37));
            Assert.That(blowFish.Substitution[3][255], Is.EqualTo(0x3ac372e6));
        }

        [Test]
        public void Encrypt_Succeeds()
        {
            blowFish.GenerateKeys(0xaabb0918, 0x2736ccdd);

            ulong plainulong = 0x123456abcd132536;

            // Act
            var result = blowFish.Encrypt(plainulong);

            // Assert
            Assert.That(result, Is.EqualTo(0xd748ec383d3405f7));
        }

        [Test]
        public void Decrypt_Succeeds()
        {
            blowFish.GenerateKeys(0xaabb0918, 0x2736ccdd);

            ulong cipherulong = 0xd748ec383d3405f7;

            // Act
            var result = blowFish.Decrypt(cipherulong);

            // Assert
            Assert.That(result, Is.EqualTo(0x123456abcd132536));
        }
    }
}
