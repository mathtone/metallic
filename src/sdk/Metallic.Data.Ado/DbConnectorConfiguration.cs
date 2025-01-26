using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metallic.Data.Ado;

public abstract class DbConnectorConfiguration {
	public string? ConnectionString { get; set; }
}
