using Math.AF;
using BlowFish.AF.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlowFish.AF
{
    // Link: https://www.geeksforgeeks.org/blowfish-algorithm-with-examples/
    public class BlowFish
    {
        public uint[][] Substitution { get; private set; } = new uint[4][];
        public uint[] Keys { get; private set; }

        public void GenerateKeys(params uint[] key)
        {
            if (key.Length < 1 || key.Length > 14)
                throw new ArgumentException($"Keylength not correct {key.Length}");

            Keys = Pi.HexPiToUints(0, 18).ToArray();
            if (Keys == null || Keys.Length != 18)
                return;

            for (int i = 0; i < Keys.Length; i++)
                Keys[i] ^= key[i % key.Length];
        }

        public void GenerateSubcstitutionBoxes()
        {
            if (Substitution[0] == null)
                Substitution[0] = Pi.HexPiToUints(18, 256).ToArray();
            if (Substitution[1] == null)
                Substitution[1] = Pi.HexPiToUints(274, 256).ToArray();
            if (Substitution[2] == null)
                Substitution[2] = Pi.HexPiToUints(530, 256).ToArray();
            if (Substitution[3] == null)
                Substitution[3] = Pi.HexPiToUints(786, 256).ToArray();
        }

        public ulong Encrypt(ulong plaintext)
        {
            for (int r = 0; r < 16; r++)
                plaintext = Round(r, plaintext);

            uint xr = (uint)plaintext;
            uint xl = (uint)plaintext.ShiftRight(32);

            xr = xr.Xor(Keys[17]);
            xl = xl.Xor(Keys[16]);

            return xr.ShiftLeftToUlong(32).Or(xl);
        }

        public ulong Decrypt(ulong ciphertext)
        {
            for (int r = 17; r > 1; r--)
                ciphertext = Round(r, ciphertext);

            uint xr = (uint)ciphertext;
            uint xl = (uint)ciphertext.ShiftRight(32);

            xr = xr.Xor(Keys[0]);
            xl = xl.Xor(Keys[1]);

            return xr.ShiftLeftToUlong(32).Or(xl);
        }

        private ulong Round(int round, ulong plaintext)
        {
            uint xr = (uint)plaintext;
            uint xl = (uint)plaintext.ShiftRight(32);

            uint fInput = Keys[round].Xor(xl);
            uint output = Substitute(fInput).Xor(xr);

            return output.ShiftLeftToUlong(32).Or(fInput);
        }

        private uint Substitute(uint input)
            => Substitution[0][input >> 24 & 0xFF]
                .Add(Substitution[1][input >> 16 & 0xFF])
                .Xor(Substitution[2][input >> 8 & 0xFF])
                .Add(Substitution[3][input & 0xFF]);
    }
}
