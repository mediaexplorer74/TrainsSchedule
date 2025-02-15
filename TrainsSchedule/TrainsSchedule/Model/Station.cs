using Java.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TrainsSchedule.Model
{
    public class Station
    {
        // [null,[["c213","Москва","г. Москва, Москва и Московская область, Россия"],
        // ["s2000006","Москва (Белорусский вокзал)","вкз. Москва (Белорусский вокзал), Москва"],
        // ["s2000003","Москва (Казанский вокзал)","вкз. Москва (Казанский вокзал), Москва"],
        // ["s2000007","Москва (Киевский вокзал)","вкз. Москва (Киевский вокзал), Москва"],
        // ["s2000001","Москва (Курский вокзал)","вкз. Москва (Курский вокзал), Москва"],
        // ["s2006004","Москва (Ленинградский вокзал)","вкз. Москва (Ленинградский вокзал), Москва"],
        // ["s2000005","Москва (Павелецкий вокзал)","вкз. Москва (Павелецкий вокзал), Москва"],
        // ["s2000008","Москва (Рижский вокзал)","вкз. Москва (Рижский вокзал), Москва"],
        // ["s2000009","Москва (Савёловский вокзал)","вкз. Москва (Савёловский вокзал), Москва"],
        // ["s2000002","Москва (Ярославский вокзал)","вкз. Москва (Ярославский вокзал), Москва"],
        // ["s9601018","Москва-3","пл. Москва-3, Москва"]]]
        //
        // ["c213","Москва","г. Москва, Москва и Московская область, Россия"],
        // ["s2000006","Москва (Белорусский вокзал)","вкз. Москва (Белорусский вокзал), Москва"],
        // ["s2000003","Москва (Казанский вокзал)","вкз. Москва (Казанский вокзал), Москва"],
        // ["s2000007","Москва (Киевский вокзал)","вкз. Москва (Киевский вокзал), Москва"],
        // ["s2000001","Москва (Курский вокзал)","вкз. Москва (Курский вокзал), Москва"],
        // ["s2006004","Москва (Ленинградский вокзал)","вкз. Москва (Ленинградский вокзал), Москва"],
        // ["s2000005","Москва (Павелецкий вокзал)","вкз. Москва (Павелецкий вокзал), Москва"],
        // ["s2000008","Москва (Рижский вокзал)","вкз. Москва (Рижский вокзал), Москва"],
        // ["s2000009","Москва (Савёловский вокзал)","вкз. Москва (Савёловский вокзал), Москва"],
        // ["s2000002","Москва (Ярославский вокзал)","вкз. Москва (Ярославский вокзал), Москва"],
        // ["s9601018","Москва-3","пл. Москва-3, Москва"]
        public string code;
        public string name;
        public string descr;
        public Station(string code, string name, string descr)
        {
            this.code = code;
            this.name = name;
            this.descr = descr;
        }

        public Station(string code, string name) : this(code, name, name)
        {
        }

        public class List : List<Station>
        {
            public List(int capacity) : base(capacity)
            {
            }

            public List()
            {
            }
        }

        public virtual void Clear()
        {
            code = "";
            name = "";
            descr = "";
        }
    }
}