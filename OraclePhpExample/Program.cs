using System;
using MassTransit;

namespace OraclePhpExample
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
	        PrintHugeNumbersTwoStructure(36617110);
	        PrintHugeNumbersTwoStructure(6916729);

	        PrintComparingTwoStructure();

	        /*
	        for (uint i = 0; i < uint.MaxValue; i++)
	        {
		        PrintHugeNumbersTwoStructure(1234567890ul, i);
	        }
	        */
	        
	        //PrintHugeNumbersTwoStructure(1234567890L);
	        //PrintHugeNumbersTwoStructure(9223372036854775807L);
	        //PrintHugeNumbersTwoStructure(18446744073709551615L);
	        
            Console.ReadKey();
        }

        private static void PrintComparingTwoStructure()
        {
	        for (ulong idx = 0; idx <= 20; idx++)
	        {
		        var someGuid1 = MoreNewerGuidGenerator.ConvertToGuid(idx, 123456789);
		        var someGuid2 = MoreNewerGuidGenerator.ConvertToGuid(idx, 987654321);

		        var someId1 = MoreNewerGuidGenerator.ConvertToULong(someGuid1);
		        var someId2 = MoreNewerGuidGenerator.ConvertToULong(someGuid2);

		        Console.WriteLine($"{someId1.id}:{someId1.entityType}");
		        Console.WriteLine($"{someId2.id}:{someId2.entityType}");
		        Console.WriteLine($"{someGuid1} === {someGuid2}");    
	        }
        }
        
        private static void PrintHugeNumbersTwoStructure(ulong number, uint entityType = 0)
        {
	        var someGuid1 = MoreNewerGuidGenerator.ConvertToGuid(number, entityType);
		 	var someGuid2 = MoreNewerGuidGenerator.ConvertToGuid(number, entityType);

		 	Console.WriteLine($"{someGuid1} === {someGuid2}");
        }
    }

    public static class MoreNewerGuidGenerator
    {
	    /// <summary>
	    /// Перевести целочисленный 8-байтный идентификатор в GUID
	    /// </summary>
	    /// <param name="id">Целочисленный 8-байтный идентификатор</param>
	    /// <param name="entityType">Тип сущности</param>
	    /// <returns>GUID (version 1)</returns>
	    public static Guid ConvertToGuid(ulong id, uint entityType = 0)
        {
	        var wid1 = (byte) (entityType >> 24);
	        var wid2 = (byte) (entityType >> 16);
	        var wid3 = (byte) (entityType >> 8);
	        var wid4 = (byte) (entityType >> 0);
	        
	        // идентификатор рабочей станции (MAC-address)
	        var workerId = new byte[]{ wid1, wid2, wid3, wid4, 0, 0, 0, 0 };

	        var a = (int)(id >> 32);
            var b = (int)(id & 0xFFFFFFFF);
            var c = (workerId[0] << 24) | (workerId[1] << 16) | (workerId[2] << 8) | workerId[3];
            var d = (workerId[4] << 24) | (workerId[5] << 16);

            var ga = (int)(0);
            var gb = (short)(c);
            var gc = (short)(c >> 16);

            var gd = (byte)(b >> 8);
            var ge = (byte)(b);
            var gf = (byte)(a >> 24);
            var gg = (byte)(a >> 16);
            var gh = (byte)(a >> 8);
            var gi = (byte)(a);
            var gj = (byte)(b >> 24);
            var gk = (byte)(b >> 16);

            return new Guid(ga, gb, gc, gd, ge, gf, gg, gh, gi, gj, gk);
        }
        
	    /// <summary>
	    /// Перевести целочисленный 8-байтный идентификатор в GUID с помощью пакета MassTransit
	    /// </summary>
	    /// <param name="id">Целочисленный 8-байтный идентификатор</param>
	    /// <returns>GUID (version 1)</returns>
        public static Guid ConvertToGuid2(ulong id, uint entityType = 0)
        {
	        var wid1 = (byte) (entityType >> 24);
	        var wid2 = (byte) (entityType >> 16);
	        var wid3 = (byte) (entityType >> 8);
	        var wid4 = (byte) (entityType >> 0);
	        
	        // идентификатор рабочей станции (MAC-address)
	        var workerId = new byte[]{ wid1, wid2, wid3, wid4, 0, 0, 0, 0 };
	        
	        var a = (int)(id >> 32);
	        var b = (int)(id & 0xFFFFFFFF);
	        var c = (workerId[0] << 24) | (workerId[1] << 16) | (workerId[2] << 8) | workerId[3];
	        var d = 0;

	        var ga = (int)(d | 0);
	        var gb = (short)(c);
	        var gc = (short)(c >> 16);

	        var bd = (byte)(b >> 8);
	        var be = (byte)(b);
	        var bf = (byte)(a >> 24);
	        var bg = (byte)(a >> 16);
	        var bh = (byte)(a >> 8);
	        var bi = (byte)(a);
	        var bj = (byte)(b >> 24);
	        var bk = (byte)(b >> 16);

	        return new NewId(ga, gb, gc, bd, be, bf, bg, bh, bi, bj, bk).ToGuid();
        }

	    /// <summary>
	    /// Перевести GUID в целочисленный 8-байтный идентификатор
	    /// </summary>
	    /// <param name="guid">Глобальный уникальный идентификатор</param>
	    /// <returns>Целочисленный 8-байтный идентификатор</returns>
        public static (ulong id, int entityType) ConvertToULong(Guid guid)
        {
	        var guidByteArray = guid.ToByteArray();
	        
	        var c = (guidByteArray[7] << 24) | (guidByteArray[6] << 16) | (guidByteArray[5] << 8) | guidByteArray[4];
	        
	        var d = guidByteArray[8];
	        var e = guidByteArray[9];
	        var f = guidByteArray[10];
	        var g = guidByteArray[11];
	        var h = guidByteArray[12];
	        var i = guidByteArray[13];
	        var j = guidByteArray[14];
	        var k = guidByteArray[15];

	        var longByteArray =new byte[8]
	        {
		       f, g, h, i, j, k, d, e
	        };

	        return (ToULong(longByteArray), c);
        }
        
	    /// <summary>
	    /// Перевести массив байт длинной 8 в целочисленное 8-байтовое число
	    /// </summary>
	    /// <param name="data">Массив байт длинны 8</param>
	    /// <returns>Целочисленное 8-байтовое число</returns>
	    /// <exception cref="ArgumentException">Массив байт должен быть длинной 8</exception>
        private static ulong ToULong(byte[] data)
        {
	        const int requiredSize = 8;

	        if (data.Length != requiredSize)
	        {
		        throw new ArgumentException($"The byte-array \"{nameof(data)}\" must be exactly {requiredSize} bytes.");
	        }

	        var result = 0ul;

	        for (var i = 0; i < requiredSize; i++)
	        {
		        result |= ((ulong)data[i] << ((requiredSize - (i + 1)) * 8));
	        }

	        return result;
        }
    }
}