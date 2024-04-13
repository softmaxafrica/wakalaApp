using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mobilewakala.Models
{
    public class ApiResponse<T>
    {
        public int TotalCount { get; set; }
        public T Data { get; set; }
        public List<T> DataList { get; set; }
    }
}
