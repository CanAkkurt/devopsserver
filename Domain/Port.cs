using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain;

public class Port
{
    public int Id { get; set; }
    public string Type { get; set; }
    public int Number { get; set; }

    public Port()
    {

    }

    public Port(string type, int number)
    {
        Type = type;
        Number = number;
    }
}
