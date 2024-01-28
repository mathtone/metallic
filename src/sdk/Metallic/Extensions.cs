using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metallic {
	public static class Extensions {
		public static O Process<I, O>(this I input, Func<I, O> processor) => processor(input);
	}
}