using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HdSimpleMatrial
{
    public interface DbInterface
    {
        bool AddNew();
        bool Edit();
        bool Delete();
        void ReadFromDbByID(int id);
    }
}
