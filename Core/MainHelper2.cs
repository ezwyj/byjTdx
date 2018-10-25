using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Core
{
    public class MainHelper2
    {
        BroadcastBlock<Bitmap> bb = new BroadcastBlock<Bitmap>((i) => { return i; });

        ActionBlock<int> displayBlock = new ActionBlock<int>((i) => Console.WriteLine("Displayed " + i));


    }
}
