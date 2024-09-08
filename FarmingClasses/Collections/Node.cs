using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace FarmingClasses.Collections;

public class Node<T> {
    public Node<T>? Next { get; set; }
    public T Current { get; set; }

    public Node(T current, Node<T>? next = null) {
        Current = current;
        Next = next;
    }
}
