using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metallic.Data;

public interface IRepository<ID, ITEM> : IRepository<ID, ITEM, ITEM> { }
public interface IRepository<ID, CREATE, ITEM> : ICreate<CREATE, ITEM>, IRead<ID, ITEM>, IUpdate<ITEM>, IDelete<ID> { }
