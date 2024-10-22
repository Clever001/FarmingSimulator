using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FarmingClasses.Serialization;
public class KVP<T1, T2> {
    [XmlElement("Key")]
    public T1? Key { get; set; } = default;
    [XmlElement("Value")]
    public T2? Value { get; set; } = default;
    public KVP() { }

    public KVP(T1 key, T2 value) {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        Key = key;
        Value = value;
    }
}
