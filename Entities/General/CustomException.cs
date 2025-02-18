using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.General
{
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException(string detail) : base(detail) { }
    }
}
