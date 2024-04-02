using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISFA.MVVM.Models
{
    static class PaulAnger
    {
	    private static List<List<State>> _states = [];

	    public static void Calculate(List<List<State>> states)
	    {
		    _states = states;


	    }
    }
}
