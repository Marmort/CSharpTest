/*********************************************************
 * Test Protobuf-net v2 with complex types
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace Protobuf_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStream ms = new MemoryStream();

            Person p = new Person()
            {
                id = int.MaxValue,
                tFloat = 13141.023f,
                name = "Ben",
                reallyBigInt = ulong.MaxValue,
                myEnum = SomeValues.five
            };

            p.likedThings.Add("coffee", 100);
            p.likedThings.Add("cookies", 599);
            p.likedThings.Add("veggies", -500);

            p.myLikes.Add(new LikedThing("one", 1));
            p.myLikes.Add(new LikedThing("two", 2));

            p.randomData.AddRange(System.Text.ASCIIEncoding.ASCII.GetBytes("wddrf dgtg456g 00ooi9"));

            // 序列化
            Serializer.Serialize(ms, p);
            byte[] bin = ms.ToArray();
            Console.WriteLine(string.Format("序列化数据的长度： {0}\n\n序列化的字节：{1}\n", bin.Length, BitConverter.ToString(bin)));

            // 反序列化
            Person p2 = Serializer.Deserialize<Person>(new MemoryStream(bin));
            Console.WriteLine(p2);

            Console.ReadKey(true);
        }
    }

    public enum SomeValues
    {
        zero, one, two, tree, four, five
    }

    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public int id = 0;

        [ProtoMember(2)]
        public string name;

        [ProtoMember(3)]
        public float tFloat;

        [ProtoMember(4)]
        public double tDouble;

        [ProtoMember(5)]
        public Dictionary<string, int> likedThings = new Dictionary<string, int>();

        [ProtoMember(6)]
        public List<byte> randomData = new List<byte>();

        [ProtoMember(7)]
        public List<LikedThing> myLikes = new List<LikedThing>();

        [ProtoMember(8)]
        public ulong reallyBigInt;

        [ProtoMember(9)]
        public SomeValues myEnum;

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(string.Format("p.id=\t\t{0}\np.name=\t\t{1}\n", this.id, this.name));
            sb.Append(string.Format("ulog Size: \t{0}\n", reallyBigInt));
            foreach (KeyValuePair<string, int> kvp in likedThings)
            {
                sb.Append(string.Format(" - likedThings: {0} x {1}\n", kvp.Key, kvp.Value));
            }

            sb.Append("字节数组：\t" + System.Text.ASCIIEncoding.ASCII.GetString(randomData.ToArray()) + "\n");

            sb.Append("myLikes\n");
            foreach (LikedThing lt in myLikes)
            {
                sb.Append(string.Format(" - like2: \t{0} => {1}\n", lt.name, lt.vote));
            }

            sb.Append("枚举类型从 zero 到 " + myEnum.ToString());

            return sb.ToString();
        }
    }

    [ProtoContract]
    public class LikedThing
    {
        [ProtoMember(1)]
        public string name;

        [ProtoMember(2)]
        public int vote;

        public LikedThing()
        {
        }

        public LikedThing(string name, int vote)
        {
            this.name = name;
            this.vote = vote;
        }
    }

}
